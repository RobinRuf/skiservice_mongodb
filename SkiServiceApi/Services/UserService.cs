using skiservice.Common;
using skiservice.Interfaces;
using skiservice.Models;
using MongoDB.Driver;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace skiservice.Services
{
    public class UserService : IUserService
    {
        private readonly IMongoCollection<UserModel> _users;

        public UserService(IMongoDatabase database)
        {
            _users = database.GetCollection<UserModel>("Users");
        }

        public void CreateUser(string username, string password, Roles role)
        {
            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

            var user = new UserModel
            {
                UserName = username,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Role = role
            };

            _users.InsertOne(user);
        }

        public bool VerifyPassword(string username, string password)
        {
            var filter = Builders<UserModel>.Filter.Eq(u => u.UserName, username);
            var user = _users.Find(filter).FirstOrDefault();

            if (user == null)
                throw new ArgumentException("Benutzername nicht gefunden.");

            if (user.IsLocked)
                throw new InvalidOperationException("Benutzerkonto ist gesperrt.");

            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                var update = Builders<UserModel>.Update
                            .Inc(u => u.PasswordInputAttempt, 1);

                _users.UpdateOne(filter, update);

                if (user.PasswordInputAttempt >= 3)
                {
                    update = Builders<UserModel>.Update.Set(u => u.IsLocked, true);
                    _users.UpdateOne(filter, update);
                    throw new InvalidOperationException("Benutzerkonto wurde wegen zu vieler fehlgeschlagener Versuche gesperrt.");
                }

                int remainingAttempts = 3 - user.PasswordInputAttempt;
                throw new ArgumentException($"Falsches Passwort. Verbleibende Versuche: {remainingAttempts}");
            }

            // Successful password entry
            var resetAttempts = Builders<UserModel>.Update.Set(u => u.PasswordInputAttempt, 0);
            _users.UpdateOne(filter, resetAttempts);
            return true;
        }

        public void UnlockUser(string username)
        {
            var filter = Builders<UserModel>.Filter.Eq(u => u.UserName, username);
            var update = Builders<UserModel>.Update.Set(u => u.IsLocked, false).Set(u => u.PasswordInputAttempt, 0);

            var result = _users.UpdateOne(filter, update);
            if (result.MatchedCount == 0)
                throw new ArgumentException("Benutzername nicht gefunden.");
        }

        public UserModel GetUserByUsername(string userName)
        {
            var filter = Builders<UserModel>.Filter.Eq(u => u.UserName, userName);
            return _users.Find(filter).FirstOrDefault();
        }

        /// <summary>
        /// creates a password hash for a user
        /// </summary>
        /// <param name="password">password of the user</param>
        /// <param name="passwordHash">password hash of the user</param>
        /// <param name="passwordSalt">password salt of the user</param>
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
    }
}

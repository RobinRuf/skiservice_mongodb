using skiservice.Common;
using skiservice.Interfaces;
using skiservice.Models;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace skiservice.Services
{
    public class AssignService : IAssignService
    {
        private readonly IMongoCollection<ServiceOrderModel> _serviceOrders;
        private readonly IMongoCollection<UserModel> _users;

        public AssignService(IMongoDatabase database)
        {
            _serviceOrders = database.GetCollection<ServiceOrderModel>("ServiceOrders");
            _users = database.GetCollection<UserModel>("Users");
        }

        public async Task AssignServiceOrderToUser(string serviceOrderId, string userId, string? currentUserName)
        {
            var serviceOrderFilter = Builders<ServiceOrderModel>.Filter.Eq(sr => sr.Id, serviceOrderId);
            var serviceOrder = await _serviceOrders.Find(serviceOrderFilter).FirstOrDefaultAsync();

            var currentUserFilter = Builders<UserModel>.Filter.Eq(u => u.UserName, currentUserName);
            var currentUser = await _users.Find(currentUserFilter).FirstOrDefaultAsync();

            if (serviceOrder == null || currentUser == null)
            {
                throw new KeyNotFoundException("ServiceOrder oder Benutzer nicht gefunden.");
            }

            // Zusätzliche Logik basierend auf dem aktuellen Benutzer
            if (currentUserName != null && serviceOrder.UserId == currentUser.Id && currentUser.Role != Roles.ADMIN)
            {
                throw new UnauthorizedAccessException("Sie haben keine Berechtigung, diesen Auftrag neu zuzuweisen. Bitte kontaktieren Sie einen Administrator.");
            }

            var update = Builders<ServiceOrderModel>.Update.Set(sr => sr.UserId, userId);
            await _serviceOrders.UpdateOneAsync(serviceOrderFilter, update);
        }

        public async Task AssignServiceOrderToUser(string serviceOrderId, string userId)
        {
            var serviceOrderFilter = Builders<ServiceOrderModel>.Filter.Eq(sr => sr.Id, serviceOrderId);
            var serviceOrder = await _serviceOrders.Find(serviceOrderFilter).FirstOrDefaultAsync();

            if (serviceOrder == null)
            {
                throw new KeyNotFoundException($"ServiceOrder mit der ID {serviceOrderId} wurde nicht gefunden.");
            }

            var update = Builders<ServiceOrderModel>.Update.Set(sr => sr.UserId, userId);
            await _serviceOrders.UpdateOneAsync(serviceOrderFilter, update);
        }
    }
}

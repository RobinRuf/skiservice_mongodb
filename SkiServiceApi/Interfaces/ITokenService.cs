namespace skiservice.Interfaces
{
    /// <summary>
    /// Interface defining the contract for the token generation service
    /// </summary>
    public interface ITokenService
    {
        string CreateToken(string username, string role);
    }
}
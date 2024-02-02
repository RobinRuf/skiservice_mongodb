namespace skiservice.Interfaces
{
    public interface IAssignService
    {
        // Task AssignServiceOrderToUser(string id, string userId, string? currentUserName);
        Task AssignServiceOrderToUser(string id, string userId);
    }
}

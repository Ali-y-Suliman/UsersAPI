namespace Users.Repositories.UserRepository
{
    public interface IUserRepository
    {
        Task<User?> GetUserByEmailAsync(string email);
        Task AddUserAsync(User user);
        Task<bool> CheckUserExictenceAsync(string email);
    }
}

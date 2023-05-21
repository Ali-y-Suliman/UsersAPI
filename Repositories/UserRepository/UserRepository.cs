namespace Users.Repositories.UserRepository
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;

        public UserRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(c => c.email == email);
        }

        public async Task AddUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> CheckUserExictenceAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.email.ToLower() == email.ToLower());
        }

        // You can also include other user-related database operations here
    }
}
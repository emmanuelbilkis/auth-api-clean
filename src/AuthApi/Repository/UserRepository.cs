using AuthApi.Data;
using AuthApi.Models;

namespace AuthApi.Repository
{
    public class UserRepository
    {
        private readonly AppDbContext _context; 
        public UserRepository(AppDbContext context) 
        {
            _context = context; 
        } 

        public User Register(User newUser) 
        {
            try
            {
                _context.Users.Add(newUser);
                return newUser;
            }
            catch (Exception ex)
            {
                return null; 
            }
        }
    }
}

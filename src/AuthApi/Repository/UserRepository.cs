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

        public UserModel Register(UserModel newUser) 
        {
            try
            {
                _context.Users.Add(newUser);
                _context.SaveChanges(); 
                return newUser;
            }
            catch (Exception ex)
            {
                return null; 
            }
        }

        public List<UserModel> GetAll()
        {
            try
            {
                return _context.Users.ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}

using Cotizaciones.Models;
namespace Cotizaciones.Data
{
    public class UserRepository : IUserRepository
    {
        DataContext _entityFramework;
        public UserRepository(IConfiguration config)
        {
            _entityFramework = new DataContext(config);
        }
        public bool SaveChanges()
        {
            return _entityFramework.SaveChanges() > 0;
        }
        public void AddEntity<T>(T entityToAdd)
        {
            if (entityToAdd != null)
            {
                _entityFramework.Add(entityToAdd);
            }
        }
        public void RemoveEntity<T>(T entityToRemove)
        {
            if (entityToRemove != null)
            {
                _entityFramework.Remove(entityToRemove);
            }
        }
        public IEnumerable<User> GetUsers()
        {

            IEnumerable<User> users = _entityFramework.Users.ToList<User>();
            return users;
        }
        public User GetSingleUser(int userId)
        {
            User? user = _entityFramework.Users?
                .Where(u => u.UserId == userId)
                .FirstOrDefault();
            if (user != null)
            {
                return user;
            }
            throw new Exception("FAILED TO GET A USER");
        }

        public User GetExistingUser(string email)
        {
            User? user = _entityFramework.Users.Where(x => x.Email == email).FirstOrDefault();
            return user;
        }

        public Auth GetAuthInfo(string email)
        {
            Auth? authInfo = _entityFramework.Auth.FirstOrDefault(u => u.Email == email);
            if (authInfo != null)
            {
                return authInfo;
            }
            throw new Exception("Could not find existing user");
        }


    }


}
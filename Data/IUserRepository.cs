using Cotizaciones.Models;
namespace Cotizaciones.Data
{
    public interface IUserRepository
    {
        public bool SaveChanges();
        public void AddEntity<T>(T entityToAdd);
        public void RemoveEntity<T>(T entityToAdd);
        public IEnumerable<User> GetUsers();
        public User GetSingleUser(int userId);
        public User GetExistingUser(string email);
        public Auth GetAuthInfo(string email);
    }
}
using Microsoft.EntityFrameworkCore;
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
        // public CotizacionTotal GetSingleCotizacionTotalByUser(int userId);
        // public IEnumerable<CotizacionTotal> GetCotizacionesTotal();
        // public CotizacionOferta GetSingleCotizacionOfertaByUser(int userId);
        // public IEnumerable<CotizacionOferta> GetCotizacionesOferta();
        // public CotizacionTotal GetSingleCotizacionTotal(int transactionId);
        // public CotizacionOferta GetSingleCotizacionOferta(int transactionId);
        // public IEnumerable<CotizacionOferta> GetLastFiveOferta();
        // public IEnumerable<CotizacionTotal> GetLastFiveTotal();
        public User GetExistingUser(string email);
        public Auth GetAuthInfo(string email);
        // public CotizacionTotal GetLastTotal();
        // public CotizacionOferta GetLastOferta();
    }
}
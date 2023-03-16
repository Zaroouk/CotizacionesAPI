using Cotizaciones.Models;

namespace Cotizaciones.Data
{
    public class TotalRepository : ITotalRepository
    {
        DataContext _entityFramework;
        public TotalRepository(IConfiguration config)
        {
            _entityFramework = new DataContext(config);
        }
        public CotizacionTotal GetSingleCotizacionTotalByUser(int userId)
        {
            CotizacionTotal? cotizacionTotal = _entityFramework.CotizacionesTotal
                .Where(u => u.UserId == userId)
                .FirstOrDefault<CotizacionTotal>();
            if (cotizacionTotal != null)
            {
                return cotizacionTotal;
            }
            throw new Exception("Failed to get cotizaciones for this user");

        }
        public IEnumerable<CotizacionTotal> GetCotizacionesTotal()
        {
            IEnumerable<CotizacionTotal> cotizaciones = _entityFramework.CotizacionesTotal.ToList<CotizacionTotal>();
            return cotizaciones;
        }


        public CotizacionTotal GetSingleCotizacionTotal(int transactionId)
        {
            CotizacionTotal? cotizacionTotal = _entityFramework.CotizacionesTotal
                .Where(u => u.TransactionId == transactionId)
                .FirstOrDefault<CotizacionTotal>();
            if (cotizacionTotal != null)
            {
                return cotizacionTotal;
            }
            throw new Exception("Failed to get a cotizacion");

        }



        public IEnumerable<CotizacionTotal> GetLastFiveTotal()
        {
            IEnumerable<CotizacionTotal> cotizacionesTotal = _entityFramework.CotizacionesTotal
                        .OrderByDescending(x => x.TransactionId)
                        .Take(5)
                        .ToList();
            return cotizacionesTotal;
        }
        public CotizacionTotal GetLastTotal()
        {
            CotizacionTotal? cotizacion = _entityFramework.CotizacionesTotal
                .OrderByDescending(x => x.TransactionId).FirstOrDefault();
            if(cotizacion != null)
            {
                return cotizacion;
            }
            throw new Exception("Could not find existing user");
        }

    }
}
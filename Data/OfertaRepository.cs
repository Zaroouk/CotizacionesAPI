using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cotizaciones.Models;

namespace Cotizaciones.Data
{
    public class OfertaRepository : IOfertaRepository
    {
        DataContext _entityFramework;
        public OfertaRepository(IConfiguration config)
        {
            _entityFramework = new DataContext(config);
        }
        public CotizacionOferta GetLastOferta()
        {
            CotizacionOferta? cotizacion = _entityFramework.CotizacionesOferta
                .OrderByDescending(x => x.TransactionId).FirstOrDefault();
            if(cotizacion != null)
            {
                return cotizacion;
            }
            throw new Exception("Could not find existing user");
        }
        public IEnumerable<CotizacionOferta> GetLastFiveOferta()
        {
            IEnumerable<CotizacionOferta> cotizacionesOferta = _entityFramework.CotizacionesOferta
                        .OrderByDescending(x => x.TransactionId)
                        .Take(5)
                        .ToList();
            return cotizacionesOferta;
        }
        public CotizacionOferta GetSingleCotizacionOferta(int transactionId)
        {
            CotizacionOferta? cotizacionTotal = _entityFramework.CotizacionesOferta
                .Where(u => u.TransactionId == transactionId)
                .FirstOrDefault<CotizacionOferta>();
            if (cotizacionTotal != null)
            {
                return cotizacionTotal;
            }
            throw new Exception("Failed to get a cotizacion");

        }
        public IEnumerable<CotizacionOferta> GetCotizacionesOferta()
        {

            IEnumerable<CotizacionOferta> cotizaciones = _entityFramework.CotizacionesOferta.ToList<CotizacionOferta>();
            return cotizaciones;
        }
        public CotizacionOferta GetSingleCotizacionOfertaByUser(int userId)
        {
            CotizacionOferta? cotizacionOferta = _entityFramework.CotizacionesOferta
                .Where(u => u.UserId == userId)
                .FirstOrDefault<CotizacionOferta>();
            if (cotizacionOferta != null)
            {
                return cotizacionOferta;
            }
            throw new Exception("Failed to Get a Cotizacion");
        }
    }
}
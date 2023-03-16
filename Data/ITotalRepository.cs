using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cotizaciones.Models;

namespace Cotizaciones.Data
{
    public interface ITotalRepository
    {
        public CotizacionTotal GetSingleCotizacionTotalByUser(int userId);
        public IEnumerable<CotizacionTotal> GetCotizacionesTotal();
        public CotizacionTotal GetSingleCotizacionTotal(int transactionId);
        public IEnumerable<CotizacionTotal> GetLastFiveTotal();
        public CotizacionTotal GetLastTotal();

    }
}
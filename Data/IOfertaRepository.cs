using Cotizaciones.Models;

namespace Cotizaciones.Data
{
    public interface IOfertaRepository
    {
        public CotizacionOferta GetLastOferta();
        public IEnumerable<CotizacionOferta> GetLastFiveOferta();
        public CotizacionOferta GetSingleCotizacionOferta(int transactionId);
        public IEnumerable<CotizacionOferta> GetCotizacionesOferta();
        public CotizacionOferta GetSingleCotizacionOfertaByUser(int userId);

    }
}
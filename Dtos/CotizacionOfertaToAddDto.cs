namespace Cotizaciones.Dtos
{
    public class CotizacionOfertaToAddDto
    {
        // public int TransactionId { get; set; }
        public int UserId { get; set; }
        public string Company { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Date { get; set; } = string.Empty;
        public string Quantity { get; set; } = string.Empty;
        public string Price { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string Product { get; set; } = string.Empty;
        public string OffPrice { get; set; } = string.Empty;
        public string DeliveryETA { get; set; } = string.Empty;
        public int OfferDuration { get; set; }
        public string PaymentDetails { get; set; } = string.Empty;
        public string org {get;set;} = string.Empty;
}
}
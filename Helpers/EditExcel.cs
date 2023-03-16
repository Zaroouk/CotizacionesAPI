using Aspose.Cells;
using Cotizaciones.Dtos;
using Cotizaciones.Models;

namespace Cotizaciones.Helpers
{
    public class EditExcel
    {
        private readonly IConfiguration _config;

        public EditExcel(IConfiguration config)
        {
            _config = config;
        }
        public byte[] CreateTotal(CotizacionTotal cotizacion){
            // Instantiate the Workbook object with the Excel file
            string template = "Helpers/Formato Myers.xlsx";
            if(cotizacion.Org == "Alfa")
            {
                template = "Helpers/Formato Alfa y Omega.xlsx";
            }
            Workbook workbook = new Workbook(template);

            Worksheet worksheet = workbook.Worksheets[0];

            Cell company = worksheet.Cells["A10"];
            company.PutValue("Sres.: " + cotizacion.Company);

            Cell name = worksheet.Cells["A11"];
            name.PutValue("Atencion: " + cotizacion.Name);

            Cell date = worksheet.Cells["F9"];
            date.PutValue("Fecha: " + cotizacion.Date);


            string[] code = cotizacion.Code.Split('~');
            string[] product = cotizacion.Product.Split('~');
            string[] quantity = cotizacion.Quantity.Split('~');
            string[] price = cotizacion.Price.Split('~');
            // string[] priceOff = cotizacion.TotalPrice.Split('~');

            //create cells to be edited
            int celda = 15;
            for(int i = 0; i < code.Length; i++)
            {
                Cell cellCode = worksheet.Cells[$"A{celda}"];
                cellCode.PutValue(code[i]);

                Cell cellProduct = worksheet.Cells[$"B{celda}"];
                cellProduct.PutValue(product[i]);

                Cell cellQuantity = worksheet.Cells[$"G{celda}"];
                int quantityInt = int.Parse(quantity[i]);
                cellQuantity.PutValue(quantityInt);

                Cell cellPrice = worksheet.Cells[$"H{celda}"];
                double priceDouble = double.Parse(price[i]);
                cellPrice.PutValue(priceDouble);

                // Cell cellOfferPrice = worksheet.Cells[$"I{celda}"];
                // cellOfferPrice.PutValue("$" + priceOff[i]);

                celda++;
            }


            Cell deliveryETA = worksheet.Cells["A38"];
            deliveryETA.PutValue("*Tiempo de entrega    :  " + cotizacion.DeliveryETA);
            Cell offerDuration = worksheet.Cells["A40"];
            offerDuration.PutValue("*Validez de la Oferta  :  " + cotizacion.OfferDuration);
            Cell paymentDetails = worksheet.Cells["A41"];
            paymentDetails.PutValue("*Forma de Pago : " + cotizacion.PaymentDetails);

            workbook.CalculateFormula();


            MemoryStream pdfStream = new MemoryStream();
            workbook.Save(pdfStream, SaveFormat.Pdf);
            byte[] data = pdfStream.ToArray();

            return data;

        }
        public byte[] CreateOferta(CotizacionOferta cotizacion){
            // Instantiate the Workbook object with the Excel file
            string template = "Helpers/Formato Myers.xlsx";
            if(cotizacion.Org == "Alfa")
            {
                template = "Helpers/Formato Alfa y Omega.xlsx";
            }
            Workbook workbook = new Workbook(template);

            Worksheet worksheet = workbook.Worksheets[0];


            Cell company = worksheet.Cells["A10"];
            company.PutValue("Sres.: " + cotizacion.Company);

            Cell name = worksheet.Cells["A11"];
            name.PutValue("Atencion: " + cotizacion.Name);

            Cell date = worksheet.Cells["F9"];
            date.PutValue("Fecha: " + cotizacion.Date);

            Cell unit = worksheet.Cells["H14"];
            unit.PutValue("Precio");
            Cell oferta = worksheet.Cells["I14"];
            oferta.PutValue("Oferta");


            string[] code = cotizacion.Code.Split('~');
            string[] product = cotizacion.Product.Split('~');
            string[] quantity = cotizacion.Quantity.Split('~');
            string[] price = cotizacion.Price.Split('~');
            string[] priceOff = cotizacion.OffPrice.Split('~');

            //create cells to be edited
            int celda = 15;
            for(int i = 0; i < code.Length; i++)
            {
                int quantityInt = int.Parse(quantity[i]);
                if(quantityInt == 0)
                {
                Cell codeEmpty = worksheet.Cells[$"A{celda}"];
                codeEmpty.PutValue("");

                Cell productEmpty = worksheet.Cells[$"B{celda}"];
                productEmpty.PutValue("");

                Cell quantityEmpty = worksheet.Cells[$"G{celda}"];
                quantityEmpty.PutValue("");

                Cell priceEmpty = worksheet.Cells[$"H{celda}"];
                // double pricedouble = double.Parse(price[i]);
                priceEmpty.PutValue("");

                Cell offeEmpty = worksheet.Cells[$"I{celda}"];
                // double ofertaDouble = double.Parse(priceOff[i]);
                offeEmpty.PutValue("");

                celda++;
                }else{
                Cell cellCode = worksheet.Cells[$"A{celda}"];
                cellCode.PutValue(code[i]);

                Cell cellProduct = worksheet.Cells[$"B{celda}"];
                cellProduct.PutValue(product[i]);

                Cell cellQuantity = worksheet.Cells[$"G{celda}"];
                cellQuantity.PutValue(quantityInt);

                Cell cellPrice = worksheet.Cells[$"H{celda}"];
                double pricedouble = double.Parse(price[i]);
                cellPrice.PutValue(pricedouble);

                Cell cellOfferPrice = worksheet.Cells[$"I{celda}"];
                double ofertaDouble = double.Parse(priceOff[i]);
                cellOfferPrice.PutValue(ofertaDouble);

                celda++;
                }
            }

            workbook.CalculateFormula();

            Cell deliveryETA = worksheet.Cells["A38"];
            deliveryETA.PutValue("*Tiempo de entrega    :  " + cotizacion.DeliveryETA);
            Cell offerDuration = worksheet.Cells["A40"];
            offerDuration.PutValue("*Validez de la Oferta  :  " + cotizacion.OfferDuration);
            Cell paymentDetails = worksheet.Cells["A41"];
            paymentDetails.PutValue("*Forma de Pago : " + cotizacion.PaymentDetails);

            MemoryStream pdfStream = new MemoryStream();
            workbook.Save(pdfStream, SaveFormat.Pdf);
            byte[] data = pdfStream.ToArray();

            return data;

        }
    }

}
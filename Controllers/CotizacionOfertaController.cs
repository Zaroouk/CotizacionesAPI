using AutoMapper;
using Cotizaciones.Data;
using Cotizaciones.Models;
using Cotizaciones.Dtos;
using Microsoft.AspNetCore.Mvc;
using Cotizaciones.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace Cotizaciones.Controllers;

[ApiController]
[Route("Api/CotizacionesOferta")]
[Authorize(Roles = "User")]
public class CotizacionOfertaController : ControllerBase
{
    IUserRepository _userRepository;
    IOfertaRepository _ofertaRepository;
    IMapper _mapper;
    private readonly EditExcel _editHelper;

    public CotizacionOfertaController(IConfiguration config, IUserRepository userRepository, IOfertaRepository ofertaRepository)
    {
        _userRepository = userRepository;
        _ofertaRepository = ofertaRepository;

        _mapper = new Mapper(new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<CotizacionOfertaToAddDto, CotizacionOferta>();
        }));
        _editHelper = new EditExcel(config);

    }

    //? GET METHODS
    [HttpGet("GetLast")]
    public IActionResult GetLast()
    {
        CotizacionOferta cotizacion = _ofertaRepository.GetLastOferta();
        // Get the PDF file data
        byte[] pdfData = _editHelper.CreateOferta(cotizacion);

        // Set the content type and headers for the response
        Response.ContentType = "application/pdf";
        Response.Headers.Add("content-disposition", "attachment; filename=Cotizacion.pdf");

        // Write the PDF data to the response stream
        return File(pdfData, "application/pdf");
    }
    [HttpGet("downloadPDF/{transactionId}")]
    public IActionResult downloadPDF(int transactionId)
    {
        CotizacionOferta cotizacion = _ofertaRepository.GetSingleCotizacionOferta(transactionId);
        // Get the PDF file data
        byte[] pdfData = _editHelper.CreateOferta(cotizacion);

        // Set the content type and headers for the response
        Response.ContentType = "application/pdf";
        Response.Headers.Add("content-disposition", "attachment; filename=Cotizacion.pdf");

        // Write the PDF data to the response stream
        return File(pdfData, "application/pdf");
    }
    [HttpGet("GetCotizacionesOferta")]
    public IEnumerable<CotizacionOferta> GetCotizacionesOferta()
    {
        IEnumerable<CotizacionOferta> cotizaciones = _ofertaRepository.GetCotizacionesOferta();
        if (cotizaciones != null)
        {
            return cotizaciones;
        }
        throw new Exception("Failed to getcotizaciones");

    }
    [HttpGet("GetCotizacionOferta/{transactionId}")]
    public CotizacionOferta GetCotizacionOferta(int transactionId)
    {
        CotizacionOferta cotizacionOferta = _ofertaRepository.GetSingleCotizacionOferta(transactionId);
        return cotizacionOferta;
    }
    [HttpGet("GetLastFiveOferta")]
    public IEnumerable<CotizacionOferta> GetLastFiveOferta()
    {
        IEnumerable<CotizacionOferta> cotizacionOferta = _ofertaRepository.GetLastFiveOferta();
        return cotizacionOferta;
    }
    //** POST METHODS
    [HttpPost("Create")]
    public IActionResult CreateCotizacionOferta(CotizacionOfertaToAddDto totalToAdd)
    {
        CotizacionOferta cTotalDb = _mapper.Map<CotizacionOferta>(totalToAdd);
        _userRepository.AddEntity<CotizacionOferta>(cTotalDb);
        if (_userRepository.SaveChanges())
        {
            return Ok();
        }
        throw new Exception("Failed to add user");
    }


    //Todo PUT METHOD
    [HttpPut("EditCotizacionOferta")]
    public IActionResult EditCotizacionOferta(CotizacionOferta cotizacion)
    {
        CotizacionOferta cDb = _ofertaRepository.GetSingleCotizacionOferta(cotizacion.TransactionId);
        if (cDb != null)
        {
            cDb.Company = cotizacion.Company;
            cDb.Name = cotizacion.Name;
            cDb.Date = cotizacion.Date;
            cDb.Quantity = cotizacion.Quantity;
            cDb.Price = cotizacion.Price;
            cDb.Code = cotizacion.Code;
            cDb.Product = cotizacion.Product;
            cDb.OffPrice = cotizacion.OffPrice;
            cDb.DeliveryETA = cotizacion.DeliveryETA;
            cDb.OfferDuration = cotizacion.OfferDuration;
            cDb.PaymentDetails = cotizacion.PaymentDetails;
            if (_userRepository.SaveChanges())
            {
                return Ok();
            }
            throw new Exception("Failed to Update Cotizacion");
        }
        throw new Exception("Failed to get a Cotizacion");
    }



    //! DELETE METHOD
    [HttpDelete("DeleteCotizacionOferta/{transactionId}")]
    public IActionResult DeleteCotizacionOferta(int transactionId)
    {
        CotizacionOferta ofertaDb = _ofertaRepository.GetSingleCotizacionOferta(transactionId);
        if (ofertaDb != null)
        {
            _userRepository.RemoveEntity<CotizacionOferta>(ofertaDb);
            _userRepository.SaveChanges();
            return Ok();
        }
        throw new Exception("Failed to Delete Cotizacion");
    }
}

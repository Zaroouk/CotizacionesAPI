using AutoMapper;
using Cotizaciones.Data;
using Cotizaciones.Models;
using Cotizaciones.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Cotizaciones.Helpers;

namespace Cotizaciones.Controllers;

[ApiController]
[Route("Api/CotizacionesTotal")]
[Authorize(Roles = "User")]
public class CotizacionTotalController : ControllerBase
{
    IUserRepository _userRepository;
    ITotalRepository _totalRepository;
    IMapper _mapper;
    private readonly EditExcel _editHelper;
    public CotizacionTotalController(IConfiguration config, IUserRepository userRepository, ITotalRepository totalRepository)
    {
        _userRepository = userRepository;
        _totalRepository = totalRepository;

        _mapper = new Mapper(new MapperConfiguration(cfg =>
        {
            // cfg.CreateMap<UserToAddDto, User>();
            cfg.CreateMap<CotizacionTotalToAddDto, CotizacionTotal>();
            // cfg.CreateMap<CotizacionOfertaToAddDto, CotizacionOferta>();
        }));
        _editHelper = new EditExcel(config);
    }

    //? GET METHODS
    [HttpGet("GetLast")]
    public IActionResult GetLastCotizacion()
    {

        CotizacionTotal cotizacion = _totalRepository.GetLastTotal();
        if (cotizacion != null)
        {

                // Get the PDF file data
                byte[] pdfData = _editHelper.CreateTotal(cotizacion);

                // Set the content type and headers for the response
                Response.ContentType = "application/pdf";
                Response.Headers.Add("content-disposition", "attachment; filename=Cotizacion.pdf");

                // Write the PDF data to the response stream
                return File(pdfData, "application/pdf");
        }
        throw new Exception("could not download a cotizacion");
    }
    [HttpGet("downloadPDF/{transactionId}")]
    public IActionResult downloadPDF(int transactionId)
    {
        CotizacionTotal cotizacion = _totalRepository.GetSingleCotizacionTotal(transactionId);
        // Get the PDF file data
        byte[] pdfData = _editHelper.CreateTotal(cotizacion);

        // Set the content type and headers for the response
        Response.ContentType = "application/pdf";
        Response.Headers.Add("content-disposition", "attachment; filename=Cotizacion.pdf");

        // Write the PDF data to the response stream
        return File(pdfData, "application/pdf");
    }
    [HttpGet("GetCotizacionesTotal")]
    public IEnumerable<CotizacionTotal> GetCotizacionesTotal()
    {
        IEnumerable<CotizacionTotal> cotizaciones = _totalRepository.GetCotizacionesTotal();
        if (cotizaciones != null)
        {
            return cotizaciones;
        }
        throw new Exception("Failed to get Cotizaciones");
    }


    [HttpGet("GetCotizacionTotal/{transactionId}")]
    public CotizacionTotal GetCotizacionTotal(int transactionId)
    {
        CotizacionTotal cotizacionTotal = _totalRepository.GetSingleCotizacionTotal(transactionId);
        return cotizacionTotal;
    }


    [HttpGet("GetLastFiveTotal")]
    public IEnumerable<CotizacionTotal> GetLastFiveTotal()
    {
        IEnumerable<CotizacionTotal> cotizacionTotal = _totalRepository.GetLastFiveTotal();
        return cotizacionTotal;
    }

    //** POST METHODS

    [HttpPost("Create")]
    public IActionResult CreateCotizacionTotal(CotizacionTotalToAddDto totalToAdd)
    {

        try
        {
            CotizacionTotal cTotalDb = _mapper.Map<CotizacionTotal>(totalToAdd);
            _userRepository.AddEntity<CotizacionTotal>(cTotalDb);
            if (_userRepository.SaveChanges() == true)
            {
                return Ok();
            }
            throw new Exception("Failed to add user");
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"Error adding entity to database: {ex.Message}");
        }
    }

    //Todo PUT METHOD

    [HttpPut("EditCotizacionTotal")]
    public IActionResult EditCotizacionTotal(CotizacionTotal cTotal)
    {
        CotizacionTotal cTotalDb = _totalRepository.GetSingleCotizacionTotal(cTotal.TransactionId);
        if (cTotalDb != null)
        {
            cTotalDb.Company = cTotal.Company;
            cTotalDb.Name = cTotal.Name;
            cTotalDb.Date = cTotal.Date;
            cTotalDb.Quantity = cTotal.Quantity;
            cTotalDb.Price = cTotal.Price;
            cTotalDb.Code = cTotal.Code;
            cTotalDb.Product = cTotal.Product;
            cTotalDb.TotalPrice = cTotal.TotalPrice;
            cTotalDb.DeliveryETA = cTotal.DeliveryETA;
            cTotalDb.OfferDuration = cTotal.OfferDuration;
            cTotalDb.PaymentDetails = cTotal.PaymentDetails;
            if (_userRepository.SaveChanges())
            {
                return Ok();
            }
            throw new Exception("Failed to Update Cotizacion");
        }
        throw new Exception("Failed to get a Cotizacion");
    }


    //! DELETE METHOD

    [HttpDelete("DeleteCotizacionTotal/{transactionId}")]
    public IActionResult DeleteCotizacionTotal(int transactionId)
    {
        CotizacionTotal cTotalDb = _totalRepository.GetSingleCotizacionTotal(transactionId);
        if (cTotalDb != null)
        {
            _userRepository.RemoveEntity<CotizacionTotal>(cTotalDb);
            _userRepository.SaveChanges();
            return Ok();
        }
        throw new Exception("Failed to Delete Cotizacion");
    }
}

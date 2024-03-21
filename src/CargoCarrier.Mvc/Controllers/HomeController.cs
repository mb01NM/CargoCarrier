using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CargoCarrier.Mvc.Models;
using CargoCarrier.Mvc.Services;

namespace CargoCarrier.Mvc.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [HttpPost]
    public IActionResult ParcelSizes(int smallParcels, int mediumParcels, int largeParcels)
    {
        // Log the received parcel sizes
        _logger.LogInformation($"Received parcel sizes: Small - {smallParcels}, Medium - {mediumParcels}, Large - {largeParcels}");
        
        var calculator = new TruckLoadCalculator();
        var (smallTrucks, largeTrucks) = calculator.CalculateTruckLoads(smallParcels, mediumParcels, largeParcels);

        ViewBag.SmallTrucks = smallTrucks;
        ViewBag.LargeTrucks = largeTrucks;
        return View("ParcelSizes");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

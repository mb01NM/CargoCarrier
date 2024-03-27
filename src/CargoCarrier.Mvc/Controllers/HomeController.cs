using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CargoCarrier.Mvc.Models;
using CargoCarrier.Mvc.Database;

namespace CargoCarrier.Mvc.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly TripDatabase _tripDatabase;

    public HomeController(ILogger<HomeController> logger,  TripDatabase tripDatabase)
    {
        _logger = logger;
        _tripDatabase = tripDatabase;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public IActionResult ParcelSizes(int smallParcels, int mediumParcels, int largeParcels)
    {
        // Log the received parcel sizes
        _logger.LogInformation($"Received parcel sizes: Small - {smallParcels}, Medium - {mediumParcels}, Large - {largeParcels}");

        // TODO: Add your logic here to handle the parcel sizes
        // Large trucks can carry 3 large parcels or 6 medium parcels or 12 small parcels
        // Small trucks can carry 1 large parcel or 2 medium parcels or 4 small parcels

        ViewBag.SmallTrucks = 1;
        ViewBag.LargeTrucks = 1;
        return View("ParcelSizes");
    }

    // Test via http://localhost:5152/home/TripsWithParcelSizeLessThan?maxParcelSize=15 using curl/browser/etc.
    [HttpGet]
    public Task<List<Trip>> TripsWithParcelSizeLessThan(string maxParcelSize)
    {
        var trips = _tripDatabase.GetTripsWithParcelSizeLessThanAsync(maxParcelSize);
        return trips;
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

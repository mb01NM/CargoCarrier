using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CargoCarrier.Mvc.Models;
using CargoCarrier.Mvc.Database;
using System.Text.Json;
using System.Text.Json.Nodes;

using CargoCarrier.Mvc.Services;
namespace CargoCarrier.Mvc.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly TripDatabase _tripDatabase;
    private static readonly HttpClient httpClient = new();

    public HomeController(ILogger<HomeController> logger, TripDatabase tripDatabase)
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
        
        var calculator = new TruckLoadCalculator();
        var (smallTrucks, largeTrucks) = calculator.CalculateTruckLoads(smallParcels, mediumParcels, largeParcels);

        ViewBag.SmallTrucks = smallTrucks;
        ViewBag.LargeTrucks = largeTrucks;
        return View("ParcelSizes");
    }

    // Test via http://localhost:5152/home/TripsWithParcelSizeLessThan?maxParcelSize=15 using curl/browser/etc.
    [HttpGet]
    public Task<List<Trip>> TripsWithParcelSizeLessThan(int maxParcelSize)
    {
        var trips = _tripDatabase.GetTripsWithParcelSizeLessThanAsync(maxParcelSize);
        return trips;
    }

    public async Task<IActionResult> Pokemon()
    {
        try
        {
            // Generate a random number between 1 and 898
            var randomId = new Random().Next(1, 899);

            // Construct the URL for the API call
            var url = $"https://pokeapi.co/api/v2/pokemon/{randomId}";

            // Make the GET request
            var response = await httpClient.GetAsync(url);

            // Check if the request was successful
            if (response.IsSuccessStatusCode)
            {
                // Parse the JSON response
                var responseBody = await response.Content.ReadAsStringAsync();
                var pokemonData = JsonSerializer.Deserialize<JsonNode>(responseBody);

                // Extract and return the name of the Pokemon
                return Ok(pokemonData?["name"]?.ToString());
            }
            else
            {
                // Handle the error
                return StatusCode((int)response.StatusCode, $"Failed to retrieve Pokemon data. Status code: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            // Handle any other exceptions
            return BadRequest($"An error occurred: {ex.Message}");
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

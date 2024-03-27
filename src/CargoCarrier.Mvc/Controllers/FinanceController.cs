using System;
using CargoCarrier.Mvc.Services;
using Microsoft.AspNetCore.Mvc;

namespace CargoCarrier.Mvc.Controllers
{
    public class FinanceController : Controller
    {
        private readonly ILogger<FinanceController> _logger;

        private readonly CostCalculationService _costCalculationService;

        public FinanceController(ILogger<FinanceController> logger, CostCalculationService costCalculationService)
        {
            _logger = logger;
            _costCalculationService = costCalculationService;
        }

        public IActionResult CostEstimator()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CalculateCost(decimal parcelAmount, bool isExpress, decimal distance, string voucherCode, bool isDangerous, DateTime startDate)
        {
            var totalCost = _costCalculationService.CalculateTotalPrice(parcelAmount, isExpress, distance, voucherCode, isDangerous, startDate);

            return totalCost < 0 ? BadRequest() : Json(new { result = totalCost });
        }

    }
}

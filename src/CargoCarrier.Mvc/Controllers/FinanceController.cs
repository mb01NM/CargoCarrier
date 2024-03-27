using System;
using Microsoft.AspNetCore.Mvc;

namespace CargoCarrier.Mvc.Controllers
{
    public class FinanceController : Controller
    {
        private readonly ILogger<FinanceController> _logger;

        public FinanceController(ILogger<FinanceController> logger)
        {
            _logger = logger;
        }

        public IActionResult CostEstimator()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CalculateCost(decimal parcelAmount, bool isExpress, decimal distance, string voucherCode, bool isDangerous, DateTime startDate)
        {
            try
            {
                // Validation
                if (distance < 0)
                {
                    return BadRequest();
                }

                // Calculation logic
                decimal basePrice = 10 + 1*parcelAmount;
                decimal expressFee = isExpress ? 5 : 0;
                decimal distanceFee = distance * 0.1m;
                decimal voucherDiscount = voucherCode == "DISCOUNT10" ? 10 : 0;
                decimal dangerousFee = isDangerous ? 15 : 0;

                // Check if the start date is on a weekend
                bool isWeekend = startDate.DayOfWeek == DayOfWeek.Saturday || startDate.DayOfWeek == DayOfWeek.Sunday;
                decimal weekendFee = isWeekend ? (basePrice + expressFee + distanceFee + dangerousFee) * 0.25m : 0;

                // Check for specific holidays
                Boolean isHoliday = false;
                if (startDate.Month == 12 && startDate.Day == 25)
                    isHoliday = true;
                if (startDate.Month == 1 && startDate.Day == 1)
                    isHoliday = true;
                if (startDate.Month == 4 && startDate.Day == 1)
                    isHoliday = true;

                decimal holidayFee = isHoliday ? (basePrice + expressFee + distanceFee + dangerousFee) * 0.50m : 0;

                // Calculate the total price
                decimal totalPrice = basePrice + expressFee + distanceFee - voucherDiscount + dangerousFee + weekendFee + holidayFee;
                totalPrice = Math.Round(totalPrice, 2);
                // Log the calculation details
                _logger.LogInformation($"Calculated price: {totalPrice} for parcel amount: {parcelAmount}, isExpress: {isExpress}, distance: {distance}, voucherCode: {voucherCode}, isDangerous: {isDangerous}, startDate: {startDate}, isWeekend: {isWeekend}, isHoliday: {isHoliday}");

                return Json(new { result = totalPrice });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating price.");
                return BadRequest();
            }
        }

    }
}

using System.Text.Json;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using CargoCarrier.Mvc.Controllers;

using Moq;
using CargoCarrier.Mvc.Services;

namespace CargoCarrier.Mvc.Tests.Services
{
    public class CostCalculationServiceTests
    {
        private readonly CostCalculationService _costCalculationService;

        public CostCalculationServiceTests()
        {
            var logger = new Mock<ILogger<FinanceController>>();
            _costCalculationService = new CostCalculationService();
        }

        [Fact]
        public void CalculateCost_WithNegativeDistance_ReturnsBadRequest()
        {
            // fails because validation didn't make it in the refactoring
            var result = _costCalculationService.CalculateTotalPrice(1, false, -1, "", false, DateTime.Now);
            Assert.Equal(0, result);
        }

        [Fact]
        public void CalculateCost_WithValidInputs_ReturnsExpectedCost()
        {
            var result = _costCalculationService.CalculateTotalPrice(1, false, 100, "", false, new DateTime(2024, 2, 1));
            Assert.Equal(21m, result);
        }

        [Fact]
        public void CalculateCost_OnWeekend_ApplyWeekendFee()
        {
            var result = _costCalculationService.CalculateTotalPrice(1, false, 100, "", false, new DateTime(2024, 2, 3));
            Assert.Equal(26.25m, result);
        }

        [Fact]
        public void CalculateCost_OnHoliday_ApplyHolidayFee()
        {
            var result = _costCalculationService.CalculateTotalPrice(1, false, 100, "", false, new DateTime(2024, 1, 1));
            Assert.Equal(31.50m, result);
        }

        [Fact]
        public void CalculateCost_OnHolidayWeekend_ApplyFees()
        {
            var result = _costCalculationService.CalculateTotalPrice(1, false, 100, "", false, new DateTime(2022, 12, 25));
            Assert.Equal(36.75m, result);
        }

        [Fact]
        public void CalculateCost_IsDangerous_ApplyFees()
        {
            var result = _costCalculationService.CalculateTotalPrice(1, false, 100, "", true, new DateTime(2024, 2, 1));
            Assert.Equal(36m, result);
        }

        [Fact]
        public void CalculateCost_ExpressDelivery_ApplyFees()
        {
            var result = _costCalculationService.CalculateTotalPrice(1, true, 100, "", false, new DateTime(2024, 2, 1));
            Assert.Equal(26m, result);
        }
    }
}

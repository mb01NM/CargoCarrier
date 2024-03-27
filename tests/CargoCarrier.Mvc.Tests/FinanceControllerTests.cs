using System.Text.Json;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using CargoCarrier.Mvc.Controllers;

using Moq;

namespace CargoCarrier.Mvc.Tests
{
    public class FinanceControllerTests
    {
        private readonly FinanceController _controller;

        public FinanceControllerTests()
        {
            var logger = new Mock<ILogger<FinanceController>>();
            _controller = new FinanceController(logger.Object);
        }

        [Fact]
        public void CalculateCost_WithNegativeDistance_ReturnsBadRequest()
        {
            var result = _controller.CalculateCost(1, false, -1, "", false, DateTime.Now);
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void CalculateCost_WithValidInputs_ReturnsExpectedCost()
        {
            string expected = "{\"result\":21.0}";
            var result = _controller.CalculateCost(1, false, 100, "", false, new DateTime(2024, 2, 1)) as JsonResult;
            var resultJson = JsonSerializer.Serialize(result?.Value);
            Assert.Equal(expected, resultJson);
        }

        [Fact]
        public void CalculateCost_OnWeekend_ApplyWeekendFee()
        {
            string expected = "{\"result\":26.25}";
            var result = _controller.CalculateCost(1, false, 100, "", false, new DateTime(2024, 2, 3)) as JsonResult;
            var resultJson = JsonSerializer.Serialize(result?.Value);
            Assert.Equal(expected, resultJson);
        }

        [Fact]
        public void CalculateCost_OnHoliday_ApplyHolidayFee()
        {
            string expected = "{\"result\":31.50}";
            var result = _controller.CalculateCost(1, false, 100, "", false, new DateTime(2024, 1, 1)) as JsonResult;
            var resultJson = JsonSerializer.Serialize(result?.Value);
            Assert.Equal(expected, resultJson);
        }

        [Fact]
        public void CalculateCost_OnHolidayWeekend_ApplyFees()
        {
            string expected = "{\"result\":36.75}";
            var result = _controller.CalculateCost(1, false, 100, "", false, new DateTime(2022, 12, 25)) as JsonResult;
            var resultJson = JsonSerializer.Serialize(result?.Value);
            Assert.Equal(expected, resultJson);
        }

        [Fact]
        public void CalculateCost_IsDangerous_ApplyFees()
        {
            string expected = "{\"result\":36.0}";
            var result = _controller.CalculateCost(1, false, 100, "", true, new DateTime(2024, 2, 1)) as JsonResult;
            var resultJson = JsonSerializer.Serialize(result?.Value);
            Assert.Equal(expected, resultJson);
        }

        [Fact]
        public void CalculateCost_ExpressDelivery_ApplyFees()
        {
            string expected = "{\"result\":26.0}";
            var result = _controller.CalculateCost(1, true, 100, "", false, new DateTime(2024, 2, 1)) as JsonResult;
            var resultJson = JsonSerializer.Serialize(result?.Value);
            Assert.Equal(expected, resultJson);
        }
    }
}

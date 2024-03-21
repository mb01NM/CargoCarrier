using Xunit;
using CargoCarrier.Mvc.Services;

namespace CargoCarrier.Mvc.Tests.Services
{
    public class TruckLoadCalculatorTests
    {
        private readonly TruckLoadCalculator _calculator;

        public TruckLoadCalculatorTests()
        {
            _calculator = new TruckLoadCalculator();
        }

        [Fact]
        public void CalculateTruckLoads_AllSmallParcels()
        {
            var result = _calculator.CalculateTruckLoads(12, 0, 0);
            Assert.Equal(0, result.smallTrucks);
            Assert.Equal(1, result.largeTrucks);
        }

        [Fact]
        public void CalculateTruckLoads_AllMediumParcels()
        {
            var result = _calculator.CalculateTruckLoads(0, 6, 0);
            Assert.Equal(0, result.smallTrucks);
            Assert.Equal(1, result.largeTrucks);
        }

        [Fact]
        public void CalculateTruckLoads_AllLargeParcels()
        {
            var result = _calculator.CalculateTruckLoads(0, 0, 3);
            Assert.Equal(0, result.smallTrucks);
            Assert.Equal(1, result.largeTrucks);
        }

        [Fact]
        public void CalculateTruckLoads_MixedParcels()
        {
            var result = _calculator.CalculateTruckLoads(2, 2, 1);
            // Incorrectly generated as 0
            Assert.Equal(3, result.smallTrucks);
            // Incorrectly generated as 1
            Assert.Equal(0, result.largeTrucks);
        }

        [Fact]
        public void CalculateTruckLoads_RequiresSmallTruck()
        {
            var result = _calculator.CalculateTruckLoads(1, 0, 0);
            Assert.Equal(1, result.smallTrucks);
            Assert.Equal(0, result.largeTrucks);
        }

        [Fact]
        public void CalculateTruckLoads_RequiresMultipleTrucks()
        {
            var result = _calculator.CalculateTruckLoads(20, 0, 0);
            // Incorrectly generated 1
            Assert.Equal(2, result.smallTrucks);
            Assert.Equal(1, result.largeTrucks);
        }
    }
}
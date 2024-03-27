using System;

namespace CargoCarrier.Mvc.Services
{
    public static class CostCalculationConstants
    {
        public const decimal BasePricePerParcel = 10;
        public const decimal ExpressFee = 5;
        public const decimal DistanceFeePerKm = 0.1m;
        public const decimal VoucherDiscountAmount = 10;
        public const decimal DangerousFee = 15;
        public const decimal WeekendFeeRate = 0.25m;
        public const decimal HolidayFeeRate = 0.50m;
        public const string DiscountVoucherCode = "DISCOUNT10";
    }

    public class CostCalculationService
    {
        public decimal CalculateTotalPrice(decimal parcelAmount, bool isExpress, decimal distance, string voucherCode, bool isDangerous, DateTime startDate)
        {
            decimal basePrice = CalculateBasePrice(parcelAmount);
            decimal expressFee = CalculateExpressFee(isExpress);
            decimal distanceFee = CalculateDistanceFee(distance);
            decimal voucherDiscount = CalculateVoucherDiscount(voucherCode);
            decimal dangerousFee = CalculateDangerousFee(isDangerous);
            decimal weekendFee = CalculateWeekendFee(basePrice, expressFee, distanceFee, dangerousFee, startDate);
            decimal holidayFee = CalculateHolidayFee(basePrice, expressFee, distanceFee, dangerousFee, startDate);

            return SumPrices(basePrice, expressFee, distanceFee, voucherDiscount, dangerousFee, weekendFee, holidayFee);
        }

        private bool CheckIfHoliday(DateTime startDate)
        {
            if (startDate.Month == 12 && startDate.Day == 25)
                return true;
            if (startDate.Month == 1 && startDate.Day == 1)
                return true;
            if (startDate.Month == 4 && startDate.Day == 1)
                return true;
            return false;
        }

        public decimal CalculateBasePrice(decimal parcelAmount)
        {
            return CostCalculationConstants.BasePricePerParcel + parcelAmount;
        }

        public decimal CalculateExpressFee(bool isExpress)
        {
            return isExpress ? CostCalculationConstants.ExpressFee : 0;
        }

        public decimal CalculateDistanceFee(decimal distance)
        {
            return distance * CostCalculationConstants.DistanceFeePerKm;
        }

        public decimal CalculateVoucherDiscount(string voucherCode)
        {
            return voucherCode == CostCalculationConstants.DiscountVoucherCode ? CostCalculationConstants.VoucherDiscountAmount : 0;
        }

        public decimal CalculateDangerousFee(bool isDangerous)
        {
            return isDangerous ? CostCalculationConstants.DangerousFee : 0;
        }

        public decimal CalculateWeekendFee(decimal basePrice, decimal expressFee, decimal distanceFee, decimal dangerousFee, DateTime startDate)
        {
            bool isWeekend = startDate.DayOfWeek == DayOfWeek.Saturday || startDate.DayOfWeek == DayOfWeek.Sunday;
            return isWeekend ? (basePrice + expressFee + distanceFee + dangerousFee) * CostCalculationConstants.WeekendFeeRate : 0;
        }

        public decimal CalculateHolidayFee(decimal basePrice, decimal expressFee, decimal distanceFee, decimal dangerousFee, DateTime startDate)
        {
            bool isHoliday = CheckIfHoliday(startDate);
            return isHoliday ? (basePrice + expressFee + distanceFee + dangerousFee) * CostCalculationConstants.HolidayFeeRate : 0;
        }

        public decimal SumPrices(decimal basePrice, decimal expressFee, decimal distanceFee, decimal voucherDiscount, decimal dangerousFee, decimal weekendFee, decimal holidayFee)
        {
            return basePrice + expressFee + distanceFee - voucherDiscount + dangerousFee + weekendFee + holidayFee;
        }
    }
}

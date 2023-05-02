using System;
using System.Globalization;

namespace FDO.Model
{
    public class YearMonth : IComparable<YearMonth>
    {
        const int PrimeValue = 30467;
        public int Month;
        public int Year;

        public string Name
        {
            get
            {
                return $"{Year}-{CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Month)}";
            }
        }

        public YearMonth(DateTime date)
        {
            Month = date.Month;
            Year = date.Year;
        }

        public override int GetHashCode()
        {
            return (Year * PrimeValue) + Month;
        }

        public int CompareTo(YearMonth other)
        {
            return this.GetHashCode() - other.GetHashCode();
        }
    }
}

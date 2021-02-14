using System;
using System.Collections.Generic;
using System.Linq;

namespace DesignCrowd.DateLibrary.Models
{
    public class HolidayOnSpecificDay : IPublicHoliday
    {
        //Default days that holidays must occur on are weekdays which are defined as Monday to Friday.
        private IEnumerable<DayOfWeek> MustOccurOn { get; } =  new List<DayOfWeek>() { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday };
        private int HolidayMonth { get; }
        private int HolidayDay { get; }
        private Displace Displace { get; }

        public HolidayOnSpecificDay(int holidayDay, int holidayMonth, Displace displace = Displace.Forward)
        {
            if (holidayDay is < 1 or > 31)
            {
                throw new ArgumentOutOfRangeException(nameof(holidayDay), " must be between 1 and 31.");
            }
            if (holidayMonth is < 1 or > 12)
            {
                throw new ArgumentOutOfRangeException(nameof(holidayMonth), " must be between 1 and 12.");
            }
            HolidayDay = holidayDay;
            HolidayMonth = holidayMonth;
            Displace = displace;
        }
        public HolidayOnSpecificDay(int holidayDay, int holidayMonth, Displace displace, List<DayOfWeek> mustOccurOn)
            : this(holidayDay, holidayMonth, displace)
        {
            if (mustOccurOn == null || mustOccurOn.Distinct().Count() is < 1 or >= 7)
            {
                throw new ArgumentException("Parameter mustOccurOn can not be null and must contain between 1 and 6 days of the week.");
            }
        }

        public bool IsPublicHoliday(DateTime targetDate)
        {
            var adjustedHoliday = CalculateExactHolidayDate(targetDate);
            return adjustedHoliday == targetDate;
        }

        public DateTime CalculateExactHolidayDate(DateTime targetDate)
        {
            var holidayDate = new DateTime(targetDate.Year, HolidayMonth, HolidayDay);
            while (MustOccurOn.Any(d => d == holidayDate.DayOfWeek) == false)
            {
                holidayDate = Displace == Displace.Forward ? holidayDate.AddDays(1) : holidayDate.AddDays(-1);
            }
            return holidayDate;
        }
    }

    public enum Displace
    {
        NoDisplacement = 0,
        Forward = 1
    }
}

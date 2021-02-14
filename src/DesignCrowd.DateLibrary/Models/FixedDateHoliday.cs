using System;

namespace DesignCrowd.DateLibrary.Models
{
    public class FixedDateHoliday : IPublicHoliday
    {
        private int HolidayMonth { get; }
        private int HolidayDay { get; }

        public FixedDateHoliday(DateTime holidayDate)
        {
            HolidayMonth = holidayDate.Month;
            HolidayDay = holidayDate.Day;
        }

        public bool IsPublicHoliday(DateTime targetDate)
        {
            var holidayDate = CalculateExactHolidayDate(targetDate);
            return holidayDate == targetDate;
        }

        public DateTime CalculateExactHolidayDate(DateTime targetDate)
        {
            return new DateTime(targetDate.Year, HolidayMonth, HolidayDay);
        }
    }
}

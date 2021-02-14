using System;

namespace DesignCrowd.DateLibrary.Models
{
    public class HolidayOnSpecificOccurrenceOfDay : IPublicHoliday
    {
        private DayOfWeek HolidayDayOfWeek { get; }
        private int HolidayMonth { get; }
        private int OccurrenceNumber { get; }

        public HolidayOnSpecificOccurrenceOfDay(DayOfWeek holidayDayOfWeek, int occurrenceNumber, int holidayMonth)
        {
            if (occurrenceNumber < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(occurrenceNumber), "must be at least 1.");
            }
            if (holidayMonth is < 1 or > 12)
            {
                throw new ArgumentOutOfRangeException(nameof(holidayMonth), "must be between 1 and 12 inclusively.");
            }
            HolidayDayOfWeek = holidayDayOfWeek;
            HolidayMonth = holidayMonth;
            OccurrenceNumber = occurrenceNumber;
        }

        public bool IsPublicHoliday(DateTime targetDate)
        {
            var adjustedHoliday = CalculateExactHolidayDate(targetDate);
            return adjustedHoliday == targetDate;
        }

        public DateTime CalculateExactHolidayDate(DateTime targetDate)
        {
            var holiday = new DateTime(targetDate.Year, HolidayMonth, 1);
            var occurrences = 0;

            while (occurrences != OccurrenceNumber)
            {
                if (holiday.DayOfWeek == HolidayDayOfWeek)
                {
                    occurrences++;
                    if (occurrences == OccurrenceNumber)
                    {
                        break;
                    }
                }
                holiday = holiday.AddDays(1);
            }
            return holiday;
        }
    }
}

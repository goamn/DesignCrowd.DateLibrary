using DesignCrowd.DateLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DesignCrowd.DateLibrary
{
    public class BusinessDayCounter
    {
        /// <summary>
        /// Counts the weekdays between two dates exclusively. Assumes weekends are Saturday and Sunday.
        /// </summary>
        public int WeekdaysBetweenTwoDates(DateTime firstDate, DateTime secondDate)
        {
            return BusinessDaysBetweenTwoDates(firstDate, secondDate);
        }

        /// <summary>
        /// Counts the business days between two dates exclusively. Assumes weekends are Saturday and Sunday.
        /// </summary>
        public int BusinessDaysBetweenTwoDates(DateTime firstDate, DateTime secondDate, IList<DateTime> publicHolidays)
        {
            var holidays = new List<IPublicHoliday>();
            foreach (var dateTime in publicHolidays)
            {
                holidays.Add(new FixedDateHoliday(dateTime));
            }
            return BusinessDaysBetweenTwoDates(firstDate, secondDate, holidays);
        }


        public int BusinessDaysBetweenTwoDates(DateTime firstDate, DateTime secondDate, IList<IPublicHoliday> publicHolidays = null)
        {
            var dateToCheck = firstDate.Date.AddDays(1);
            secondDate = secondDate.Date;

            var businessDays = 0;
            var skipHolidaysCheck = publicHolidays == null;

            if (dateToCheck != secondDate && secondDate > dateToCheck)
            {
                while (dateToCheck < secondDate)
                {
                    if (dateToCheck.DayOfWeek != DayOfWeek.Saturday && dateToCheck.DayOfWeek != DayOfWeek.Sunday)
                    {
                        var notPublicHoliday = skipHolidaysCheck || publicHolidays.Any(x => x.IsPublicHoliday(dateToCheck)) == false;
                        if (notPublicHoliday)
                        {
                            businessDays++;
                        }
                    }
                    dateToCheck = dateToCheck.AddDays(1);
                }
            }
            return businessDays;
        }
    }
}
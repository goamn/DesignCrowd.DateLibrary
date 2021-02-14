using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace DesignCrowd.DateLibrary.Tests.BusinessDayCounter
{
    public class WeekdaysAndBusinessDayTests
    {
        private readonly DateLibrary.BusinessDayCounter _businessDayCounter;

        public WeekdaysAndBusinessDayTests()
        {
            _businessDayCounter = new DateLibrary.BusinessDayCounter();
        }

        [Theory]
        [InlineData(1, "7-oct-2013", "9-oct-2013")]
        [InlineData(5, "5-oct-2013", "14-oct-2013")]
        [InlineData(61, "7-oct-2013", "1-jan-2014")]
        [InlineData(0, "12-feb-2021", "15-feb-2021")]
        [InlineData(127052, "31-dec-2013", "31-dec-2500")]
        public void GivenValidParameters_WeekdayCalculation_ShouldBeCorrect(int expectedWeekdayResult, string fromDate, string toDate)
        {
            var weekdays = _businessDayCounter.WeekdaysBetweenTwoDates(DateTime.Parse(fromDate), DateTime.Parse(toDate));
            Assert.Equal(expectedWeekdayResult, weekdays);
        }

        [Theory]
        [InlineData(0, "7-oct-2013", "5-oct-2013")]
        [InlineData(0, "15-feb-2021", "15-feb-2021")]
        [InlineData(0, "1-feb-2021", "20-feb-2001")]
        public void GivenInvalidToDate_WeekdayCalculation_ShouldBeZero(int expectedWeekdayResult, string fromDate, string toDate)
        {
            var weekdays = _businessDayCounter.WeekdaysBetweenTwoDates(DateTime.Parse(fromDate), DateTime.Parse(toDate));
            Assert.Equal(expectedWeekdayResult, weekdays);
        }

        [Theory]
        [InlineData(1, "7-oct-2013", "9-oct-2013", new[] { "25-dec-2013", "26-dec-2013", "1-jan-2014" })]
        [InlineData(0, "24-12-2013", "27-dec-2013", new[] { "25-dec-2013", "26-dec-2013", "1-jan-2014" })]
        [InlineData(59, "7-oct-2013", "1-jan-2014", new[] { "25-dec-2013", "26-dec-2013", "1-jan-2014" })]
        [InlineData(59, "7-oct-2014", "1-jan-2015", new[] { "25-dec-2013", "26-dec-2013", "1-jan-2014" })]
        public void GivenHolidayList_BusinessDayCalculation_ShouldNotIncludeHolidays(int expectedBusinessDayResult, string fromDate, string toDate, IList<string> holidays)
        {
            var holidayDates = holidays.Select(DateTime.Parse).ToList();
            var businessDays = _businessDayCounter.BusinessDaysBetweenTwoDates(DateTime.Parse(fromDate), DateTime.Parse(toDate), holidayDates);
            Assert.Equal(expectedBusinessDayResult, businessDays);
        }

        [Theory]
        [InlineData(0, "15-feb-2030", "15-feb-2030", new[] { "1-feb-2000", "1-feb-2010", "1-feb-2030" })]
        [InlineData(0, "15-feb-2030", "10-feb-2030", new[] { "1-feb-2000", "1-feb-2010", "1-feb-2030" })]
        [InlineData(0, "1-feb-2021", "20-feb-2001", new[] { "1-feb-2000", "1-feb-2010", "1-feb-2030" })]
        public void GivenInvalidToDate_BusinessDayCalculation_ShouldBeZero(int expectedBusinessDayResult, string fromDate, string toDate, IList<string> holidays)
        {
            var holidayDates = holidays.Select(DateTime.Parse).ToList();
            var businessDays = _businessDayCounter.BusinessDaysBetweenTwoDates(DateTime.Parse(fromDate), DateTime.Parse(toDate), holidayDates);
            Assert.Equal(expectedBusinessDayResult, businessDays);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using DesignCrowd.DateLibrary.Models;
using Xunit;

namespace DesignCrowd.DateLibrary.Tests.BusinessDayCounter
{
    public class ComplexHolidayTests
    {
        private readonly DateLibrary.BusinessDayCounter _businessDayCounter;

        public ComplexHolidayTests()
        {
            _businessDayCounter = new DateLibrary.BusinessDayCounter();
        }

        public static readonly object[][] TestHolidayOnSpecificDay =
        {
            new object[] { 0, "25-jan-2021", "27-jan-2021", new[] { new HolidayOnSpecificDay(26, 1) } },
            new object[] { 0, "31-dec-2021", "4-jan-2022", new[] { new HolidayOnSpecificDay(1, 1) } },
            new object[] { 1, "23-apr-2021", "27-apr-2021", new[] { new HolidayOnSpecificDay(25, 4, Displace.NoDisplacement) } },
            new object[] { 0, "23-apr-2021", "27-apr-2021", new[] { new HolidayOnSpecificDay(25, 4, Displace.Forwards, new List<DayOfWeek>() { DayOfWeek.Sunday, DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday }) } },
        };
        [Theory, MemberData(nameof(TestHolidayOnSpecificDay))]
        public void GivenHolidayOnSpecificDay_BusinessDayCalculation_ShouldBeCorrect(int expectedBusinessDayResult, string fromDate, string toDate, IPublicHoliday[] publicHolidays)
        {
            var businessDays = _businessDayCounter.BusinessDaysBetweenTwoDates(DateTime.Parse(fromDate), DateTime.Parse(toDate), publicHolidays.ToList());
            Assert.Equal(expectedBusinessDayResult, businessDays);
        }

        public static readonly object[][] TestInvalidHolidayOnSpecificDay =
        {
            new object[] { typeof(ArgumentOutOfRangeException), 1, 13, new List<DayOfWeek> { DayOfWeek.Monday } },
            new object[] { typeof(ArgumentOutOfRangeException), 0, 1, new List<DayOfWeek> { DayOfWeek.Monday } },
            new object[] { typeof(ArgumentException), 1, 12, new List<DayOfWeek>()},
        };
        [Theory, MemberData(nameof(TestInvalidHolidayOnSpecificDay))]
        public void GivenInvalidParameters_HolidayOnSpecificDayModel_ShouldThrow(Type expectedExceptionType, int holidayDay, int holidayMonth, List<DayOfWeek> mustOccurOn)
        {
            var exceptionThrown = Assert.ThrowsAny<Exception>(() => new HolidayOnSpecificDay(holidayDay, holidayMonth, Displace.Forwards, mustOccurOn));
            Assert.Equal(expectedExceptionType, exceptionThrown.GetType());
        }

        public static readonly object[][] TestHolidayOnSpecificOccurrenceOfDay =
        {
            new object[] { 0, "13-jun-2021", "15-jun-2021", new[] { new HolidayOnSpecificOccurrenceOfDay(DayOfWeek.Monday, 2, 6) } },
            new object[] { 0, "12-jun-2022", "14-jun-2022", new[] { new HolidayOnSpecificOccurrenceOfDay(DayOfWeek.Monday, 2, 6) } },
            new object[] { 4, "3-oct-2021", "9-oct-2021", new[] { new HolidayOnSpecificOccurrenceOfDay(DayOfWeek.Monday, 1, 10) } },
        };
        [Theory, MemberData(nameof(TestHolidayOnSpecificOccurrenceOfDay))]
        public void GivenHolidayOnSpecificOccurrenceOfDay_BusinessDayCalculation_ShouldBeCorrect(int expectedBusinessDayResult, string fromDate, string toDate, IPublicHoliday[] publicHolidays)
        {
            var businessDays = _businessDayCounter.BusinessDaysBetweenTwoDates(DateTime.Parse(fromDate), DateTime.Parse(toDate), publicHolidays.ToList());
            Assert.Equal(expectedBusinessDayResult, businessDays);
        }

        public static readonly object[][] TestInvalidHolidayOnSpecificOccurrenceOfDay =
        {
            new object[] { 0, 6 },
            new object[] { 2, 0 },
        };
        [Theory, MemberData(nameof(TestInvalidHolidayOnSpecificOccurrenceOfDay))]
        public void GivenInvalidParameters_HolidayOnSpecificOccurrenceOfDayModel_ShouldThrow(int occurrenceNumber, int holidayMonth)
        {
            Assert.ThrowsAny<ArgumentOutOfRangeException>(() => new HolidayOnSpecificOccurrenceOfDay(DayOfWeek.Monday, occurrenceNumber, holidayMonth));
        }
    }
}
# DesignCrowd.DateLibrary

Requires .Net 5.

## Tests

All tests are unit tests. You can run the tests with your preferred test runner, or using the standard command line:

```
dotnet test
```

## Remarks

Regarding the test "WeekdaysAndBusinessDayTests.GivenHolidayList_BusinessDayCalculation_ShouldNotIncludeHolidays()". InlineData was preferred over MemberData here as the dates and holiday list are more readable.

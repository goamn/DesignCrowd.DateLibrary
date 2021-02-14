using System;

namespace DesignCrowd.DateLibrary.Models
{
	public interface IPublicHoliday
	{
		bool IsPublicHoliday(DateTime date);
        DateTime CalculateExactHolidayDate(DateTime date);
	}
}

using BIP.InternalCRM.Domain.Enums;
using System.Globalization;

namespace BIP.InternalCRM.Application.Extensions;

public static class DateTimeExtension
{
    public static FinancialQuarter GetFinancialQuarter(this DateTime date)
        => (FinancialQuarter)((date.AddMonths(-3).Month + 2) / 3);

    public static int GetIso8601WeekOfYear(this DateTime time)
    {
        var day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
        if (day is >= DayOfWeek.Monday and <= DayOfWeek.Wednesday)
        {
            time = time.AddDays(3);
        }

        return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
    }
}
using PublicHoliday;
using WalleyCodeTest.Vehicles;

namespace WalleyCodeTest.Pricing;

public class TollFeeProvider : ITollFeeProvider
{
    private readonly IPublicHolidays _holidays;

    private readonly IEnumerable<TimeRange> _prices = new List<TimeRange>
    {
        new TimeRange(new TimeSpan(6, 0, 0), new TimeSpan(6, 29, 0), 8),
        new TimeRange(new TimeSpan(6, 30, 0), new TimeSpan(6, 59, 0), 13),
        new TimeRange(new TimeSpan(7, 0, 0), new TimeSpan(7, 59, 0), 18),
        new TimeRange(new TimeSpan(8, 0, 0), new TimeSpan(8, 29, 0), 13),
        new TimeRange(new TimeSpan(8, 30, 0), new TimeSpan(14, 59, 0), 8),
        new TimeRange(new TimeSpan(15, 0, 0), new TimeSpan(15, 29, 0), 13),
        new TimeRange(new TimeSpan(15, 30, 0), new TimeSpan(16, 59, 0), 18),
        new TimeRange(new TimeSpan(17, 0, 0), new TimeSpan(17, 59, 0), 13),
        new TimeRange(new TimeSpan(18, 0, 0), new TimeSpan(18, 29, 0), 8),
        new TimeRange(new TimeSpan(18, 30, 0), new TimeSpan(5, 59, 0), 0)
    };

    public TollFeeProvider(IPublicHolidays holidays)
    {
        _holidays = holidays;
    }
    
    //Comment: Här funderade jag på om man bör returnera 0 eller throwa exception(alternativt returnera Result.Fail())
    // om pris saknas för intervallet, däremot tänker jag att man specifik specifierar när och hur mycket man ska betala men utanför det
    // är det gratis.
    public int GetTollFee(IVehicle vehicle, DateTime date)
    {
        if (IsTollFreeDate(date) || vehicle.IsTollFree()) 
            return 0;
        
        foreach (var price in _prices)
        {
            if (date.TimeOfDay >= price.StartTime && date.TimeOfDay <= price.EndTime)
            {
                return price.Price;
            }
        }
        
        return 0;
    }
 
    private bool IsTollFreeDate(DateTime date)
    {
        if (IsWeekend(date)) 
            return true;

        if (IsInJuly(date))
            return true;

        return IsHolidayOrDayBeforeHoliday(date);
    }
    
    // Comment: Valde att köra på ett library hoppas det är okej, det känns dumt om man
    // varje år måste specifiera vilka högtider som finns.
    private bool IsHolidayOrDayBeforeHoliday(DateTime date)
    {
        var isPublicHoliday = _holidays.IsPublicHoliday(date);
        var isDayBeforePublicHoliday = _holidays.IsPublicHoliday(date.AddDays(1));
        
        return isPublicHoliday || isDayBeforePublicHoliday;
    }

    //Comment: kanske är snyggare att skapa en extension metod om det används på flera ställen
    private static bool IsWeekend(DateTime date) => date.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday;

    private static bool IsInJuly(DateTime date) => date.Month == 7;

    private readonly struct TimeRange(TimeSpan startTime, TimeSpan endTime, int price)
    {
        public TimeSpan StartTime { get; } = startTime;
        public TimeSpan EndTime { get; } = endTime;
        public int Price { get; } = price;
    }
}

public interface ITollFeeProvider
{
    int GetTollFee(IVehicle vehicle, DateTime date);
}
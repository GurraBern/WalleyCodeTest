using WalleyCodeTest.Vehicles;

namespace WalleyCodeTest.Pricing;

public class TollFeeProvider : ITollFeeProvider
{
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
    
    //TODO beroendes på hur logiken ändras kanske det är bättre att fordon själv har koll på om den ska betala avgift eller ej
    //Just nu kanske det är overkill? Åandra måste man in i den här klassen när man lägger ett nytt fordon vilket bryter mot OCP
    private static IEnumerable<VehicleType> TollFreeVehicles =>
    [
        VehicleType.Motorbike,
        VehicleType.Tractor,
        VehicleType.Emergency,
        VehicleType.Diplomat,
        VehicleType.Foreign,
        VehicleType.Military
    ];
    
    //Comment: Här funderade jag på om man bör returnera 0 eller throwa exception(alternativt returnera Result.Fail())
    // om pris saknas för intervallet, däremot tänker jag att man specifik specifierar när och hur mycket man ska betala men utanför det
    // är det gratis.
    public int GetTollFee(DateTime date, IVehicle vehicle)
    {
        if (IsTollFreeDate(date) || IsTollFreeVehicle(vehicle)) 
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
 
    //TODO hitta ett bättre att få ut helgdagar? Libraries?
    private static bool IsTollFreeDate(DateTime date)
    {
        if (date.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday) 
            return true;

        if (date.Year == 2013)
        {
            return IsHolidayIn2013(date);
        }
        
        return false;
    }
    
    private static bool IsHolidayIn2013(DateTime date)
    {
        var month = date.Month;
        var day = date.Day;
        
        return month == 1 && day == 1 ||
               month == 3 && (day == 28 || day == 29) ||
               month == 4 && (day == 1 || day == 30) ||
               month == 5 && (day == 1 || day == 8 || day == 9) ||
               month == 6 && (day == 5 || day == 6 || day == 21) ||
               month == 7 ||
               month == 11 && day == 1 ||
               month == 12 && (day == 24 || day == 25 || day == 26 || day == 31);
    }
    
    private static bool IsTollFreeVehicle(IVehicle? vehicle)
    {
        if (vehicle == null) 
            return false;
        
        var isTollFree = TollFreeVehicles.Contains(vehicle.VehicleType);

        return isTollFree;
    }

    
    private readonly struct TimeRange(TimeSpan startTime, TimeSpan endTime, int price)
    {
        public TimeSpan StartTime { get; } = startTime;
        public TimeSpan EndTime { get; } = endTime;
        public int Price { get; } = price;
    }
}

public interface ITollFeeProvider
{
    int GetTollFee(DateTime date, IVehicle vehicle);
}
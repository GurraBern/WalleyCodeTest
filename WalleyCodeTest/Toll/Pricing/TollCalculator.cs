using WalleyCodeTest.Vehicles;

namespace WalleyCodeTest.Pricing;

public class TollCalculator
{

    /**
     * Calculate the total toll fee for one day
     *
     * @param vehicle - the vehicle
     * @param dates   - date and time of all passes on one day
     * @return - the total toll fee for that day
     */
    private const int MaxTotalFee = 60;
    
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

    // Comment: tidordning spelar roll och skulle kunna skapa buggar om listan kommer blandad, därför vill jag använda en IEnumerable så
    // att man kan slänga in olika typer "listor" till metoden, som sedan sorteras.
    public int GetTollFee(IVehicle vehicle, IEnumerable<DateTime> dates)
    {
        var timeStamps = dates
            .OrderBy(x => x.Date)
            .ToList();
        
        if (timeStamps.Count == 0)
            return 0;

        var intervalStart = timeStamps.First();

        var currentIntervalTimestamps = new List<DateTime>();
        
        //Jag vill att om det har passerat mer 60 minuter måste man betala nästa stop igen, men det är bug när man åker under timme 
        var totalFee = 0;
        foreach (var timestamp in timeStamps)
        {
            var minutesPassed = (timestamp - intervalStart).TotalMinutes;
            if (minutesPassed >= 60)
            {
                currentIntervalTimestamps.Add(timestamp);
                intervalStart = timestamp;
                
                totalFee += GetHighestTollFee(vehicle, currentIntervalTimestamps );
                
                currentIntervalTimestamps.Clear();
            }
            
            currentIntervalTimestamps.Add(timestamp);
        }
        
        if(currentIntervalTimestamps .Count > 0)
            totalFee += GetHighestTollFee(vehicle, currentIntervalTimestamps );

        return totalFee > MaxTotalFee
            ? MaxTotalFee
            : totalFee;
    }
    
    private int GetHighestTollFee(IVehicle vehicle, List<DateTime> timestamps)
    {
        var tollFees = timestamps
            .Select(date => GetTollFee(date, vehicle))
            .ToList();

        return tollFees.Max();
    }

    private static bool IsTollFreeVehicle(IVehicle? vehicle)
    {
        if (vehicle == null) 
            return false;
        
        var isTollFree = TollFreeVehicles.Contains(vehicle.VehicleType);

        return isTollFree;
    }

    //Comment: Här funderade jag på om man bör returnera 0 eller throwa exception(alternativt returnera Result.Fail())
    // om pris saknas för intervallet, däremot tänker jag att man specifik specifierar när och hur mycket man ska betala men utanför det
    // är det gratis.
    private int GetTollFee(DateTime date, IVehicle vehicle)
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
    
    private readonly struct TimeRange(TimeSpan startTime, TimeSpan endTime, int price)
    {
        public TimeSpan StartTime { get; } = startTime;
        public TimeSpan EndTime { get; } = endTime;
        public int Price { get; } = price;
    }
}
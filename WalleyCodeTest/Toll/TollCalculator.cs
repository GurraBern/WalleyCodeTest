using WalleyCodeTest.Vehicles;

namespace WalleyCodeTest;

public class TollCalculator
{

    /**
     * Calculate the total toll fee for one day
     *
     * @param vehicle - the vehicle
     * @param dates   - date and time of all passes on one day
     * @return - the total toll fee for that day
     */

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

    private const int MaxTotalFee = 60;

    //Get rid of multi enumeration
    public int GetTollFee(IVehicle vehicle, IEnumerable<DateTime> dates)
    {
        var dateTimes = dates.ToList();
        
        if (dateTimes.Count == 0)
            return 0;
        
        var totalFee = 0;
        foreach (var date in dateTimes)
        {
            totalFee += GetTollFee(date, vehicle);
        }


        //     var nextFee = GetTollFee(date, vehicle);
        //     var tempFee = GetTollFee(intervalStart, vehicle);
        //
        //     long diffInMillies = date.Millisecond - intervalStart.Millisecond;
        //     var minutes = diffInMillies/1000/60;
        //
        //     if (minutes <= 60)
        //     {
        //         if (totalFee > 0) 
        //             totalFee -= tempFee;
        //         
        //         if (nextFee >= tempFee) 
        //             tempFee = nextFee;
        //         
        //         totalFee += tempFee;
        //     }
        //     else
        //     {
        //         totalFee += nextFee;
        //     }
        // }
        return totalFee > MaxTotalFee
            ? MaxTotalFee
            : totalFee;
    }

    private static bool IsTollFreeVehicle(IVehicle? vehicle)
    {
        if (vehicle == null) 
            return false;
        
        var isTollFree = TollFreeVehicles.Contains(vehicle.VehicleType);

        return isTollFree;
    }

    private static int GetTollFee(DateTime date, IVehicle vehicle)
    {
        if (IsTollFreeDate(date) || IsTollFreeVehicle(vehicle)) 
            return 0;

        var hour = date.Hour;
        var minute = date.Minute;
        
        if (hour == 6 && minute is >= 0 and <= 29) return 8;
        if (hour == 6 && minute >= 30 && minute <= 59) return 13;
        if (hour == 7 && minute >= 0 && minute <= 59) return 18;
        if (hour == 8 && minute >= 0 && minute <= 29) return 13;
        if (hour >= 8 && hour <= 14 && minute >= 30 && minute <= 59) return 8;
        if (hour == 15 && minute >= 0 && minute <= 29) return 13;
        if (hour == 15 && minute >= 0 || hour == 16 && minute <= 59) return 18;
        if (hour == 17 && minute >= 0 && minute <= 59) return 13;
        if (hour == 18 && minute >= 0 && minute <= 29) return 8;
        
        return 0;
    }

    //TODO hitta ett bättre att få ut helgdagar? Libraries?
    private static bool IsTollFreeDate(DateTime date)
    {
        //TODO ska det skattas på söndag?
        if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday) 
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
}
using WalleyCodeTest.Vehicles;

namespace WalleyCodeTest.Pricing;

public class TollCalculator
{
    private readonly ITollFeeProvider _tollFeeProvider;

    /**
     * Calculate the total toll fee for one day
     *
     * @param vehicle - the vehicle
     * @param dates   - date and time of all passes on one day
     * @return - the total toll fee for that day
     */

    // Comment: Jag tänker att denna klassens uppgift är att räkna ut trängselskatten, därför tycker jag att den ansvarar för max total fee och inte TollFeeProvider
    private const int MaxTotalFee = 60;

    public TollCalculator(ITollFeeProvider tollFeeProvider)
    {
        _tollFeeProvider = tollFeeProvider;
    }
    
    // Comment: tidordning spelar roll och skulle kunna skapa buggar om listan kommer blandad, därför vill jag använda en IEnumerable så
    // att man kan slänga in olika typer "listor" till metoden, som sedan sorteras.
    public int GetTotalTollFee(IVehicle vehicle, IEnumerable<DateTime> dates)
    {
        var timeStamps = dates
            .OrderBy(x => x.Date)
            .ToList();
        
        if (timeStamps.Count == 0)
            return 0;

        var totalFee = 0;
        var currentIntervalTimestamps = new List<DateTime>();
        var intervalStart = timeStamps.First();
        
        foreach (var timestamp in timeStamps)
        {
            currentIntervalTimestamps.Add(timestamp);
            
            var minutesPassed = (timestamp - intervalStart).TotalMinutes;
            if ((minutesPassed >= 60) is false)
                continue;

            totalFee += GetHighestTollFee(vehicle, currentIntervalTimestamps);
                
            currentIntervalTimestamps.Clear();
            intervalStart = timestamp;
        }
        
        if(currentIntervalTimestamps.Count > 0)
            totalFee += GetHighestTollFee(vehicle, currentIntervalTimestamps);
        
        return Math.Min(totalFee, MaxTotalFee);
    }
    
    private int GetHighestTollFee(IVehicle vehicle, List<DateTime> timestamps)
    {
        var tollFees = timestamps
            .Select(date => _tollFeeProvider.GetTollFee(vehicle, date))
            .ToList();

        return tollFees.Max();
    }
}
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
            .Select(date => _tollFeeProvider.GetTollFee(date, vehicle))
            .ToList();

        return tollFees.Max();
    }
}
using WalleyCodeTest;
using WalleyCodeTest.Pricing;
using WalleyCodeTest.Vehicles;

namespace TollTests;

public class TollCalculatorTests
{
    //TODO use Datetime instead of string
    [Theory]
    [InlineData("2013-02-01 06:00", 8)]
    [InlineData("2013-02-01 06:29", 8)]
    [InlineData("2013-02-01 06:30", 13)]
    [InlineData("2013-02-01 06:59", 13)]
    [InlineData("2013-02-01 07:00", 18)]
    [InlineData("2013-02-01 07:59", 18)]
    [InlineData("2013-02-01 08:00", 13)]
    [InlineData("2013-02-01 08:29", 13)]
    [InlineData("2013-02-01 08:30", 8)]
    [InlineData("2013-02-01 14:59", 8)]
    [InlineData("2013-02-01 15:00", 13)]
    [InlineData("2013-02-01 15:29", 13)]
    [InlineData("2013-02-01 15:30", 18)]
    [InlineData("2013-02-01 16:59", 18)]
    [InlineData("2013-02-01 17:00", 13)]
    [InlineData("2013-02-01 17:59", 13)]
    [InlineData("2013-02-01 18:00", 8)]
    [InlineData("2013-02-01 18:29", 8)]
    [InlineData("2013-02-01 18:30", 0)]
    [InlineData("2013-02-01 05:59", 0)]
    [InlineData("2013-02-01 09:00", 8)]
    [InlineData("2013-02-01 10:00", 8)]
    [InlineData("2013-02-01 11:00", 8)]
    [InlineData("2013-02-01 12:00", 8)]
    [InlineData("2013-02-01 13:00", 8)]
    [InlineData("2013-02-01 14:00", 8)]
    [InlineData("2013-02-01 16:00", 18)]
    [InlineData("2013-02-01 19:00", 0)]
    public void GetTollFee_ShouldReturnCorrectAmount(string date, int expectedAmount)
    {
        // Arrange
        var currentTime = DateTime.Parse(date);

        var car = new Car();
        var tollFeeProvider = new TollFeeProvider();
        var sut = new TollCalculator(tollFeeProvider);
        
        // Act
        var result = sut.GetTotalTollFee(car, [currentTime]);

        //Assert
        Assert.Equal(expectedAmount, result);
    }
    
    [Fact]
    public void GetTollFee_Given_no_dates_Should_return_0_fee()
    {
        // Arrange
        var car = new Car();
        var tollFeeProvider = new TollFeeProvider();
        var sut = new TollCalculator(tollFeeProvider);
        
        // Act
        var result = sut.GetTotalTollFee(car, []);

        //Assert
        Assert.Equal(0, result);
    }
    
    [Fact]
    public void GetTollFee_max_price_for_date_Should_be_60()
    {
        // Arrange
        var car = new Car();
        var tollFeeProvider = new TollFeeProvider();
        var sut = new TollCalculator(tollFeeProvider);
        
        var dates = GetTollTimestamps();

        // Act
        var result = sut.GetTotalTollFee(car, dates);

        //Assert
        Assert.Equal(60, result);
    }
    
    //Baserat på uppgiften att det står "En bil passerar flera betalstationer" så gör jag antagandet 
    // att fordon paserar flera betalstationer(alltså att även motorcyckel t.ex följer samma logik)
    [Fact]
    public void GetTollFee_car_passing_multiple_toll_stations_Should_just_pay_once_per_60_minutes()
    {
        // Arrange
        var baseDate = DateTime.Parse("2013-02-01");
        const int highestTollFee = 21;
        
        var dates = new List<DateTime>
        {
            baseDate.AddHours(6).AddMinutes(0),
            baseDate.AddHours(6).AddMinutes(21),
            baseDate.AddHours(6).AddMinutes(30),
        };
        
        var car = new Car();
        var tollFeeProvider = new TollFeeProvider();
        var sut = new TollCalculator(tollFeeProvider);

        // Act
        var result = sut.GetTotalTollFee(car, dates);

        //Assert
        Assert.Equal(highestTollFee, result);
    }

    [Theory]
    [InlineData(VehicleType.Motorbike)]
    public void GetTollFee_Given_toll_free_vehicle_should_return_0_fee(VehicleType vehicleType)
    {
        // Arrange
        var timeStamps = GetTollTimestamps();
        var tollFeeProvider = new TollFeeProvider();
        var sut = new TollCalculator(tollFeeProvider);
        var vehicle = VehicleFactory.Create(vehicleType);

        // Act
        var actualFee = sut.GetTotalTollFee(vehicle, timeStamps);

        // Assert

        Assert.Equal(0, actualFee);
    }
    
    
    private static List<DateTime> GetTollTimestamps()
    {
        var baseDate = DateTime.Parse("2013-02-01");
        var dates = new List<DateTime>
        {
            baseDate.AddHours(6).AddMinutes(0),
            baseDate.AddHours(6).AddMinutes(30),
            baseDate.AddHours(7).AddMinutes(0),
            baseDate.AddHours(8).AddMinutes(0),
            baseDate.AddHours(9).AddMinutes(0),
            baseDate.AddHours(10).AddMinutes(0),
            baseDate.AddHours(11).AddMinutes(0),
            baseDate.AddHours(12).AddMinutes(0),
            baseDate.AddHours(13).AddMinutes(0),
            baseDate.AddHours(14).AddMinutes(0),
            baseDate.AddHours(15).AddMinutes(0)
        };

        return dates;
    }

    //TODO gör theory med alla enums?
    //TODO Skattas inte på lördag
    [Fact]
    public void GetTollFee_When_saturday_Should_be_zero()
    {
        // Arrange
        var saturday = new DateTime(2025, 2, 1);

        var car = new Car();
        var tollFeeProvider = new TollFeeProvider();
        var sut = new TollCalculator(tollFeeProvider);

        // Act
        var result = sut.GetTotalTollFee(car, [saturday]);

        //Assert
        Assert.Equal(0, result);
    }
    
    //TODO!!!!!
    // , helgdagar, dagar före helgdag, eller juli månad
}
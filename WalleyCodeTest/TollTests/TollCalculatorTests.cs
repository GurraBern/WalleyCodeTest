using WalleyCodeTest;
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
    public void GetTollFee_ShouldReturnCorrectAmount(string date, int expectedAmount)
    {
        // Arrange
        var currentTime = DateTime.Parse(date);

        var car = new Car();
        var sut = new TollCalculator();
        
        // Act
        var result = sut.GetTollFee(car, [currentTime]);

        //Assert
        Assert.Equal(expectedAmount, result);
    }
    
    [Fact]
    public void GetTollFee_Given_no_dates_Should_return_0_fee()
    {
        // Arrange
        var car = new Car();
        var sut = new TollCalculator();
        
        // Act
        var result = sut.GetTollFee(car, []);

        //Assert
        Assert.Equal(0, result);
    }
    
    [Fact]
    public void GetTollFee_max_price_for_date_Should_be_60()
    {
        // Arrange
        var car = new Car();
        var sut = new TollCalculator();
        
        var dates = GetTollDates();

        // Act
        var result = sut.GetTollFee(car, dates);

        //Assert
        Assert.Equal(60, result);
    }
    
    private static List<DateTime> GetTollDates()
    {
        var baseDate = DateTime.Parse("2013-02-01"); // Start with the base date
        var dates = new List<DateTime>
        {
            baseDate.AddHours(6).AddMinutes(0),  // 06:00
            baseDate.AddHours(6).AddMinutes(30), // 06:30
            baseDate.AddHours(7).AddMinutes(0),  // 07:00
            baseDate.AddHours(8).AddMinutes(0),  // 08:00
            baseDate.AddHours(8).AddMinutes(30), // 08:30
            baseDate.AddHours(15).AddMinutes(0), // 15:00
            baseDate.AddHours(15).AddMinutes(30),// 15:30
            baseDate.AddHours(17).AddMinutes(0), // 17:00
            baseDate.AddHours(18).AddMinutes(0), // 18:00
            baseDate.AddHours(18).AddMinutes(30) // 18:30
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
        var sut = new TollCalculator();

        // Act
        var result = sut.GetTollFee(car, [saturday]);

        //Assert
        Assert.Equal(0, result);
    }
    
    //TODO!!!!!
    // , helgdagar, dagar före helgdag, eller juli månad
}
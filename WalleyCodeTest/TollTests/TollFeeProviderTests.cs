using PublicHoliday;
using WalleyCodeTest.Pricing;
using WalleyCodeTest.Vehicles;

namespace TollTests;

public class TollFeeProviderTests
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
        var holidayProvider = new SwedenPublicHoliday();
        var sut = new TollFeeProvider(holidayProvider);
        
        // Act
        var result = sut.GetTollFee(car, currentTime);

        //Assert
        Assert.Equal(expectedAmount, result);
    }
    
    [Fact]
    public void Given_month_is_july_Then_toll_fee_should_be_zero()
    {
        //Arrange
        var dateInJuly = DateTime.Parse("2025-07-24 07:00");
        
        var car = new Car();
        var holidayProvider = new SwedenPublicHoliday();
        var sut = new TollFeeProvider(holidayProvider);

        // Act
        var result = sut.GetTollFee(car, dateInJuly);

        //Assert
        Assert.Equal(0, result);
    }
    
    [Theory]
    [InlineData("2013-01-01 08:00")]
    [InlineData("2013-03-28 09:00")]
    [InlineData("2013-03-29 10:00")]
    [InlineData("2013-04-01 08:00")]
    [InlineData("2013-04-30 09:00")]
    [InlineData("2013-05-01 10:00")]
    [InlineData("2013-05-08 08:00")]
    [InlineData("2013-05-09 09:00")]
    [InlineData("2013-06-05 10:00")]
    [InlineData("2013-06-06 08:00")]
    [InlineData("2013-06-21 11:00")]
    [InlineData("2013-11-01 12:00")]
    [InlineData("2013-12-24 9:00")]
    [InlineData("2013-12-25 15:00")]
    [InlineData("2013-12-26 08:00")]
    [InlineData("2013-12-31 08:00")]
    public void Given_date_is_holiday_Then_return_zero(string date)
    {
        //Arrange
        var holidayDate = DateTime.Parse(date);
        
        var car = new Car();
        var holidayProvider = new SwedenPublicHoliday();
        var sut = new TollFeeProvider(holidayProvider);

        // Act
        var result = sut.GetTollFee(car, holidayDate);

        //Assert
        Assert.Equal(0, result);
    }
    
    [Theory]
    [InlineData("2025-04-17")]
    [InlineData("2025-04-19")]
    [InlineData("2025-05-28")]
    [InlineData("2025-06-05")]
    [InlineData("2025-06-07")]
    [InlineData("2025-06-19")]
    public void Given_date_is_one_day_before_holiday_Then_return_zero(string date)
    {
        // Arrange
        var holidayDate = DateTime.Parse(date);
        
        var car = new Car();
        var holidayProvider = new SwedenPublicHoliday();
        var sut = new TollFeeProvider(holidayProvider);

        // Act
        var result = sut.GetTollFee(car, holidayDate);

        //Assert
        Assert.Equal(0, result);
    }
    
    [Fact]
    public void GetTollFee_When_saturday_Should_be_zero()
    {
        // Arrange
        var saturday = new DateTime(2025, 2, 1);

        var car = new Car();
        var holidayProvider = new SwedenPublicHoliday();
        var sut = new TollFeeProvider(holidayProvider);

        // Act
        var result = sut.GetTollFee(car, saturday);

        //Assert
        Assert.Equal(0, result);
    }
}
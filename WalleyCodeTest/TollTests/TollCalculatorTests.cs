using WalleyCodeTest;
using WalleyCodeTest.Vehicles;

namespace TollTests;

public class TollCalculatorTests
{
    //TODO use Datetime instead of string
    [Theory]
    [InlineData("2025-02-01 06:00", 8)]
    [InlineData("2025-02-01 06:29", 8)]
    [InlineData("2025-02-01 06:30", 13)]
    [InlineData("2025-02-01 06:59", 13)]
    [InlineData("2025-02-01 07:00", 18)]
    [InlineData("2025-02-01 07:59", 18)]
    [InlineData("2025-02-01 08:00", 13)]
    [InlineData("2025-02-01 08:29", 13)]
    [InlineData("2025-02-01 08:30", 8)]
    [InlineData("2025-02-01 14:59", 8)]
    [InlineData("2025-02-01 15:00", 13)]
    [InlineData("2025-02-01 15:29", 13)]
    [InlineData("2025-02-01 15:30", 18)]
    [InlineData("2025-02-01 16:59", 18)]
    [InlineData("2025-02-01 17:00", 13)]
    [InlineData("2025-02-01 17:59", 13)]
    [InlineData("2025-02-01 18:00", 8)]
    [InlineData("2025-02-01 18:29", 8)]
    [InlineData("2025-02-01 18:30", 0)]
    [InlineData("2025-02-01 05:59", 0)]
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
    
    //TODO max 60 kr per fordon
    
    
    
    
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
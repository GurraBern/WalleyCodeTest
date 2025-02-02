using System;
using System.Collections.Generic;
using PublicHoliday;
using WalleyCodeTest.Pricing;
using WalleyCodeTest.Vehicles;
using Xunit;

namespace TollTests;

public class TollCalculatorTests
{
    [Fact]
    public void GetTollFee_Given_no_dates_Should_return_0_fee()
    {
        // Arrange
        var car = new Car();
        var holidayProvider = new SwedenPublicHoliday();
        var tollFeeProvider = new TollFeeProvider(holidayProvider);
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
        var holidayProvider = new SwedenPublicHoliday();
        var tollFeeProvider = new TollFeeProvider(holidayProvider);
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
        const int highestTollFee = 13;
        
        var dates = new List<DateTime>
        {
            baseDate.AddHours(6).AddMinutes(0),
            baseDate.AddHours(6).AddMinutes(21),
            baseDate.AddHours(6).AddMinutes(30) // 13kr
        };
        
        var car = new Car();
        var holidayProvider = new SwedenPublicHoliday();
        var tollFeeProvider = new TollFeeProvider(holidayProvider);
        var sut = new TollCalculator(tollFeeProvider);

        // Act
        var result = sut.GetTotalTollFee(car, dates);

        //Assert
        Assert.Equal(highestTollFee, result);
    }

    [Theory]
    [InlineData(VehicleType.Motorbike)]
    [InlineData(VehicleType.Military)]
    [InlineData(VehicleType.Emergency)]
    [InlineData(VehicleType.Tractor)]
    [InlineData(VehicleType.Diplomat)]
    [InlineData(VehicleType.Foreign)]
    public void GetTollFee_Given_toll_free_vehicle_should_return_0_fee(VehicleType vehicleType)
    {
        // Arrange
        var timeStamps = GetTollTimestamps();
        var holidayProvider = new SwedenPublicHoliday();
        var tollFeeProvider = new TollFeeProvider(holidayProvider);
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
}
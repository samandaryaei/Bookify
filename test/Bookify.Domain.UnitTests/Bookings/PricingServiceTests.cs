using Bookify.Domain.Bookings;
using Bookify.Domain.Shared;
using Bookify.Domain.UnitTests.Apartments;
using FluentAssertions;

namespace Bookify.Domain.UnitTests.Bookings;

public class PricingServiceTests
{
    [Fact]
    public void CalculatePrice_Should_ReturbCorrectTotalPrice()
    {
        // Arrange
        var price = new Money(10.0m, Currency.FromCode("USD"));
        var period = DateRange.Create(new DateOnly(2024,1,1),new DateOnly(2024,1,10));
        var apartment = ApartmentData.Create(price);
        var pricingService = new PricingService();
        
        // Act
        var pricingDetails = pricingService.CalculatePrice(apartment, period);
        
        //Assert
        var expectedTotalPrice = new Money(price.Amount * period.LenghInDays, Currency.FromCode("USD"));
        pricingDetails.TotalPrice.Should().Be(expectedTotalPrice);
    }
    
    [Fact]
    public void CalculatePrice_Should_ReturbCorrectTotalPrice_WhenCleaningFeeIsIncluded()
    {
        // Arrange
        var price = new Money(10.0m, Currency.FromCode("USD"));
        var cleaningFee = new Money(99.99m, Currency.FromCode("USD"));
        var period = DateRange.Create(new DateOnly(2024,1,1),new DateOnly(2024,1,10));
        var apartment = ApartmentData.Create(price,cleaningFee);
        var pricingService = new PricingService();
        
        // Act
        var pricingDetails = pricingService.CalculatePrice(apartment, period);
        
        //Assert
        var expectedTotalPrice = new Money(price.Amount * period.LenghInDays + cleaningFee.Amount, Currency.FromCode("USD"));
        pricingDetails.TotalPrice.Should().Be(expectedTotalPrice);
    }
}
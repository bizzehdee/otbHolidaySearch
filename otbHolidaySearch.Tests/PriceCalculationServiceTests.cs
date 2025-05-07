using otbHolidaySearch.Models;

namespace otbHolidaySearch.Tests;

public class PriceCalculationServiceTests
{
    private IPriceCalculationService _priceCalculationService;
    
    [SetUp]
    public void Setup()
    {
        _priceCalculationService = new PriceCalculationService();
    }

    [TearDown]
    public void TearDown()
    {
        _priceCalculationService = null;
    }

    [Test]
    public void CalculatesCorrectPrice()
    {
        var result = _priceCalculationService.CalculatePrice(new FlightDataObject { Price = 100 }, new HotelDataObject { PricePerNight = 50 }, 2);
        
        // 100 + (50 * 2) = 200
        Assert.That(result, Is.EqualTo(200));
    }
    
    [Test]
    public void ReturnsErrorWithNullFlightData()
    {
        var result = _priceCalculationService.CalculatePrice(null, new HotelDataObject { PricePerNight = 50 }, 2);
        
        Assert.That(result, Is.EqualTo(-1));
    }
    
    [Test]
    public void ReturnsErrorWithNullHotelData()
    {
        var result = _priceCalculationService.CalculatePrice(new FlightDataObject { Price = 100 }, null, 2);
        
        Assert.That(result, Is.EqualTo(-1));
    }
    
    [Test]
    public void ReturnsErrorWithZeroDuration()
    {
        var result = _priceCalculationService.CalculatePrice(new FlightDataObject { Price = 100 }, new HotelDataObject { PricePerNight = 50 }, 0);
        
        Assert.That(result, Is.EqualTo(-1));
    }
    
    [Test]
    public void ReturnsErrorWithNegativeDuration()
    {
        var result = _priceCalculationService.CalculatePrice(new FlightDataObject { Price = 100 }, new HotelDataObject { PricePerNight = 50 }, -10);
        
        Assert.That(result, Is.EqualTo(-1));
    }
}
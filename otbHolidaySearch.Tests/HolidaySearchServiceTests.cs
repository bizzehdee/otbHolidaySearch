using Moq;
using otbHolidaySearch.Data;
using otbHolidaySearch.Data.Entities;
using otbHolidaySearch.Models;

namespace otbHolidaySearch.Tests;

public class HolidaySearchServiceTests
{
    private IFlightDataRepository _flightDataRepository;
    private IHotelDataRepository _hotelDataRepository;
    private IPriceCalculationService _priceCalculationService;
    private IHolidaySearchService _holidaySearchService;
    private Mock<IConfigurationService> _configurationServiceMock;

    private const string TestFlightDataFilePath = "TestData/FlightData.json";
    private const string TestHotelDataFilePath = "TestData/HotelData.json";

    [SetUp]
    public void Setup()
    {
        //mock feels better for this than building a concrete implementation
        _configurationServiceMock = new Mock<IConfigurationService>();
        _configurationServiceMock.Setup(x => x.GetConfiguration())
            .Returns(new ConfigurationDto
            {
                FlightDataConnectionString = TestFlightDataFilePath,
                HotelDataConnectionString = TestHotelDataFilePath
            });

        //i could use DI for this, but it seems a bit overkill to implement a full DI for this test, and DI in unit testing is discouraged by Microsoft
        //this is also closer to an integration test than a proper unit test, but it makes more sense to test with the repos here too
        _flightDataRepository =
            new FlightDataRepository(_configurationServiceMock.Object, new JsonDataLoader<FlightDataEntity>());
        _hotelDataRepository =
            new HotelDataRepository(_configurationServiceMock.Object, new JsonDataLoader<HotelDataEntity>());
        _priceCalculationService = new PriceCalculationService();

        _holidaySearchService = new HolidaySearchService(_flightDataRepository, _hotelDataRepository, _priceCalculationService);
    }

    [TearDown]
    public void TearDown()
    {
        _holidaySearchService = null;
    }

    [Test]
    public void CustomerOneTestCase()
    {
        var results = _holidaySearchService.Search(new HolidaySearch
        {
            DepartingFrom = ["MAN"],
            TravelingTo = "AGP",
            DepartureDate = "2023-07-01",
            Duration = 7
        });
        
        Assert.That(results, Is.Not.Null);
        Assert.That(results.Success, Is.True);

        var result = results.Results.FirstOrDefault();

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Flight, Is.Not.Null);
        Assert.That(result.Hotel, Is.Not.Null);

        Assert.That(result.Flight.Id, Is.EqualTo(2));
        Assert.That(result.Hotel.Id, Is.EqualTo(9));
    }

    [Test]
    public void CustomerTwoTestCase()
    {
        var results = _holidaySearchService.Search(new HolidaySearch
        {
            DepartingFrom = ["LTN", "LGW"],
            TravelingTo = "PMI",
            DepartureDate = "2023-06-15",
            Duration = 10
        });
        
        Assert.That(results, Is.Not.Null);
        Assert.That(results.Success, Is.True);

        var result = results.Results.FirstOrDefault();

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Flight, Is.Not.Null);
        Assert.That(result.Hotel, Is.Not.Null);

        Assert.That(result.Flight.Id, Is.EqualTo(6));
        Assert.That(result.Hotel.Id, Is.EqualTo(5));
    }

    [Test]
    public void CustomerThreeTestCase()
    {
        var results = _holidaySearchService.Search(new HolidaySearch
        {
            DepartingFrom = [],
            TravelingTo = "LPA",
            DepartureDate = "2022-11-10",
            Duration = 14
        });
        
        Assert.That(results, Is.Not.Null);
        Assert.That(results.Success, Is.True);

        var result = results.Results.FirstOrDefault();

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Flight, Is.Not.Null);
        Assert.That(result.Hotel, Is.Not.Null);

        Assert.That(result.Flight.Id, Is.EqualTo(7));
        Assert.That(result.Hotel.Id, Is.EqualTo(6));
    }

    [Test]
    public void CustomerFourTestCase()
    {
        var results = _holidaySearchService.Search(new HolidaySearch
        {
            DepartingFrom = ["LHR"],//departing from an airport we do not cover
            TravelingTo = "LPA", 
            DepartureDate = "2022-11-10",
            Duration = 14
        });
        
        Assert.That(results, Is.Not.Null);
        Assert.That(results.Success, Is.False);
    }

    [Test]
    public void CustomerFiveTestCase()
    {
        var results = _holidaySearchService.Search(new HolidaySearch
        {
            DepartingFrom = ["MAN"],
            TravelingTo = "LPA", 
            DepartureDate = "2023-11-10", //date we do not cover
            Duration = 14
        });
        
        Assert.That(results, Is.Not.Null);
        Assert.That(results.Success, Is.False);
    }

    [Test]
    public void CustomerSixTestCase()
    {
        var results = _holidaySearchService.Search(new HolidaySearch
        {
            DepartingFrom = ["MAN"],
            TravelingTo = "LTN", //destination we do not cover 
            DepartureDate = "2023-11-10", //date we do not cover
            Duration = 14
        });
        
        Assert.That(results, Is.Not.Null);
        Assert.That(results.Success, Is.False);
    }

    [Test]
    public void ExceptionIfDateIsNotValid()
    {
        Assert.Throws<ArgumentException>(() => _holidaySearchService.Search(new HolidaySearch
        {
            DepartureDate = "xxx-xxx-xxx"
        }));
    }

    [Test]
    public void ExceptionIfInvalidDuration()
    {
        Assert.Throws<ArgumentException>(() => _holidaySearchService.Search(new HolidaySearch
        {
            Duration = -1
        }));
    }

    [Test]
    public void ExceptionIfNull()
    {
        Assert.Throws<ArgumentNullException>(() => _holidaySearchService.Search(null));
    }
}

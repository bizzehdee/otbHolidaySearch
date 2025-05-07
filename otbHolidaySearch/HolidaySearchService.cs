using otbHolidaySearch.Data;
using otbHolidaySearch.Models;

namespace otbHolidaySearch;

public class HolidaySearchService : IHolidaySearchService
{
    private readonly IFlightDataRepository _flightDataRepository;
    private readonly IHotelDataRepository _hotelDataRepository;
    private readonly IPriceCalculationService _priceCalculationService;

    public HolidaySearchService(IFlightDataRepository flightDataRepository, IHotelDataRepository hotelDataRepository, IPriceCalculationService priceCalculationService)
    {
        _flightDataRepository = flightDataRepository;
        _hotelDataRepository = hotelDataRepository;
        _priceCalculationService = priceCalculationService;
    }

    private IEnumerable<FlightDataObject> GetAllValidFlights(string[] canDepartFrom, string travelingTo, DateTime departureDate)
    {
        var flightData = _flightDataRepository.GetFlightData();
        if (flightData == null)
        {
            //i would usually use an application specific exception here, but it seems excessive here
            throw new Exception("Flight data not found");
        }
        
        return flightData
            .Where(f => (canDepartFrom.Contains(f.From) || canDepartFrom.Length == 0) &&
                        f.To == travelingTo &&
                        f.DepartureDate.Date == departureDate.Date)
            .OrderBy(f => f.Price);
    }

    private IEnumerable<HotelDataObject> GetAllValidHotels(string travelingTo, DateTime departureDate, int duration)
    {
        var hotelData = _hotelDataRepository.GetHotelData();
        
        if (hotelData == null)
        {
            //i would usually use an application specific exception here, but it seems excessive here
            throw new Exception("Hotel data not found");
        }
        
        return hotelData.Where(f =>
                f.LocalAirports.Contains(travelingTo) && f.ArrivalDate.Date == departureDate.Date &&
                f.Nights == duration)
            .OrderBy(f => f.PricePerNight);
    }

    public HolidayResults Search(HolidaySearch search)
    {
        if (search == null)
        {
            throw new ArgumentNullException(nameof(search));
        }
        
        if(search.Duration <= 0)
        {
            throw new ArgumentException("Duration must be greater than 0", nameof(search.Duration));
        }

        if (!DateTime.TryParse(search.DepartureDate, out var departureDate))
        {
            throw new ArgumentException("Departure date is not a valid date", nameof(search));
        }
        
        // i would usually check to see if the date is in the past,
        // but not for this case as we have test data that is all in the past
        
        //get all possible flights that match the criteria
        var flights = GetAllValidFlights(search.DepartingFrom, search.TravelingTo, departureDate);

        //get all possible hotels that match the criteria
        var hotels = GetAllValidHotels(search.TravelingTo, departureDate, search.Duration);

        var results = new List<HolidayResult>();
        
        //no results found, return empty list
        if(flights == null || hotels == null || flights.Count() == 0 || hotels.Count() == 0)
        {
            return new HolidayResults
            {
                Message = "No results found",
                Success = false,
                Results = []
            };
        }
        
        //easy and readable way to create a combination of all flights and hotels, and calculate the total price
        foreach (var flight in flights)
        {
            foreach (var hotel in hotels)
            {
                results.Add(new HolidayResult
                {
                    Flight = flight,
                    Hotel = hotel,
                    TotalPrice = _priceCalculationService.CalculatePrice(flight, hotel, search.Duration)
                });
            }
        }

        //return the results, ordered by the total price, lowest to highest
        return new HolidayResults
        {
            Message = results.Count == 0 ? "No results found" : "Results found",
            Success = results.Count > 0,
            Results = results.OrderBy(f => f.TotalPrice)
        };
    }
}
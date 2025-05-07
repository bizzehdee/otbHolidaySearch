using otbHolidaySearch.Data.Entities;
using otbHolidaySearch.Models;

namespace otbHolidaySearch;

public interface IPriceCalculationService
{
    int CalculatePrice(FlightDataObject flightData, HotelDataObject hotelData, int duration);
}
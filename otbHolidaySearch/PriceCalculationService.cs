using otbHolidaySearch.Data.Entities;
using otbHolidaySearch.Models;

namespace otbHolidaySearch;

public class PriceCalculationService : IPriceCalculationService
{
    public int CalculatePrice(FlightDataObject flightData, HotelDataObject hotelData, int duration)
    {
        if(flightData == null || hotelData == null || duration <= 0)
        {
            // Return 0 if any of the inputs are invalid
            return -1;
        }
        // Calculate the total price based on flight and hotel data
        var flightPrice = flightData.Price;
        var hotelPrice = hotelData.PricePerNight * duration;

        // Return the total price
        return flightPrice + hotelPrice;
    }
}
namespace otbHolidaySearch.Models;

public class HolidayResult
{
    public int TotalPrice { get; set; }
    public FlightDataObject Flight { get; set; }
    public HotelDataObject Hotel { get; set; }
}
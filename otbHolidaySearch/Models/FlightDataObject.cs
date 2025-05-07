namespace otbHolidaySearch.Data;

public class FlightDataObject
{
    public int Id { get; set; }
    public string Airline { get; set; }
    public string From { get; set; }
    public string To { get; set; }
    public int Price { get; set; }
    public DateTime DepartureDate { get; set; }
}
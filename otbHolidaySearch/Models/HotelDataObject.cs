namespace otbHolidaySearch.Data;

public class HotelDataObject
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime ArrivalDate { get; set; }
    public int PricePerNight { get; set; }
    public int Nights { get; set; }
    public string[] LocalAirports { get; set; }
}

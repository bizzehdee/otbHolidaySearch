using System.Text.Json.Serialization;

namespace otbHolidaySearch.Data.Entities;

public class HotelDataEntity
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    
    [JsonPropertyName("name")]
    public string Name { get; set; }
    
    [JsonPropertyName("arrival_date")]
    public string ArrivalDate { get; set; }
    
    [JsonPropertyName("price_per_night")]
    public int PricePerNight { get; set; }
    
    [JsonPropertyName("nights")]
    public int Nights { get; set; }
    
    [JsonPropertyName("local_airports")]
    public string[] LocalAirports { get; set; }
}
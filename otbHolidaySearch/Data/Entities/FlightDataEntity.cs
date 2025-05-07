using System.Text.Json.Serialization;

namespace otbHolidaySearch.Data.Entities;

public class FlightDataEntity
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("airline")]
    public string Airline { get; set; }
    [JsonPropertyName("from")]
    public string From { get; set; }
    [JsonPropertyName("to")]
    public string To { get; set; }
    [JsonPropertyName("price")]
    public int Price { get; set; }
    [JsonPropertyName("departure_date")]
    public string DepartureDate { get; set; }
}

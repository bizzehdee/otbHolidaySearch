namespace otbHolidaySearch.Models;

public class HolidayResults
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public IEnumerable<HolidayResult> Results { get; set; }
}
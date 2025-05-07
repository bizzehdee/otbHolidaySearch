using otbHolidaySearch.Data.Entities;
using otbHolidaySearch.Models;

namespace otbHolidaySearch.Data;

public interface IFlightDataRepository
{
    IEnumerable<FlightDataObject> GetFlightData();
}
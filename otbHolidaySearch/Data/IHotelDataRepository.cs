using otbHolidaySearch.Models;

namespace otbHolidaySearch.Data;

public interface IHotelDataRepository
{
    IEnumerable<HotelDataObject> GetHotelData();
}
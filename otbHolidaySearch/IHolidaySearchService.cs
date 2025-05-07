using otbHolidaySearch.Models;

namespace otbHolidaySearch;

public interface IHolidaySearchService
{
    HolidayResults Search(HolidaySearch search);
}
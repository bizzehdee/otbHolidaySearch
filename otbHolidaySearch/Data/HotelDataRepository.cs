using otbHolidaySearch.Data.Entities;
using otbHolidaySearch.Models;

namespace otbHolidaySearch.Data;

public class HotelDataRepository : IHotelDataRepository
{
    private readonly IDataLoader<HotelDataEntity> _hotelDataLoader;
    private readonly ConfigurationDto _configuration;

    public HotelDataRepository(IConfigurationService configurationService, IDataLoader<HotelDataEntity> hotelDataLoader)
    {
        _hotelDataLoader = hotelDataLoader;

        _configuration = configurationService.GetConfiguration();
    }
    
    public IEnumerable<HotelDataObject> GetHotelData()
    {
        var hotelDataEntities = 
            _hotelDataLoader.LoadData(_configuration.HotelDataConnectionString);
        
        if (hotelDataEntities == null)
        {
            throw new Exception("Hotel data not found");
        }
        
        return hotelDataEntities.Select(f=> new HotelDataObject
        {
            Id = f.Id,
            Name = f.Name,
            ArrivalDate = DateTime.Parse(f.ArrivalDate),
            PricePerNight = f.PricePerNight,
            Nights = f.Nights,
            LocalAirports = f.LocalAirports
        });
    }
}

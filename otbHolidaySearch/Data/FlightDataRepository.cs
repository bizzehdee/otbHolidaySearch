using otbHolidaySearch.Data.Entities;
using otbHolidaySearch.Models;

namespace otbHolidaySearch.Data;

public class FlightDataRepository : IFlightDataRepository
{
    private readonly IDataLoader<FlightDataEntity> _flightDataLoader;
    private readonly ConfigurationDto _configuration;

    public FlightDataRepository(IConfigurationService configurationService, IDataLoader<FlightDataEntity> flightDataLoader)
    {
        _flightDataLoader = flightDataLoader;

        _configuration = configurationService.GetConfiguration();
    }

    public IEnumerable<FlightDataObject> GetFlightData()
    {
        var flightDataEntities = 
            _flightDataLoader.LoadData(_configuration.FlightDataConnectionString);
        
        if (flightDataEntities == null)
        {
            throw new Exception("Flight data not found");
        }
        
        // using a mapper feels like overkill, but no matter what type of data loader we use
        // be it json, csv, database... we do not want to leak entities to the application or the user
        return flightDataEntities.Select(f=> new FlightDataObject
        {
            Id = f.Id,
            From = f.From,
            To = f.To,
            Airline = f.Airline,
            Price = f.Price,
            DepartureDate = DateTime.Parse(f.DepartureDate)
        });
    }
}
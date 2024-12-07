using Database_project.Core.Entities;

namespace Database_project.Core.Interfaces;

public interface IAirportService
{
    Task<Airport?> GetAirportByIdAsync(long id);
    Task<Airport> CreateAirportAsync(Airport airport);
    Task<Airport> UpdateAirportAsync(Airport airport);
    Task<Airport> DeleteAirportAsync(long id);
}
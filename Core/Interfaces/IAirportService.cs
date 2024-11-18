using Database_project.Core.Entities;

namespace Database_project.Core.Interfaces;

public interface IAirportService
{
    Task<Airport?> GetAirportByIdAsync(long id);
    Task<Airport> CreateAirportAsync(Airport airport);
    Task<bool> UpdateAirportAsync(long id, Airport airport);
    Task<bool> DeleteAirportAsync(long id);
}
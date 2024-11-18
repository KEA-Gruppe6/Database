using Database_project.Core.DTOs;
using Database_project.Core.Entities;

namespace Database_project.Core.Interfaces;

public interface IAirlineService
{
    Task<AirlineDTO?> GetAirlineByIdAsync(long id);
    Task<Airline> CreateAirlineAsync(Airline airline);
    Task<bool> UpdateAirlineAsync(Airline airline);
    Task<bool> DeleteAirlineAsync(long id);
}
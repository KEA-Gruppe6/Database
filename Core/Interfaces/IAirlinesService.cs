using Database_project.Core.DTOs;
using Database_project.Core.Entities;

namespace Database_project.Core.Interfaces;

public interface IAirlineService
{
    Task<AirlineDTO?> GetAirlineByIdAsync(long id);
    Task<AirlineDTO> CreateAirlineAsync(Airline airline);
    Task<AirlineDTO> UpdateAirlineAsync(Airline airline);
    Task<Airline> DeleteAirlineAsync(long id);
}
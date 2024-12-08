using Database_project.Core.SQL.DTOs;
using Database_project.Core.SQL.Entities;

namespace Database_project.Core.SQL.Interfaces;

public interface IAirlineService
{
    Task<AirlineDTO_Planes?> GetAirlineByIdAsync(long id);
    Task<AirlineDTO_Planes> CreateAirlineAsync(Airline airline);
    Task<AirlineDTO_Planes> UpdateAirlineAsync(Airline airline);
    Task<Airline> DeleteAirlineAsync(long id);
}
using Database_project.Core.SQL.DTOs;
using Database_project.Core.SQL.Entities;

namespace Database_project.Core.SQL.Interfaces;

public interface IFlightrouteService
{
    Task<FlightrouteDTO?> GetFlightrouteByIdAsync(long id);
    Task<FlightrouteDTO> CreateFlightrouteAsync(Flightroute flightroute);
    Task<FlightrouteDTO> UpdateFlightrouteAsync(Flightroute flightroute);
    Task<Flightroute> DeleteFlightrouteAsync(long id);
}
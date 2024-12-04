using Database_project.Core.DTOs;
using Database_project.Core.Entities;

namespace Database_project.Core.Interfaces;

public interface IFlightrouteService
{
    Task<FlightrouteDTO?> GetFlightrouteByIdAsync(long id);
    Task<FlightrouteDTO> CreateFlightrouteAsync(Flightroute flightroute);
    Task<FlightrouteDTO> UpdateFlightrouteAsync(Flightroute flightroute);
    Task<Flightroute> DeleteFlightrouteAsync(long id);
}
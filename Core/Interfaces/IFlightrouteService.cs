using Database_project.Core.DTOs;
using Database_project.Core.Entities;

namespace Database_project.Core.Interfaces;

public interface IFlightrouteService
{
    Task<Route?> GetFlightrouteByIdAsync(long id);
    Task<Route> CreateFlightrouteAsync(Flightroute flightroute);
    Task<bool> UpdateFlightrouteAsync(Flightroute updatedFlightroute);
    Task<bool> DeleteFlightrouteAsync(long id);
}
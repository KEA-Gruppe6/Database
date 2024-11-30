using Database_project.Core.Entities;
using Database_project.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Database_project.Controllers;

[ApiController]
[Route("[controller]")]
public class FlightrouteController : ControllerBase
{
    private readonly IFlightrouteService _flightrouteService;

    public FlightrouteController(IFlightrouteService flightrouteService)
    {
        _flightrouteService = flightrouteService;
    }

    [HttpGet]
    public string GetFlightroute(int id)
    {
        throw new NotImplementedException();
    }

    [HttpPost]
    public string CreateFlightroute([FromBody] Flightroute flightroute)
    {
        throw new NotImplementedException();
    }

    [HttpPatch]
    public string UpdateFlightroute([FromBody] Flightroute flightroute)
    {
        throw new NotImplementedException();
    }

    [HttpDelete]
    public string DeleteFlightroute(int id)
    {
        throw new NotImplementedException();
    }
}
using Database_project.Neo4j.Entities;
using Database_project.Neo4j.Services;
using Microsoft.AspNetCore.Mvc;

namespace Database_project.Neo4jControllers
{
    [Route("api/neo4j/[controller]")]
    [ApiController]
    public class MaintenanceController : ControllerBase
    {
        private readonly MaintenanceService _maintenanceService;

        public MaintenanceController(MaintenanceService maintenanceService)
        {
            _maintenanceService = maintenanceService;
        }

        [HttpGet("plane/{planeId}")]
        public async Task<ActionResult<IEnumerable<Maintenance>>> GetMaintenanceByPlaneId(long planeId)
        {
            var maintenances = await _maintenanceService.GetMaintenanceByPlaneIdAsync(planeId);
            return Ok(maintenances);
        }

        [HttpDelete("{maintainanceId}")]
        public async Task<IActionResult> DeleteMaintenance(long maintainanceId)
        {
            await _maintenanceService.DeleteMaintenanceAsync(maintainanceId);
            return NoContent();
        }
    }
}

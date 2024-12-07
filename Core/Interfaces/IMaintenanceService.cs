using Database_project.Core.DTOs;
using Database_project.Core.Entities;

namespace Database_project.Core.Interfaces;

public interface IMaintenanceService
{
    Task<MaintenanceDTO?> GetMaintenanceByIdAsync(long id);
    Task<MaintenanceDTO> CreateMaintenanceAsync(Maintenance maintenance);
    Task<MaintenanceDTO> UpdateMaintenanceAsync(Maintenance maintenance);
    Task<Maintenance> DeleteMaintenanceAsync(long id);
}
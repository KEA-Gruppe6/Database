using Database_project.Core.SQL.DTOs;
using Database_project.Core.SQL.Entities;

namespace Database_project.Core.SQL.Interfaces;

public interface IMaintenanceService
{
    Task<MaintenanceDTO?> GetMaintenanceByIdAsync(long id);
    Task<MaintenanceDTO> CreateMaintenanceAsync(Maintenance maintenance);
    Task<MaintenanceDTO> UpdateMaintenanceAsync(Maintenance maintenance);
    Task<Maintenance> DeleteMaintenanceAsync(long id);
}
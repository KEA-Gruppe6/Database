using Database_project.Core.SQL.DTOs;
using Database_project.Core.SQL.Entities;

namespace Database_project.Core.SQL.Interfaces;

public interface ILuggageService
{
    Task<Luggage?> GetLuggageByIdAsync(long id);
    Task<Luggage> CreateLuggageAsync(Luggage luggage);
    Task<Luggage> UpdateLuggageAsync(Luggage luggage);
    Task<Luggage> DeleteLuggageAsync(long id);
}
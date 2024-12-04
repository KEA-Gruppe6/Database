using Database_project.Core.DTOs;
using Database_project.Core.Entities;

namespace Database_project.Core.Interfaces;

public interface ILuggageService
{
    Task<Luggage?> GetLuggageByIdAsync(long id);
    Task<Luggage> CreateLuggageAsync(Luggage luggage);
    Task<Luggage> UpdateLuggageAsync(Luggage luggage);
    Task<Luggage> DeleteLuggageAsync(long id);
}
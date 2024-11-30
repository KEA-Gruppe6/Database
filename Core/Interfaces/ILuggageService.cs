using Database_project.Core.DTOs;
using Database_project.Core.Entities;

namespace Database_project.Core.Interfaces;

public interface ILuggageService
{
    Task<Luggage?> GetLuggageByIdAsync(long id);
    Task<Luggage> CreateLuggageAsync(Luggage luggage);
    Task<bool> UpdateLuggageAsync(Luggage updatedLuggage);
    Task<bool> DeleteLuggageAsync(long id);
}
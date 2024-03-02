using Hexagonal_Refactoring.Models;

namespace Hexagonal_Refactoring.DTOs;

public record NewEventDto(string? Name, string? Date, int TotalSpots, Partner? Partner);
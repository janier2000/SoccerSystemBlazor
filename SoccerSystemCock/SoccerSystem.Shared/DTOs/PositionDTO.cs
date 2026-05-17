using SoccerSystem.Shared.Entites;

namespace SoccerSystem.Shared.DTOs;

public class PositionDTO
{
    public User User { get; set; } = null!;

    public int Points { get; set; }
}
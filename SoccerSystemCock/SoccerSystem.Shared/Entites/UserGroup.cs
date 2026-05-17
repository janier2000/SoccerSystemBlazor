using System.ComponentModel.DataAnnotations;

namespace SoccerSystem.Shared.Entites;

public class UserGroup
{
    public int Id { get; set; }

    public bool IsActive { get; set; }

    [MaxLength(450)]
    public string UserId { get; set; } = null!;

    public User User { get; set; } = null!;

    public int GroupId { get; set; }
    public Group Group { get; set; } = null!;
}
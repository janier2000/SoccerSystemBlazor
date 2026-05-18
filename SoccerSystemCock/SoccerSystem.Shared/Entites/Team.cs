using SoccerSystem.Shared.Resources;
using System.ComponentModel.DataAnnotations;

namespace SoccerSystem.Shared.Entites;

public class Team
{
    public int Id { get; set; }

    [Display(Name = "Team", ResourceType = typeof(Literals))]
    [MaxLength(100, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public string Name { get; set; } = null!;

    public string? Image { get; set; }

    public int CountryId { get; set; }
    public Country? Country { get; set; }

    //[Display(Name = "IsImageSquare", ResourceType = typeof(Literals))]
    //public bool IsImageSquare { get; set; }

    public string ImageFull => string.IsNullOrEmpty(Image) ? "/images/NoImage.png" : Image;

    public ICollection<TournamentTeam>? TournamentTeams { get; set; }
    public int TournamentsCount => TournamentTeams == null ? 0 : TournamentTeams.Count;
}
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using SoccerSystem.Shared.Resources;

namespace SoccerSystem.Frontend.Pages;

public partial class About
{
    [Inject] private IStringLocalizer<Literals> Localizer { get; set; } = null!;
}
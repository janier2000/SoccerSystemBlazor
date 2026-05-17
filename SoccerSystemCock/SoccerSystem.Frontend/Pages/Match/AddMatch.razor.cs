using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using SoccerSystem.Frontend.Repositories;
using SoccerSystem.Shared.DTOs;
using SoccerSystem.Shared.Resources;

namespace SoccerSystem.Frontend.Pages.Match;

public partial class AddMatch
{
    private MatchDTO? matchDTO;
    private MatchForm? addMatchForm;

    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private IRepository Repository { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;
    [Inject] private IStringLocalizer<Literals> Localizer { get; set; } = null!;

    [Parameter] public int Id { get; set; }

    protected override void OnParametersSet()
    {
        base.OnParametersSet();
        matchDTO = new MatchDTO
        {
            IsActive = true,
            TournamentId = Id,
        };
    }

    private async Task AddAsync()
    {
        matchDTO!.Date = matchDTO.Date.ToUniversalTime();
        var responseHttp = await Repository.PostAsync("api/Matches/full", matchDTO);

        if (responseHttp.Error)
        {
            var mensajeError = await responseHttp.GetErrorMessageAsync();
            Snackbar.Add(Localizer[mensajeError!], Severity.Error);
            return;
        }

        Return();
        Snackbar.Add(Localizer["RecordCreatedOk"], Severity.Success);
    }

    private void Return()
    {
        addMatchForm!.FormPostedSuccessfully = true;
        NavigationManager.NavigateTo($"/matches/{Id}");
    }
}
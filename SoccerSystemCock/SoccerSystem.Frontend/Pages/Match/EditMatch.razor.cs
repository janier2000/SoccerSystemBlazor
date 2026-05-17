using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using SoccerSystem.Frontend.Repositories;
using SoccerSystem.Shared.DTOs;
using SoccerSystem.Shared.Resources;
using e = SoccerSystem.Shared.Entites;

namespace SoccerSystem.Frontend.Pages.Match;

public partial class EditMatch
{
    private MatchDTO? matchDTO;
    private MatchForm? addMatchForm;

    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private IRepository Repository { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;
    [Inject] private IStringLocalizer<Literals> Localizer { get; set; } = null!;

    [Parameter] public int Id { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var responseHttp = await Repository.GetAsync<e.Match>($"api/Matches/{Id}");

        if (responseHttp.Error)
        {
            if (responseHttp.HttpResponseMessage.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                NavigationManager.NavigateTo("tournaments");
            }
            else
            {
                var messageError = await responseHttp.GetErrorMessageAsync();
                Snackbar.Add(messageError, Severity.Error);
            }
        }
        else
        {
            var match = responseHttp.Response;
            matchDTO = new MatchDTO()
            {
                Id = match!.Id,
                IsActive = match!.IsActive,
                Date = match!.Date,
                GoalsLocal = match!.GoalsLocal,
                GoalsVisitor = match!.GoalsVisitor,
                LocalId = match!.LocalId,
                TournamentId = match!.TournamentId,
                VisitorId = match!.VisitorId,
            };
        }
    }

    private async Task EditAsync()
    {
        matchDTO!.Date = matchDTO.Date.ToUniversalTime();
        var responseHttp = await Repository.PutAsync("api/Matches/full", matchDTO);

        if (responseHttp.Error)
        {
            var mensajeError = await responseHttp.GetErrorMessageAsync();
            Snackbar.Add(Localizer[mensajeError!], Severity.Error);
            return;
        }

        Return();
        Snackbar.Add(Localizer["RecordSavedOk"], Severity.Success);
    }

    private void Return()
    {
        addMatchForm!.FormPostedSuccessfully = true;
        NavigationManager.NavigateTo($"/matches/{matchDTO!.TournamentId}");
    }
}
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using SoccerSystem.Frontend.Repositories;
using SoccerSystem.Shared.DTOs;
using SoccerSystem.Shared.Entites;
using SoccerSystem.Shared.Resources;

namespace SoccerSystem.Frontend.Pages.Tournaments;

public partial class TournamentEdit
{
    private TournamentDTO? tournamentDTO;
    private TournamentForm? tournamentForm;

    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private IRepository Repository { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;
    [Inject] private IStringLocalizer<Literals> Localizer { get; set; } = null!;

    [Parameter] public int Id { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var responseHttp = await Repository.GetAsync<Tournament>($"api/tournaments/{Id}");

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
            var tournament = responseHttp.Response;
            tournamentDTO = new TournamentDTO()
            {
                Id = tournament!.Id,
                Name = tournament!.Name,
                Image = tournament.Image,
                IsActive = tournament!.IsActive,
                Remarks = tournament!.Remarks,
            };
        }
    }

    private async Task EditAsync()
    {
        var responseHttp = await Repository.PutAsync("api/tournaments/full", tournamentDTO);

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
        tournamentForm!.FormPostedSuccessfully = true;
        NavigationManager.NavigateTo("tournaments");
    }
}
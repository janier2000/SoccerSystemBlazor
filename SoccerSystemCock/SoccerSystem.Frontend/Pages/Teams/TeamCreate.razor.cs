using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.Win32;
using MudBlazor;
using SoccerSystem.Frontend.Repositories;
using SoccerSystem.Shared.DTOs;
using SoccerSystem.Shared.Resources;

namespace SoccerSystem.Frontend.Pages.Teams;

public partial class TeamCreate
{
    private TeamForm? teamForm;
    private TeamDTO teamDTO = new();

    [Inject] private IRepository Repository { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;
    [Inject] private IStringLocalizer<Literals> Localizer { get; set; } = null!;

    private async Task CreateAsync()
    {
        var responseHttp = await Repository.PostAsync("/api/teams/Create", teamDTO);
        if (responseHttp.Error)
        {
            var message = await responseHttp.GetErrorMessageAsync();
            Snackbar.Add(Localizer[message!], Severity.Error);
            return;
        }

        Return();
        Snackbar.Add(Localizer["RecordCreatedOk"], Severity.Success); // Registro creado con éxito.
    }

    private void Return()
    {
        teamForm!.FormPostedSuccessfully = true;
        NavigationManager.NavigateTo("/teams");
    }
}
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using SoccerSystem.Frontend.Repositories;
using SoccerSystem.Shared.Entites;
using SoccerSystem.Shared.Resources;

namespace SoccerSystem.Frontend.Pages.Groups;

public partial class GroupDetails
{
    private Group? group;

    [Inject] private IStringLocalizer<Literals> Localizer { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private IRepository Repository { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;

    [Parameter] public int GroupId { get; set; }

    protected override async Task OnParametersSetAsync()
    {
        await LoadGroupAsync();
        await CheckPredictionsForAllMatchesAsync();
    }

    private async Task CheckPredictionsForAllMatchesAsync()
    {
        var responseHttp = await Repository.GetAsync($"api/groups/CheckPredictionsForAllMatches/{GroupId}");
    }

    private async Task LoadGroupAsync()
    {
        var responseHttp = await Repository.GetAsync<Group>($"api/groups/{GroupId}");

        if (responseHttp.Error)
        {
            if (responseHttp.HttpResponseMessage.StatusCode != System.Net.HttpStatusCode.NotFound)
            {
                var messageError = await responseHttp.GetErrorMessageAsync();
                Snackbar.Add(messageError, Severity.Error);
            }
            NavigationManager.NavigateTo("groups");
            return;
        }
        group = responseHttp.Response;
    }
}
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using SoccerSystem.Frontend.Repositories;
using SoccerSystem.Shared.DTOs;
using SoccerSystem.Shared.Resources;

namespace SoccerSystem.Frontend.Pages.JoinGroup;

public partial class JoinGroup
{
    private JoinGroupForm? joinGroupForm;
    private JoinGroupDTO joinGroupDTO = new();

    [Inject] private IRepository Repository { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;
    [Inject] private IStringLocalizer<Literals> Localizer { get; set; } = null!;
    [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;

    private async Task CreateAsync()
    {
        var responseHttp = await Repository.PostAsync("/api/usergroups/join", joinGroupDTO);
        if (responseHttp.Error)
        {
            var message = await responseHttp.GetErrorMessageAsync();
            Snackbar.Add(Localizer[message!], Severity.Error);
            return;
        }
        var group = responseHttp.Response;

        Return();
        Snackbar.Add(Localizer["UserAddedToGroupOk"], Severity.Success);
    }

    private void Return()
    {
        joinGroupForm!.FormPostedSuccessfully = true;
        NavigationManager.NavigateTo("/groups");
    }
}
using CurrieTechnologies.Razor.SweetAlert2;
using Fantasy.Shared.Resources;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using SoccerSystem.Frontend.Helpers;
using SoccerSystem.Frontend.Repositories;
using SoccerSystem.Shared.DTOs;
using SoccerSystem.Shared.Entites;
using SoccerSystem.Shared.Resources;

namespace SoccerSystem.Frontend.Pages.Groups;

public partial class GroupCreate
{
    private GroupForm? groupForm;
    private GroupDTO groupDTO = new() { IsActive = true };

    [Inject] private IRepository Repository { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;
    [Inject] private IStringLocalizer<Literals> Localizer { get; set; } = null!;
    [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;

    [Inject] private IStringLocalizer<Parameters> Parameters { get; set; } = null!;
    [Inject] private IClipboardService ClipboardService { get; set; } = null!;

    private async Task CreateAsync()
    {
        groupDTO.Code = "123456";
        groupDTO.AdminId = "123456";
        var responseHttp = await Repository.PostAsync<GroupDTO, Group>("/api/groups/full", groupDTO);
        if (responseHttp.Error)
        {
            var message = await responseHttp.GetErrorMessageAsync();
            Snackbar.Add(Localizer[message!], Severity.Error);
            return;
        }
        var group = responseHttp.Response;
        var joinURL = $"{Parameters["URLFront"]}/groups/join/?code={group!.Code}";
        await ClipboardService.CopyToClipboardAsync(joinURL);

        Return();
        var result = await SweetAlertService.FireAsync(new SweetAlertOptions
        {
            Title = Localizer["Confirmation"],
            //Text = string.Format(Localizer["GroupCreated"], group!.Name, group.Code),
            Text = string.Format(Localizer["GroupCreated"], group!.Name, group.Code, joinURL),
            Icon = SweetAlertIcon.Info,
        });
    }

    private void Return()
    {
        groupForm!.FormPostedSuccessfully = true;
        NavigationManager.NavigateTo("/groups");
    }
}
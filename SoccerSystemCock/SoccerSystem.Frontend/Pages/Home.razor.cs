using Fantasy.Shared.Resources;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using SoccerSystem.Frontend.Helpers;
using SoccerSystem.Frontend.Repositories;
using SoccerSystem.Shared.Entites;
using SoccerSystem.Shared.Resources;

namespace SoccerSystem.Frontend.Pages;

public partial class Home
{
    private const string baseUrl = "api/groups";
    private List<Group>? Groups { get; set; }

    [Inject] private IStringLocalizer<Literals> Localizer { get; set; } = null!;
    [Inject] private IRepository Repository { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;
    [Inject] private IClipboardService ClipboardService { get; set; } = null!;
    [Inject] private IStringLocalizer<Parameters> Parameters { get; set; } = null!;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        await LoadGroupsAsync();
    }

    private async Task LoadGroupsAsync()
    {
        var url = $"{baseUrl}/all";
        var responseHttp = await Repository.GetAsync<List<Group>>(url);
        if (responseHttp.Error)
        {
            var message = await responseHttp.GetErrorMessageAsync();
            Snackbar.Add(Localizer[message!], Severity.Error);
            return;
        }
        Groups = responseHttp.Response;
    }

    private async Task CopyInvitationAsync(Group group)
    {
        var joinURL = $"{Parameters["URLFront"]}/groups/join/?code={group!.Code}";
        await ClipboardService.CopyToClipboardAsync(joinURL);
        var text = string.Format(Localizer["InvitationURLCopied"], group!.Name);
        Snackbar.Add(text, Severity.Success);
    }
}
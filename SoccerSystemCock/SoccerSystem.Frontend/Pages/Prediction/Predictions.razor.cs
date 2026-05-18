using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using SoccerSystem.Frontend.Repositories;
using SoccerSystem.Shared.Entites;
using SoccerSystem.Shared.Resources;

using e = SoccerSystem.Shared.Entites;

namespace SoccerSystem.Frontend.Pages.Prediction;

[Authorize(Roles = "Admin, User")]
public partial class Predictions
{
    private List<e.Prediction>? predictions;
    private MudTable<e.Prediction> table = new();
    private readonly int[] pageSizeOptions = { 10, 25, 50, int.MaxValue };
    private int totalRecords = 0;
    private bool loading;
    private const string baseUrlMatch = "api/predictions";
    private string infoFormat = "{first_item}-{last_item} de {all_items}";
    private bool userEnabledForGroup;
    private string username = string.Empty;

    [Parameter] public int GroupId { get; set; }

    [Inject] private IRepository Repository { get; set; } = null!;
    [Inject] private IDialogService DialogService { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private IStringLocalizer<Literals> Localizer { get; set; } = null!;

    [Parameter, SupplyParameterFromQuery] public string Filter { get; set; } = string.Empty;

    protected override async Task OnInitializedAsync()
    {
        await LoadAsync();
    }

    private async Task LoadAsync()
    {
        await LoadTotalRecords();
    }

    private async Task<bool> LoadTotalRecords()
    {
        loading = true;

        var url = $"{baseUrlMatch}/totalRecordsPaginated/?id={GroupId}";
        if (!string.IsNullOrWhiteSpace(Filter))
        {
            url += $"&filter={Filter}";
        }
        var responseHttp = await Repository.GetAsync<int>(url);
        if (responseHttp.Error)
        {
            var message = await responseHttp.GetErrorMessageAsync();
            Snackbar.Add(Localizer[message!], Severity.Error);
            return false;
        }
        totalRecords = responseHttp.Response;
        loading = false;
        return true;
    }

    private async Task<TableData<e.Prediction>> LoadListAsync(TableState state, CancellationToken cancellationToken)
    {
        int page = state.Page + 1;
        int pageSize = state.PageSize;
        var url = $"{baseUrlMatch}/paginated?id={GroupId}&page={page}&recordsnumber={pageSize}";

        if (!string.IsNullOrWhiteSpace(Filter))
        {
            url += $"&filter={Filter}";
        }

        var responseHttp = await Repository.GetAsync<List<e.Prediction>>(url);
        if (responseHttp.Error)
        {
            var message = await responseHttp.GetErrorMessageAsync();
            Snackbar.Add(Localizer[message!], Severity.Error);
            return new TableData<e.Prediction> { Items = [], TotalItems = 0 };
        }
        if (responseHttp.Response == null)
        {
            return new TableData<e.Prediction> { Items = [], TotalItems = 0 };
        }
        return new TableData<e.Prediction>
        {
            Items = responseHttp.Response,
            TotalItems = totalRecords
        };
    }

    private async Task SetFilterValue(string value)
    {
        Filter = value;
        await LoadAsync();
        await table.ReloadServerData();
    }

    private void ReturnAction()
    {
        NavigationManager.NavigateTo("/groups");
    }

    private async Task EditPredictionAsync(e.Prediction prediction)
    {
        //TODO: Pending
    }

    private async Task WatchPredictionAsync(e.Prediction prediction)
    {
        //TODO: Pending
    }

    private bool CanWatch(e.Prediction prediction)
    {
        if (prediction.Match.GoalsLocal != null || prediction.Match.GoalsVisitor != null)
        {
            return true;
        }

        var dateMatch = prediction.Match.Date.ToLocalTime();
        var currentDate = DateTime.Now;
        var minutesMatch = dateMatch.Subtract(DateTime.MinValue).TotalMinutes;
        var minutesNow = currentDate.Subtract(DateTime.MinValue).TotalMinutes;
        var difference = minutesNow - minutesMatch;
        var canWatch = difference >= -10;
        return canWatch; 
        
    }

    private async Task EditPredictionAsync(int id)
    {
        var options = new DialogOptions() { CloseOnEscapeKey = true, CloseButton = true };
        var parameters = new DialogParameters
    {
        { "Id", id }
    };
        var dialog = DialogService.Show<PredictionEdit>($"{Localizer["Edit"]} {Localizer["Prediction"]}", parameters, options);

        var result = await dialog.Result;
        if (result!.Canceled)
        {
            await LoadAsync();
            await table.ReloadServerData();
        }
    }

    private async Task WatchPredictionsAsync(e.Prediction prediction)
    {
        var options = new DialogOptions()
        {
            CloseOnEscapeKey = true,
            CloseButton = true,
            MaxWidth = MaxWidth.Medium,
            FullWidth = true
        };
        var parameters = new DialogParameters
        {
            { "GroupId", prediction.GroupId },
            { "MatchId", prediction.MatchId }
        };
        var dialog = DialogService.Show<WatchPredictions>($"{Localizer["Watch"]} {Localizer["Predictions"]}", parameters, options);

        await dialog.Result;
    }

    private async Task CheckUserEnabledAsync()
    {
        var responseHttp = await Repository.GetAsync<UserGroup>($"api/userGroups/{GroupId}/{username}");

        if (responseHttp.Error)
        {
            if (responseHttp.HttpResponseMessage.StatusCode != System.Net.HttpStatusCode.NotFound)
            {
                var messageError = await responseHttp.GetErrorMessageAsync();
                Snackbar.Add(messageError, Severity.Error);
            }
        }
        var userGroup = responseHttp.Response;
        userEnabledForGroup = userGroup!.IsActive;
    }
}
using SoccerSystem.Shared.Entites;
using SoccerSystem.Shared.Resources;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using SoccerSystem.Frontend.Repositories;
using CurrieTechnologies.Razor.SweetAlert2;

namespace SoccerSystem.Frontend.Pages.Countries;

public partial class CountryCreate
{
    private CountryForm? countryForm;
    private Country country = new();

    [Inject] private IRepository Repository { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
    [Inject] private IStringLocalizer<Literals> Localizer { get; set; } = null!;

    private async Task CreateAsync()
    {
        var responseHttp = await Repository.PostAsync("/api/countries", country);
        if (responseHttp.Error)
        {
            var message = await responseHttp.GetErrorMessageAsync();
            await SweetAlertService.FireAsync(Localizer["Error"],
                                              Localizer[message!],
                                              SweetAlertIcon.Error);
            return;
        }

        Return();
        var toast = SweetAlertService.Mixin(new SweetAlertOptions
        {
            Toast = true,
            Position = SweetAlertPosition.BottomEnd,
            ShowConfirmButton = true,
            Timer = 3000 // son 3 segundos
        });
        await toast.FireAsync(icon: SweetAlertIcon.Success,
                              message: Localizer["RecordCreatedOk"]);
    }

    private void Return()
    {
        countryForm!.FormPostedSuccessfully = true;
        NavigationManager.NavigateTo("/countries");
    }
}
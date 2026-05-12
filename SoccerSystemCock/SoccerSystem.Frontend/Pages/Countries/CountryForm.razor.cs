using SoccerSystem.Shared.Entites;
using SoccerSystem.Shared.Resources;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;

namespace SoccerSystem.Frontend.Pages.Countries;

public partial class CountryForm
{
    private EditContext editContext = null!;

    protected override void OnInitialized()
    {
        editContext = new(Country);
    }

    [EditorRequired, Parameter] public Country Country { get; set; } = null!;

    //EventCallback se utiliza pra que nose llame de este formulario sino de otro
    //componente padre, es decir, el formulario no se encarga de hacer el submit sino que se lo delega a otro componente
    [EditorRequired, Parameter] public EventCallback OnValidSubmit { get; set; }

    [EditorRequired, Parameter] public EventCallback ReturnAction { get; set; }

    // FormPostedSuccessfully para evitar que pierda lso cambios controlar sie l usuario sicliquea a otra parte avisarle que perder los cambios
    public bool FormPostedSuccessfully { get; set; } = false;

    [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
    [Inject] private IStringLocalizer<Literals> Localizer { get; set; } = null!;

    private async Task OnBeforeInternalNavigation(LocationChangingContext context)
    {
        var formWasEdited = editContext.IsModified();

        if (!formWasEdited || FormPostedSuccessfully)
        {
            return;
        }

        var result = await SweetAlertService.FireAsync(new SweetAlertOptions
        {
            Title = Localizer["Confirmation"],
            Text = Localizer["LeaveAndLoseChanges"],
            Icon = SweetAlertIcon.Warning,
            ShowCancelButton = true
        });

        var confirm = !string.IsNullOrEmpty(result.Value);
        if (confirm)
        {
            return;
        }

        context.PreventNavigation();
    }
}
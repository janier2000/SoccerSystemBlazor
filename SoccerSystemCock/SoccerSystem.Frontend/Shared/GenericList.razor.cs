using Microsoft.AspNetCore.Components;

namespace SoccerSystem.Frontend.Shared;

public partial class GenericList<Titem>
{
    // RenderFragment es un fragmento de código que se puede renderizar, es decir, es un bloque de código que se puede mostrar en la pantalla. Es una forma de definir una plantilla de renderizado que se puede reutilizar en diferentes partes de la aplicación. agregar as razor
    [Parameter] public RenderFragment? Loading { get; set; }

    [Parameter] public RenderFragment? NoRecords { get; set; }

    //EditorRequired es que es obligatorio, si no se le asigna un valor, el compilador lanzará un error
    [EditorRequired, Parameter] public RenderFragment Body { get; set; } = null!;

    [EditorRequired, Parameter] public List<Titem> MyList { get; set; } = null!;
}
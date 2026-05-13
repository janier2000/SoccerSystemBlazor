using SoccerSystem.Shared.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoccerSystem.Shared.Entites;

public class Country
{
    public int Id { get; set; }

    //[Literals]
    //Display va reemplasar el nombre del campo por el valor que se le asigna, en este caso "Team" y se obtiene del recurso [Literals], esto es para que se pueda mostrar en diferentes idiomas dependiendo de la cultura del usuario
    [Display(Name = "Country", ResourceType = typeof(Literals))]
    [MaxLength(100, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public string Name { get; set; } = null!;

    public ICollection<Team>? Teams { get; set; }

    // nsoe apmea en la bd es de lectura, no se guarda en la bd, es solo para mostrar el numero de equipos que tiene cada pais
    public int TeamsCount => Teams == null ? 0 : Teams.Count;
}
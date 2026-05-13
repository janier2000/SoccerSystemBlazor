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

    [MaxLength(100)]
    [Required]
    public string Name { get; set; } = null!;

    public ICollection<Team>? Teams { get; set; }

    // nsoe apmea en la bd es de lectura, no se guarda en la bd, es solo para mostrar el numero de equipos que tiene cada pais
    public int TeamsCount => Teams == null ? 0 : Teams.Count;
}
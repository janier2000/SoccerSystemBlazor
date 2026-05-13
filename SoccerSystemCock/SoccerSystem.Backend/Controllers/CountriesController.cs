using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoccerSystem.Backend.Data;
using SoccerSystem.Backend.UnitsOfWork.Interfaces;
using SoccerSystem.Shared.Entites;

namespace SoccerSystem.Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CountriesController : GenericController<Country>
{
    public CountriesController(IGenericUnitOfWork<Country> unit) : base(unit)
    {
    }
}
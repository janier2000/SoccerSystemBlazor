using Microsoft.AspNetCore.Mvc;
using SoccerSystem.Shared.Entites;
using SoccerSystem.Backend.UnitsOfWork.Interfaces;

namespace SoccerSystem.Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TeamsController : GenericController<Team>
{
    public TeamsController(IGenericUnitOfWork<Team> unitOfWork) : base(unitOfWork)
    {
    }
}
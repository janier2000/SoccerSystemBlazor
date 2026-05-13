using SoccerSystem.Shared.DTOs;
using SoccerSystem.Shared.Entites;
using SoccerSystem.Shared.Responses;
using SoccerSystem.Backend.UnitsOfWork.Interfaces;
using SoccerSystem.Backend.Repositories.Interfaces;

namespace SoccerSystem.Backend.UnitsOfWork.Implementations;

public class TeamsUnitOfWork : GenericUnitOfWork<Team>, ITeamsUnitOfWork
{
    private readonly ITeamsRepository _teamsRepository;

    public TeamsUnitOfWork(IGenericRepository<Team> repository, ITeamsRepository teamsRepository) : base(repository)
    {
        _teamsRepository = teamsRepository;
    }

    public async Task<ActionResponse<Team>> AddAsync(TeamDTO teamDTO) => await _teamsRepository.AddAsync(teamDTO);

    public async Task<IEnumerable<Team>> GetComboAsync(int countryId) => await _teamsRepository.GetComboAsync(countryId);

    public async Task<ActionResponse<Team>> UpdateAsync(TeamDTO teamDTO) => await _teamsRepository.UpdateAsync(teamDTO);

    public override async Task<ActionResponse<Team>> GetAsync(int id) => await _teamsRepository.GetAsync(id);

    public override async Task<ActionResponse<IEnumerable<Team>>> GetAsync() => await _teamsRepository.GetAsync();
}
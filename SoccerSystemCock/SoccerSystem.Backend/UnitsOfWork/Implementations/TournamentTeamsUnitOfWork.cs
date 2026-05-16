using SoccerSystem.Backend.Repositories.Interfaces;
using SoccerSystem.Backend.UnitsOfWork.Interfaces;
using SoccerSystem.Shared.DTOs;
using SoccerSystem.Shared.Entites;
using SoccerSystem.Shared.Responses;

namespace SoccerSystem.Backend.UnitsOfWork.Implementations;

public class TournamentTeamsUnitOfWork : GenericUnitOfWork<TournamentTeam>, ITournamentTeamsUnitOfWork
{
    private readonly ITournamentTeamsRepository _tournamentTeamsRepository;

    public TournamentTeamsUnitOfWork(IGenericRepository<TournamentTeam> repository, ITournamentTeamsRepository tournamentTeamsRepository) : base(repository)
    {
        _tournamentTeamsRepository = tournamentTeamsRepository;
    }

    public async Task<ActionResponse<TournamentTeam>> AddAsync(TournamentTeamDTO tournamentTeamDTO) => await _tournamentTeamsRepository.AddAsync(tournamentTeamDTO);

    public async Task<IEnumerable<TournamentTeam>> GetComboAsync(int tournamentId) => await _tournamentTeamsRepository.GetComboAsync(tournamentId);

    public async Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination) => await _tournamentTeamsRepository.GetTotalRecordsAsync(pagination);

    public override async Task<ActionResponse<IEnumerable<TournamentTeam>>> GetAsync(PaginationDTO pagination) => await _tournamentTeamsRepository.GetAsync(pagination);
}
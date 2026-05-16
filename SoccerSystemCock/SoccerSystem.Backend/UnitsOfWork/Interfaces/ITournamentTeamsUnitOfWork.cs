using SoccerSystem.Shared.DTOs;
using SoccerSystem.Shared.Entites;
using SoccerSystem.Shared.Responses;

namespace SoccerSystem.Backend.UnitsOfWork.Interfaces;

public interface ITournamentTeamsUnitOfWork
{
    Task<IEnumerable<TournamentTeam>> GetComboAsync(int tournamentId);

    Task<ActionResponse<TournamentTeam>> AddAsync(TournamentTeamDTO tournamentTeamDTO);

    Task<ActionResponse<IEnumerable<TournamentTeam>>> GetAsync(PaginationDTO pagination);

    Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination);
}
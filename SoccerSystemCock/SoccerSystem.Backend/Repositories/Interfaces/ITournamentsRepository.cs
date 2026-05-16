using SoccerSystem.Shared.DTOs;
using SoccerSystem.Shared.Entites;
using SoccerSystem.Shared.Responses;

namespace SoccerSystem.Backend.Repositories.Interfaces;

public interface ITournamentsRepository
{
    Task<IEnumerable<Tournament>> GetComboAsync();

    Task<ActionResponse<Tournament>> AddAsync(TournamentDTO tournamentDTO);

    Task<ActionResponse<Tournament>> UpdateAsync(TournamentDTO tournamentDTO);

    Task<ActionResponse<Tournament>> GetAsync(int id);

    Task<ActionResponse<IEnumerable<Tournament>>> GetAsync(PaginationDTO pagination);

    Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination);
}
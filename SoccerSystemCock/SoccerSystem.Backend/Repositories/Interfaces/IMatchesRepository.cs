using SoccerSystem.Shared.DTOs;
using SoccerSystem.Shared.Entites;
using SoccerSystem.Shared.Responses;

namespace SoccerSystem.Backend.Repositories.Interfaces;

public interface IMatchesRepository
{
    Task<ActionResponse<Match>> AddAsync(MatchDTO matchDTO);

    Task<ActionResponse<Match>> UpdateAsync(MatchDTO matchDTO);

    Task<ActionResponse<Match>> GetAsync(int id);

    Task<ActionResponse<IEnumerable<Match>>> GetAsync(PaginationDTO pagination);

    Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination);
}
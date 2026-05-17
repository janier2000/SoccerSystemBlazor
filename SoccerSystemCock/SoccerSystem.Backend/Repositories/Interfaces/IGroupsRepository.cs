using SoccerSystem.Shared.DTOs;
using SoccerSystem.Shared.Entites;
using SoccerSystem.Shared.Responses;

namespace SoccerSystem.Backend.Repositories.Interfaces;

public interface IGroupsRepository
{
    Task<ActionResponse<Group>> AddAsync(GroupDTO groupDTO);

    Task<ActionResponse<Group>> UpdateAsync(GroupDTO groupDTO);

    Task<ActionResponse<Group>> GetAsync(int id);

    Task<ActionResponse<IEnumerable<Group>>> GetAsync(PaginationDTO pagination);

    Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination);

    Task<ActionResponse<Group>> GetAsync(string code);
}
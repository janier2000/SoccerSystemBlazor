using SoccerSystem.Shared.DTOs;
using SoccerSystem.Shared.Entites;
using SoccerSystem.Shared.Responses;

namespace SoccerSystem.Backend.Repositories.Interfaces;

public interface IUserGroupsRepository
{
    Task<ActionResponse<UserGroup>> AddAsync(UserGroupDTO userGroupDTO);

    Task<ActionResponse<UserGroup>> UpdateAsync(UserGroupDTO userGroupDTO);

    Task<ActionResponse<UserGroup>> GetAsync(int id);

    Task<ActionResponse<IEnumerable<UserGroup>>> GetAsync(PaginationDTO pagination);

    Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination);
}
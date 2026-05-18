using SoccerSystem.Shared.DTOs;
using SoccerSystem.Shared.Entites;
using SoccerSystem.Shared.Responses;

namespace SoccerSystem.Backend.UnitsOfWork.Interfaces;

public interface IUserGroupsUnitOfWork
{
    Task<ActionResponse<UserGroup>> AddAsync(UserGroupDTO userGroupDTO);

    Task<ActionResponse<UserGroup>> UpdateAsync(UserGroupDTO userGroupDTO);

    Task<ActionResponse<UserGroup>> GetAsync(int id);

    Task<ActionResponse<IEnumerable<UserGroup>>> GetAsync(PaginationDTO pagination);

    Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination);

    Task<ActionResponse<UserGroup>> JoinAsync(JoinGroupDTO joinGroupDTO);

    Task<ActionResponse<UserGroup>> GetAsync(int groupId, string email);
}
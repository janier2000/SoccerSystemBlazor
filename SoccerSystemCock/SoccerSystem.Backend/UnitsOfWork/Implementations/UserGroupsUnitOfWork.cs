using SoccerSystem.Backend.Repositories.Interfaces;
using SoccerSystem.Backend.UnitsOfWork.Interfaces;
using SoccerSystem.Shared.DTOs;
using SoccerSystem.Shared.Entites;
using SoccerSystem.Shared.Responses;

namespace SoccerSystem.Backend.UnitsOfWork.Implementations;

public class UserGroupsUnitOfWork : GenericUnitOfWork<UserGroup>, IUserGroupsUnitOfWork
{
    private readonly IUserGroupsRepository _userGroupsRepository;

    public UserGroupsUnitOfWork(IGenericRepository<UserGroup> repository, IUserGroupsRepository userGroupsRepository) : base(repository)
    {
        _userGroupsRepository = userGroupsRepository;
    }

    public override async Task<ActionResponse<IEnumerable<UserGroup>>> GetAsync(PaginationDTO pagination) => await _userGroupsRepository.GetAsync(pagination);

    public override async Task<ActionResponse<UserGroup>> GetAsync(int id) => await _userGroupsRepository.GetAsync(id);

    public async Task<ActionResponse<UserGroup>> AddAsync(UserGroupDTO userGroupDTO) => await _userGroupsRepository.AddAsync(userGroupDTO);

    public async Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination) => await _userGroupsRepository.GetTotalRecordsAsync(pagination);

    public async Task<ActionResponse<UserGroup>> UpdateAsync(UserGroupDTO userGroupDTO) => await _userGroupsRepository.UpdateAsync(userGroupDTO);

    public async Task<ActionResponse<UserGroup>> JoinAsync(JoinGroupDTO joinGroupDTO) => await _userGroupsRepository.JoinAsync(joinGroupDTO);
}
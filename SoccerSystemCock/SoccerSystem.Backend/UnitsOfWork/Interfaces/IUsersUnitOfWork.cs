using SoccerSystem.Shared.Entites;
using Microsoft.AspNetCore.Identity;

namespace SoccerSystem.Backend.UnitsOfWork.Interfaces;

public interface IUsersUnitOfWork
{
    Task<User> GetUserAsync(string email);

    Task<IdentityResult> AddUserAsync(User user, string password);

    Task CheckRoleAsync(string roleName);

    Task AddUserToRoleAsync(User user, string roleName);

    Task<bool> IsUserInRoleAsync(User user, string roleName);
}
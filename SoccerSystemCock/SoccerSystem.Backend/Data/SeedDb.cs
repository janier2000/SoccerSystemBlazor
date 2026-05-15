using Microsoft.EntityFrameworkCore;
using SoccerSystem.Backend.Helpers;
using SoccerSystem.Backend.UnitsOfWork.Implementations;
using SoccerSystem.Backend.UnitsOfWork.Interfaces;
using SoccerSystem.Shared.Entites;
using SoccerSystem.Shared.Enums;

namespace SoccerSystem.Backend.Data;

public class SeedDb
{
    private readonly DataContext _context;
    private readonly IFileStorage _fileStorage;
    private readonly IUsersUnitOfWork _usersUnitOfWork;

    public SeedDb(DataContext context, IFileStorage fileStorage, IUsersUnitOfWork usersUnitOfWork)
    {
        _context = context;
        _fileStorage = fileStorage;
        _usersUnitOfWork = usersUnitOfWork;
    }

    public async Task SeedAsync()
    {
        await _context.Database.EnsureCreatedAsync();
        await CheckCountriesAsync();
        await CheckTeamsAsync();
        await CheckRolesAsync();
        await CheckUserAsync("Janier", "Machado", "jomr@yopmail.com", "322 311 4620", UserType.Admin);
    }

    private async Task CheckRolesAsync()
    {
        await _usersUnitOfWork.CheckRoleAsync("Admin");
        await _usersUnitOfWork.CheckRoleAsync(UserType.User.ToString());
    }

    private async Task<User> CheckUserAsync(string firstName, string lastName, string email, string phone, UserType userType)
    {
        var user = await _usersUnitOfWork.GetUserAsync(email);
        if (user == null)
        {
            var country = await _context.Countries.FirstOrDefaultAsync(x => x.Name == "Colombia");
            user = new User
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                UserName = email,
                PhoneNumber = phone,
                Country = country!,
                UserType = userType,
            };

            await _usersUnitOfWork.AddUserAsync(user, "123456");
            await _usersUnitOfWork.AddUserToRoleAsync(user, userType.ToString());
        }
        return user;
    }

    private async Task CheckCountriesAsync()
    {
        if (!_context.Countries.Any())
        {
            var countriesSQLScript = File.ReadAllText("Data\\Countries.sql");
            await _context.Database.ExecuteSqlRawAsync(countriesSQLScript);
        }
    }

    private async Task CheckTeamsAsync()
    {
        if (!_context.Teams.Any())
        {
            foreach (var country in _context.Countries)
            {
                var imagePath = string.Empty;
                var filePath = $"{Environment.CurrentDirectory}\\Images\\Flags\\{country.Name}.png";
                if (File.Exists(filePath))
                {
                    var fileBytes = File.ReadAllBytes(filePath);
                    //comentado tempralmente no funciono
                    //imagePath = await _fileStorage.SaveFileAsync(fileBytes, "jpg", "flags");
                    imagePath = $"\\Images\\Flags\\{country.Name}.png"; ;
                }
                _context.Teams.Add(new Team { Name = country.Name, Country = country!, Image = imagePath });

                //_context.Teams.Add(new Team { Name = country.Name, Country = country! });
                //if (country.Name == "Colombia")
                //{
                //    _context.Teams.Add(new Team { Name = "Medellín", Country = country! });
                //    _context.Teams.Add(new Team { Name = "Nacional", Country = country! });
                //    _context.Teams.Add(new Team { Name = "Millonarios", Country = country! });
                //    _context.Teams.Add(new Team { Name = "Junior", Country = country! });
                //    _context.Teams.Add(new Team { Name = "Real cartagena", Country = country! });
                //}
            }

            await _context.SaveChangesAsync();
        }
    }
}
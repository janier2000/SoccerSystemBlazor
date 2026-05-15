using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace SoccerSystem.Frontend.AuthenticationProviders;

public class AuthenticationProviderTest : AuthenticationStateProvider
{
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        //aca se prueba como anomnimo y como administrador
        await Task.Delay(3000);
        var anonimous = new ClaimsIdentity();
        var user = new ClaimsIdentity(authenticationType: "test");

        var admin = new ClaimsIdentity(new List<Claim>
        {
            new Claim("FirstName", "janier"),
            new Claim("LastName", "machado"),
            new Claim(ClaimTypes.Name, "jomr@yopmail.com"),
            new Claim(ClaimTypes.Role, "Admin")
        },
        authenticationType: "test");

        return await Task.FromResult(new AuthenticationState(new ClaimsPrincipal(admin)));
        //return await Task.FromResult(new AuthenticationState(new ClaimsPrincipal(user)));
    }
}
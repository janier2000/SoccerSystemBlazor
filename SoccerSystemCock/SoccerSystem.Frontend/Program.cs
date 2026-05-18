using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using SoccerSystem.Frontend;
using SoccerSystem.Frontend.AuthenticationProviders;
using SoccerSystem.Frontend.Helpers;
using SoccerSystem.Frontend.Repositories;
using SoccerSystem.Frontend.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddSingleton(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7140") });
builder.Services.AddScoped<IRepository, Repository>();

// este es para  e nuge del idioma [Microsoft.Extensions.Localization]
builder.Services.AddLocalization();

// este es para  el nuge de mensaje  [CurrieTechnologies.Razor.SweetAlert2]
builder.Services.AddSweetAlert2();

//este es para  el nuge  [MudBlazor]
builder.Services.AddMudServices();

// este es para  el nuge de autorizacion [Microsoft.AspNetCore.Components.Authorization]
// el AuthenticationProviderTest es un proveedor de autenticacion de prueba que devuelve un usuario anonimo osea para ejecutar manualment
builder.Services.AddAuthorizationCore();

// todo esto es apra el token
//para pruebas
//builder.Services.AddScoped<AuthenticationStateProvider, AuthenticationProviderTest>();
builder.Services.AddScoped<AuthenticationProviderJWT>();
builder.Services.AddScoped<AuthenticationStateProvider, AuthenticationProviderJWT>(x => x.GetRequiredService<AuthenticationProviderJWT>());
builder.Services.AddScoped<ILoginService, AuthenticationProviderJWT>(x => x.GetRequiredService<AuthenticationProviderJWT>());

//para utilizar los javascrip propios de la aplicacion
builder.Services.AddScoped<IClipboardService, ClipboardService>();

// Register language service and localStorage service for managing language preferences
builder.Services.AddScoped<LanguageService>();
builder.Services.AddScoped<LocalStorageService>();

// Build the application
var host = builder.Build();

// Retrieve the language service to set the initial language based on user preferences or browser language
var languageService = host.Services.GetRequiredService<LanguageService>();

// Initialize the language preference (from localStorage or browser)
await languageService.InitializeLanguageAsync(); // This will set the initial culture based on local storage or browser

// Run the application
await host.RunAsync();
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SoccerSystem.Frontend;
using SoccerSystem.Frontend.Repositories;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7140") });
builder.Services.AddScoped<IRepository, Repository>();

// este es para  e nuge del idioma [Microsoft.Extensions.Localization]
builder.Services.AddLocalization();

// este es para  el nuge de mensaje  [CurrieTechnologies.Razor.SweetAlert2]
builder.Services.AddSweetAlert2();

await builder.Build().RunAsync();
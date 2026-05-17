using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SoccerSystem.Backend.Data;
using SoccerSystem.Backend.Helpers;
using SoccerSystem.Backend.Repositories.Implementations;
using SoccerSystem.Backend.Repositories.Interfaces;
using SoccerSystem.Backend.UnitsOfWork.Implementations;
using SoccerSystem.Backend.UnitsOfWork.Interfaces;
using SoccerSystem.Shared.Entites;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
// esto sirve para evitar el error de referencia cíclica al serializar objetos que tienen relaciones entre sí, como entidades relacionadas en una base de datos. Al establecer ReferenceHandler en IgnoreCycles, se evita que el serializador intente seguir las referencias circulares, lo que puede causar un desbordamiento de pila o un error de referencia cíclica. En su lugar, el serializador omitirá las propiedades que causan la referencia circular, lo que permite que la serialización se complete sin problemas.
// ejemplo public Country? Country { get; se3ht; } en team
// public ICollection<Team>? Teams { get; set; } en country

builder.Services.AddControllers()
                .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// aca permite poder agregar tolen manualmente en swagger, para que se pueda probar la autenticacion con JWT desde la interfaz de swagger, esto es muy util para probar los endpoints protegidos por autenticacion sin necesidad de usar herramientas externas como Postman o Insomnia. Al agregar esta configuracion, se habilita un campo en la interfaz de Swagger donde se puede ingresar el token JWT y luego usarlo para autenticar las solicitudes a los endpoints protegidos.
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Orders Backend", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme. <br /> <br />
                      Enter 'Bearer' [space] and then your token in the text input below.<br /> <br />
                      Example: 'Bearer 12345abcdef'<br /> <br />",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
      {
        {
          new OpenApiSecurityScheme
          {
            Reference = new OpenApiReference
              {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
              },
              Scheme = "oauth2",
              Name = "Bearer",
              In = ParameterLocation.Header,
            },
            new List<string>()
          }
        });
});

builder.Services.AddDbContext<DataContext>(x => x.UseSqlServer("name=LocalConnection"));

builder.Services.AddTransient<SeedDb>();// para agregar datos ala base de datos
builder.Services.AddScoped<IFileStorage, FileStorage>();// para agregar imagenes a azure storage

builder.Services.AddScoped<ICountriesRepository, CountriesRepository>();
builder.Services.AddScoped<ICountriesUnitOfWork, CountriesUnitOfWork>();

builder.Services.AddScoped(typeof(IGenericUnitOfWork<>), typeof(GenericUnitOfWork<>));
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

builder.Services.AddScoped<IGroupsRepository, GroupsRepository>();
builder.Services.AddScoped<IGroupsUnitOfWork, GroupsUnitOfWork>();

builder.Services.AddScoped<IMatchesRepository, MatchesRepository>();
builder.Services.AddScoped<IMatchesUnitOfWork, MatchesUnitOfWork>();

builder.Services.AddScoped<ITeamsRepository, TeamsRepository>();
builder.Services.AddScoped<ITeamsUnitOfWork, TeamsUnitOfWork>();

builder.Services.AddScoped<ITournamentsRepository, TournamentsRepository>();
builder.Services.AddScoped<ITournamentsUnitOfWork, TournamentsUnitOfWork>();

builder.Services.AddScoped<ITournamentTeamsRepository, TournamentTeamsRepository>();
builder.Services.AddScoped<ITournamentTeamsUnitOfWork, TournamentTeamsUnitOfWork>();

builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.AddScoped<IUsersUnitOfWork, UsersUnitOfWork>();

// esto apra reglas del usuario, como que el email sea unico, o que la contraseña no tenga requisitos de seguridad, etc. Esto se hace para facilitar las pruebas y el desarrollo, pero en un entorno de producción se recomienda establecer reglas de seguridad más estrictas para proteger la información de los usuarios.
// este es debil cuando se pasa a produccion colocarle mas cosas
builder.Services.AddIdentity<User, IdentityRole>(x =>
{
    x.Tokens.AuthenticatorTokenProvider = TokenOptions.DefaultAuthenticatorProvider;//opcional
    x.SignIn.RequireConfirmedEmail = true;//requiere email confirmado para iniciar sesion
    x.User.RequireUniqueEmail = true;
    x.Password.RequireDigit = false;
    x.Password.RequiredUniqueChars = 0;
    x.Password.RequireLowercase = false;
    x.Password.RequireNonAlphanumeric = false;
    x.Password.RequireUppercase = false;
    x.Password.RequiredLength = 4;
    x.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5); //aqui por 5 minutos se bloquea
    x.Lockout.MaxFailedAccessAttempts = 3;//caundo el usuarion ingrese la contraseña 3 se bloquea
    x.Lockout.AllowedForNewUsers = true;//opcional
})
    .AddEntityFrameworkStores<DataContext>()
    .AddDefaultTokenProviders();

//----------------------

// esto es para la autenticacion con JWT, se configura el esquema de autenticacion y las opciones de validacion del token. En este caso, se establece que no se validará el emisor ni el público del token, pero sí se validará la vida útil del token y la clave de firma. La clave de firma se obtiene de la configuración de la aplicación, específicamente de la clave "jwtKey". Además, se establece un tiempo de tolerancia (ClockSkew) de cero para evitar problemas con la expiración del token.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(x => x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["jwtKey"]!)),
        ClockSkew = TimeSpan.Zero
    });

builder.Services.AddScoped<IMailHelper, MailHelper>();//este sirve para la confirmacion del correo

var app = builder.Build();

SeedData(app);// para agregar datos ala base de datos
void SeedData(WebApplication app)// para agregar datos ala base de datos
{
    var scopedFactory = app.Services.GetService<IServiceScopeFactory>();
    using var scope = scopedFactory!.CreateScope();
    var service = scope.ServiceProvider.GetService<SeedDb>();
    service!.SeedAsync().Wait();
}

if (app.Environment.IsDevelopment())// Configure the HTTP request pipeline.
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//Agregamos estas líneas al Program del proyecto Backend para habilitar su consumo:
//apliaccion usa cors acepta cualquier metodo, header, peticion esto
//ayuda cualquier peticion
app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true)
    .AllowCredentials());

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
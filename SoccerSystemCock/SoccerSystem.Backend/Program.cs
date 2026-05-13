using Microsoft.EntityFrameworkCore;
using SoccerSystem.Backend.Data;
using SoccerSystem.Backend.Repositories.Implementations;
using SoccerSystem.Backend.Repositories.Interfaces;
using SoccerSystem.Backend.UnitsOfWork.Implementations;
using SoccerSystem.Backend.UnitsOfWork.Interfaces;
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
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DataContext>(x => x.UseSqlServer("name=LocalConnection"));

// para agregar datos ala base de datos
builder.Services.AddTransient<SeedDb>();

builder.Services.AddScoped(typeof(IGenericUnitOfWork<>), typeof(GenericUnitOfWork<>));
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<ICountriesRepository, CountriesRepository>();
builder.Services.AddScoped<ICountriesUnitOfWork, CountriesUnitOfWork>();

var app = builder.Build();

// para agregar datos ala base de datos
SeedData(app);
// para agregar datos ala base de datos
void SeedData(WebApplication app)
{
    var scopedFactory = app.Services.GetService<IServiceScopeFactory>();
    using var scope = scopedFactory!.CreateScope();
    var service = scope.ServiceProvider.GetService<SeedDb>();
    service!.SeedAsync().Wait();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

//Agregamos estas líneas al Program del proyecto Backend para habilitar su consumo:
//apliaccion usa cors acepta cualquier metodo, header, peticion esto
//ayuda cualquier peticion
app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true)
    .AllowCredentials());

app.Run();
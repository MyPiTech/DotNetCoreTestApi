using TestApi.Hubs;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Test.Data;
using TestApi.Dtos;
using TestApi.Services;

var builder = WebApplication.CreateBuilder(args);
var AllowSpecificOrigins = "_allowSpecificOrigins";

builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull);

// Add services to the container.

builder.Services.AddMemoryCache();
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: AllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins(
                                  "http://localhost:52518", 
                                  "https://mango-ground-0c8d1f710.4.azurestaticapps.net", 
                                  "https://mypitech.com",
							      "https://localhost:7232"
                              )
							.AllowAnyHeader()
                            .AllowAnyMethod();
                      });
});
builder.Services.AddDbContext<MSTestDataContext>(
        o => o.UseSqlServer(
                builder.Configuration.GetConnectionString("MSTestConnectionString"), 
                o => o.EnableRetryOnFailure()
            )
        );
builder.Services.AddTransient<IService<User, CreateUserDto, UserDto>, UserService>();
builder.Services.AddTransient<IService<Event, CreateEventDto, EventDto>, EventService>();
builder.Services.AddTransient<IService<Event, CreateUserEventDto, EventDto>, UserEventService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Demo API",
        Description = "A demo .NET Core Web API for managing users and user events." +
        "<br> Users and events represent a one to many relationship, but otherwise they were an arbitrary choice." +
        "<br><br> This is not production-ready." +
        "<br> Please see the source repository for more information." +
        "<br><br><a target=”_blank” href=\"https://github.com/MyPiTech/DotNetCoreTestApi\">Source Repository</a><br>",
        Contact = new OpenApiContact
        {
            Name = "Shawn Wheeler",
            Email = "swheeler@mypitech.com",
            Url = new Uri("http://mypitech.com")
        }
    });

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

builder.Services.AddSignalR();

var app = builder.Build();
app.UseCors(AllowSpecificOrigins);
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
});


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapHub<ConsoleHub>("/console");

app.Run();

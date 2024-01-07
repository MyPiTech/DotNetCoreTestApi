using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Test.Data;
using TestApi.Dtos;
using TestApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull);

// Add services to the container.
builder.Services.AddDbContext<MSTestDataContext>(
        o => o.UseSqlServer(
                builder.Configuration.GetConnectionString("MSTestConnectionString"), 
                o => o.EnableRetryOnFailure()
            )
        );
builder.Services.AddTransient<IService<User, CreateUserDto, UserDto>, UserService>();
builder.Services.AddTransient<IService<Event, CreateEventDto, EventDto>, EventService>();
builder.Services.AddTransient<IService<Event, CreateUserEventDto, EventDto>, UserEventService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Test API",
        Description = "A test ASP.NET Core Web API for managing users and user events." +
        "<br><br> This is not production-ready." +
        "<br> Please see the source repository for more information." +
        " <br><br><a target=”_blank” href=\"https://github.com/MyPiTech/DotNetCoreTestApi\">Source Repository</a><br>",
        Contact = new OpenApiContact
        {
            Name = "Shawn Wheeler",
            Email = "swheeler1974@gmail.com",
            Url = new Uri("https://www.linkedin.com/in/shawn-wheeler-5850a956/")
            
        }
    });

    // using System.Reflection;
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
});


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

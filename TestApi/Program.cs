using Microsoft.EntityFrameworkCore;
using Test.Data;
using TestApi.Dtos;
using TestApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull);

// Add services to the container.
builder.Services.AddDbContext<MSTestDataContext>(o => o.UseSqlServer(builder.Configuration.GetConnectionString("MSTestConnectionString")));
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IService<CreateEventDto, EventDto>, EventService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

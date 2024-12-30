using Microsoft.EntityFrameworkCore;
using R3M.Finantial.Backend.Context;

var builder = WebApplication.CreateBuilder();

builder.Services.AddControllers();
builder.Services.AddDbContext<FinantialContext>(options =>
{
    options
        .UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
        .UseSnakeCaseNamingConvention();
});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


app.UseCors();
app.MapControllers();

app.UseSwagger();
app.UseSwaggerUI();
    
await app.RunAsync();

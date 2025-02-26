using Backend.Models;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

if (builder.Environment.IsDevelopment())
{
    Env.Load();
    // use sqlite
    builder.Services.AddDbContext<DataContext>(option => option.UseSqlite(Environment.GetEnvironmentVariable("DB_PATH")));
}
else
{
    // SQL Server 
    // builder.Services.AddDbContext<DataContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("Database")));

    // MYSQL
    // builder.Services.AddDbContext<DataContext>(option => option.UseMySQL(builder.Configuration.GetConnectionString("Database")));
}

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

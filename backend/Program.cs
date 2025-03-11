using System.Text.Json;
using Backend.Models;
using Backend.Profiles;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    });
builder.Services.AddDbContext<DataContext>(options =>
{
    if (builder.Environment.IsDevelopment())
    {
        // use sqlite
        options.UseSqlite("./test.db");
    }
    else
    {
        var Env = Environment.GetEnvironmentVariables();

        // // MYSQL
        // string connectionString = $"Server=mysql_db;Port=3306;Database={Env["DB"]};User ID={Env["USER"]};Password={Env["PW"]}";
        // options.UseMySql(connectionString, new MySqlServerVersion(new Version(9, 2, 0)));

        // PostgreSQL
        string connectionString = $"Host=postgresql_db;Port={(Env["Port"] == null ? "5432" : Env["Port"])};Database={Env["DB"]};Username={Env["USER"]};Password={Env["PW"]}";
        options.UseNpgsql(connectionString);
    }
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(option =>
{
    //未登入時會自動導到這個網址
    option.LoginPath = new PathString("/api/v1/login/401");
    // 沒權限會自動導到這個網址
    option.AccessDeniedPath = new PathString("/api/v1/login/403");
    // cookie失效時間
    option.ExpireTimeSpan = TimeSpan.FromHours(1);
});
builder.Services.AddAutoMapper(typeof(MappingProfile));
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

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();
    if (app.Environment.IsDevelopment())
        dbContext.Database.Migrate();  // Apply migrations and create the database

    if (dbContext.Users.Count() == 0)
    {
        // run seed
    }
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

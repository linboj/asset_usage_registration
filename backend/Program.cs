using System.Text.Json;
using backend.Helpers;
using backend.Models;
using backend.Profiles;
using backend.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Set JSON serializer options to use camelCase naming policy
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
    });

builder.Services.AddDbContext<DataContext>(options =>
{
    if (builder.Environment.IsDevelopment())
    {
        // Use SQLite database in development environment
        options.UseSqlite("Data Source=test.sqlite");
    }
    else
    {
        var Env = Environment.GetEnvironmentVariables();

        // // Use MYSQL database in non-development environments
        // string connectionString = $"Server=mysql_db;Port=3306;Database={Env["DB"]};User ID={Env["USER"]};Password={Env["PW"]}";
        // options.UseMySql(connectionString, new MySqlServerVersion(new Version(9, 2, 0)));

        // Use PostgreSQL database in non-development environments
        string connectionString = $"Host=postgresql_db;Port={(Env["Port"] == null ? "5432" : Env["Port"])};Database={Env["DB"]};Username={Env["USER"]};Password={Env["PW"]}";
        options.UseNpgsql(connectionString);
    }
});

// Configure authentication using cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(option =>
{
    // Redirect to this URL when not logged in
    option.LoginPath = new PathString("/api/v1/login/401");
    // Redirect to this URL when access is denied
    option.AccessDeniedPath = new PathString("/api/v1/login/403");
    // Set cookie expiration time
    option.ExpireTimeSpan = TimeSpan.FromHours(1);
});

// Add AutoMapper with the specified mapping profile
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Add HTTP context accessor
builder.Services.AddHttpContextAccessor();

// Register application services
builder.Services.AddScoped<UsageService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<AssetService>();
builder.Services.AddScoped<LoginService>();
builder.Services.AddScoped<RoleService>();

// Register seed helper
builder.Services.AddScoped<SeedHelper>();

// Configure Swagger for API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Backend API", Version = "v1" });
});

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
        await dbContext.Database.MigrateAsync();  // Apply migrations and create the database

    if (dbContext.Users.Count() == 0)
    {
        // Run seed data if the database is empty
        var seedHelper = scope.ServiceProvider.GetRequiredService<SeedHelper>();
        await seedHelper.SeedAsync();
    }
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

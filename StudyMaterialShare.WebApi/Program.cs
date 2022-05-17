using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StudyMaterialShare.Database;
using StudyMaterialShare.Database.Models;
using StudyMaterialShare.Database.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<StudyMaterialShareDbContext>(options =>
{
    IConfigurationRoot configuration = builder.Configuration;
    DbType dbType = configuration.GetValue<DbType>("DbType");

    switch (dbType)
    {
        case DbType.SQLServer:
            // Need Microsoft.EntityFrameworkCore.SqlServer package for this
            options.UseSqlServer(configuration.GetConnectionString("SqlServerConnection"));
            break;

        case DbType.SQLite:
            // Need Microsoft.EntityFrameworkCore.Sqlite package for this
            options.UseSqlite(configuration.GetConnectionString("SqliteConnection"));
            break;
    }

    // Use lazy loading (don't forget the virtual keyword on the navigational properties also)
    options.UseLazyLoadingProxies();
});

// Add services to the container.

builder.Services.AddTransient<StudyMaterialRepository>();
builder.Services.AddTransient<SubjectRepository>();
builder.Services.AddTransient<RatingRepository>();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 3;
    options.Password.RequiredUniqueChars = 0;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
})
.AddEntityFrameworkStores<StudyMaterialShareDbContext>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddControllers();
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

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

using (var serviceScope = app.Services.CreateScope())
{
    using var context = serviceScope.ServiceProvider
        .GetRequiredService<StudyMaterialShareDbContext>();
    using var userManager = serviceScope.ServiceProvider
        .GetRequiredService<UserManager<ApplicationUser>>();
    using var roleManager = serviceScope.ServiceProvider
        .GetRequiredService<RoleManager<IdentityRole>>();

    string appDataFolder = app.Configuration.GetValue<string>("App_Data");
    await DbInitializer.Initialize(context, userManager, roleManager, appDataFolder);
}

app.Run();

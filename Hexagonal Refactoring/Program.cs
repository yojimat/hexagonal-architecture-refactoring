using Hexagonal_Refactoring.Repositories.Contexts;
using Hexagonal_Refactoring.Util;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<EventContext>(opt =>
{
    opt.EnableSensitiveDataLogging();
    opt.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

builder.Services.AddRepositories();
builder.Services.AddServices();
builder.Services.AddUseCases();

builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();

    // Reset database
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<EventContext>();
    //context.Database.EnsureDeleted();
    context.Database.EnsureCreated();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
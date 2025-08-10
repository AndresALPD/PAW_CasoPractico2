using PAWCP2.Core.Manager;
using PAWCP2.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Usualmente dentro de builder.Services
builder.Services.AddScoped<IUserBusiness, BusinessUser>();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

// También registra el repositorio que BusinessUser requiere
builder.Services.AddScoped<IRepositoryUser, RepositoryUser>();

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

app.UseAuthorization();

app.MapControllers();

app.Run();

using PAWCP2.Repositories;
using PAWCP2.Core.Manager; // Asegúrate que aquí estén IUserBusiness y UserBusiness

var builder = WebApplication.CreateBuilder(args);

// Registrar servicios para inyección
builder.Services.AddControllersWithViews();

// Registro de repositorio
builder.Services.AddScoped<IRepositoryUser, RepositoryUser>();

// Registro del manager (servicio de negocio)
builder.Services.AddScoped<IUserBusiness, BusinessUser>();

var app = builder.Build();

// Middleware pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

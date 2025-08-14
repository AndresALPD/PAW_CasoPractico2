using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PAWCP2.Api.Services;
using PAWCP2.Core.Manager;
using PAWCP2.Core.Repositories.Implementations;
using PAWCP2.Core.Repositories.Interfaces;
using PAWCP2.Data;
using PAWCP2.Mvc.Service;
using PAWCP2.Repositories;

var builder = WebApplication.CreateBuilder(args);

var cs = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrWhiteSpace(cs))
    throw new InvalidOperationException("La cadena de conexión 'DefaultConnection' no está configurada en appsettings.json.");
builder.Services.AddDbContext<PAWCP2DbContext>(options =>
    options.UseSqlServer(cs));

// Registrar servicios
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IRepositoryUser, RepositoryUser>();
builder.Services.AddScoped<IUserBusiness, BusinessUser>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IUserRoleRepository, UserRoleRepository>();
builder.Services.AddScoped<IUserRoleManager, UserRoleManager>();


// Nuevo servicio HTTP para FoodItem
builder.Services.AddHttpClient<IFoodItemService, FoodItemService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7289/"); // Cambiar la ruta si es necesario
});

// Configuración de sesión
builder.Services.AddSession(options => {
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Configuración de autenticación
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options => {
        options.LoginPath = "/Auth/Login";
        options.AccessDeniedPath = "/Auth/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        options.SlidingExpiration = true;
        options.Cookie.Name = "AuthCookie"; // Nombre específico para la cookie
    });

// Configuración de autorización
builder.Services.AddAuthorization();

// Elimina los filtros globales para evitar conflictos
builder.Services.Configure<MvcOptions>(options => {
    options.Filters.Clear();
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// Middlewares en ORDEN CORRECTO
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}");

app.Run();
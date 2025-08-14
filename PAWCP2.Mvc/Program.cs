using PAWCP2.Mvc.Service;
using Microsoft.AspNetCore.Authentication.Cookies;
using PAWCP2.Data; // Asegúrate de tener esta referencia para el DbContext
using Microsoft.EntityFrameworkCore;
using PAWCP2.Core.Manager;
using PAWCP2.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PAWCP2.Core.Repositories;
using PAWCP2.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Registrar servicios
builder.Services.AddControllersWithViews();

// Configuración del DbContext (AGREGA ESTO)
builder.Services.AddDbContext<PAWCP2DbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registro de servicios de autenticación
builder.Services.AddScoped<IRepositoryUser, RepositoryUser>();
builder.Services.AddScoped<IUserBusiness, BusinessUser>();
builder.Services.AddScoped<IAuthService, AuthService>();

// Registro de servicios para FoodItems (VERIFICA LOS NAMESPACES)
builder.Services.AddScoped<IFoodItemRepository, FoodItemRepository>();
builder.Services.AddScoped<IBusinessFoodItem, BusinessFoodItem>();

// Configuración HttpClient para FoodItemService
builder.Services.AddHttpClient<IFoodItemService, FoodItemService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7289/");
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
        options.Cookie.Name = "AuthCookie";
    });

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
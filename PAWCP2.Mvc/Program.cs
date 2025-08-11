using Microsoft.AspNetCore.Authentication.Cookies;
using PAWCP2.Repositories;
using PAWCP2.Core.Manager;
using PAWCP2.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Registrar servicios
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IRepositoryUser, RepositoryUser>();
builder.Services.AddScoped<IUserBusiness, BusinessUser>();
builder.Services.AddScoped<IAuthService, AuthService>();

// Configuraci�n de sesi�n
builder.Services.AddSession(options => {
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Configuraci�n de autenticaci�n (CORREGIDO)
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options => {
        options.LoginPath = "/Auth/Login";
        options.AccessDeniedPath = "/Auth/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        options.SlidingExpiration = true;
        options.Cookie.Name = "AuthCookie"; // Nombre espec�fico para la cookie
    });

// Configuraci�n de autorizaci�n (IMPORTANTE)
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

// Elimina el middleware de redirecci�n personalizado (temporalmente para pruebas)
// app.Use(async (context, next) => {
//     var isAuthPage = context.Request.Path.StartsWithSegments("/Auth");
//     if (context.User.Identity.IsAuthenticated && isAuthPage)
//     {
//         context.Response.Redirect("/Home");
//         return;
//     }
//     await next();
// });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}");

app.Run();
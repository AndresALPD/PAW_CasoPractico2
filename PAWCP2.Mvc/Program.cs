
using PAWCP2.Repositories;
using PAWCP2.Core.Manager;
using PAWCP2.Mvc.Service;

var builder = WebApplication.CreateBuilder(args);

// Registrar servicios para inyecci�n
builder.Services.AddControllersWithViews();

// Registro de repositorio
builder.Services.AddScoped<IRepositoryUser, RepositoryUser>();

// Registro del manager (servicio de negocio)
builder.Services.AddScoped<IUserBusiness, BusinessUser>();
//nuevo
builder.Services.AddHttpClient<IFoodItemService, FoodItemService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7289/"); //cambiar la ruta si no les sirve
});

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

using Microsoft.AspNetCore.Mvc;
using PAWCP2.Api.Services;
using PAWCP2.Models;
using System.Threading.Tasks;

namespace PAWCP2.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // GET: /Auth/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Auth/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string username, string email)
        {
            if (!ModelState.IsValid)
            {
                return View(); // Retorna la vista con errores de validación
            }

            var user = await _authService.AuthenticateAsync(username, email);

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Credenciales inválidas o usuario inactivo.");
                return View();
            }

            // Guardar sesión (ejemplo básico)
            HttpContext.Session.SetString("UserId", user.UserId.ToString());
            HttpContext.Session.SetString("Username", user.Username);

            return RedirectToAction("Index", "Home"); // Redirige al dashboard
        }

        // GET: /Auth/Logout
        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); // Elimina toda la sesión
            return RedirectToAction("Login"); // Redirige al login
        }
    }
}
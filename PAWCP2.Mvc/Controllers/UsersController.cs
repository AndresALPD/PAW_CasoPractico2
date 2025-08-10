using Microsoft.AspNetCore.Mvc;
using PAWCP2.Core.Manager;
using PAWCP2.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

public class UsersController : Controller
{
    private readonly IUserBusiness _userService;

    public UsersController(IUserBusiness userService)
    {
        _userService = userService;
    }

    // GET: Users
    public async Task<IActionResult> Index()
    {
        var users = await _userService.GetAllAsync();
        return View(users);
    }

    // POST: Users/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Users user)
    {
        if (!ModelState.IsValid)
        {
            ViewData["Error"] = "Errores: " + string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            var users = await _userService.GetAllAsync();
            return View("Index", users);
        }

        try
        {
            bool saved = await _userService.SaveAsync(user);
            if (!saved)
                ViewData["Error"] = "No se pudo guardar el usuario.";
            else
                ViewData["Success"] = "Usuario creado correctamente.";
        }
        catch (Exception ex)
        {
            ViewData["Error"] = "Excepción al guardar: " + ex.Message;
        }

        var allUsers = await _userService.GetAllAsync();
        return View("Index", allUsers);
    }


    // POST: Users/Edit
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Users user)
    {
        if (!ModelState.IsValid)
        {
            ViewData["Error"] = "Errores de validación: " +
                string.Join("; ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            var users = await _userService.GetAllAsync();
            return View("Index", users);
        }

        try
        {
            // Obtiene la entidad original para evitar tracking duplicado
            var originalUser = await _userService.GetByIdAsync(user.UserId);
            if (originalUser == null)
            {
                ViewData["Error"] = "Usuario no encontrado.";
                var users = await _userService.GetAllAsync();
                return View("Index", users);
            }

            // Actualiza solo las propiedades editables
            originalUser.Username = user.Username;
            originalUser.Email = user.Email;
            originalUser.FullName = user.FullName;
            originalUser.IsActive = user.IsActive;
            // No modificar CreatedAt ni LastLogin

            // Guarda usando la instancia original
            bool updated = await _userService.SaveAsync(originalUser);

            if (!updated)
            {
                ViewData["Error"] = "No se pudo actualizar el usuario.";
            }
            else
            {
                ViewData["Success"] = "Usuario actualizado correctamente.";
            }
        }
        catch (Exception ex)
        {
            ViewData["Error"] = "Excepción al actualizar: " + ex.Message;
        }

        var usersList = await _userService.GetAllAsync();
        return View("Index", usersList);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null)
            {
                ViewData["Error"] = "Usuario no encontrado.";
            }
            else
            {
                // Cambiar IsActive a false (o 0)
                user.IsActive = false;

                var updated = await _userService.SaveAsync(user);

                if (!updated)
                {
                    ViewData["Error"] = "No se pudo desactivar el usuario.";
                }
                else
                {
                    ViewData["Success"] = "Usuario desactivado correctamente.";
                }
            }
        }
        catch (Exception ex)
        {
            ViewData["Error"] = "Excepción al desactivar: " + ex.Message;
        }

        var users = await _userService.GetAllAsync();
        return View("Index", users);
    }

}

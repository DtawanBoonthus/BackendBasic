using BackendBasic.Models;
using BackendBasic.Services;
using Microsoft.AspNetCore.Mvc;

namespace BackendBasic.Controllers;

public class RegisterController(IRegisterService registerService) : Controller
{
    public async Task<IActionResult> ShowData()
    {
        var users = await registerService.GetUsersAsync();
        return View(users);
    }

    public IActionResult RegisterUser()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RegisterUser(User user)
    {
        if (!ModelState.IsValid)
        {
            return View(user);
        }

        bool success = await registerService.CreateUserAsync(user);

        if (!success)
        {
            ModelState.AddModelError(string.Empty, "Error creating user.");
            return View(user);
        }

        return RedirectToAction("ShowData");
    }

    public async Task<IActionResult> Edit(int id)
    {
        var user = await registerService.GetUserByIdAsync(id);
        if (user == null)
        {
            return NotFound($"User ID {id} does not exist.");
        }

        return View(user);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(User user)
    {
        if (!ModelState.IsValid)
        {
            return View(user);
        }

        bool success = await registerService.UpdateUserAsync(user);

        if (!success)
        {
            ModelState.AddModelError(string.Empty, "Error updating user.");
            return View(user);
        }

        return RedirectToAction("ShowData");
    }

    public async Task<IActionResult> Remove(int id)
    {
        bool success = await registerService.DeleteUserAsync(id);

        if (!success)
        {
            return NotFound($"User ID {id} does not exist.");
        }

        return RedirectToAction("ShowData");
    }
}

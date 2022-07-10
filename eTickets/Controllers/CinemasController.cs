using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using eTickets.Data;
using eTickets.Models;
using eTickets.Data.Services;
using eTickets.Data.Static;
using Microsoft.AspNetCore.Authorization;

namespace eTickets.Controllers;

[Authorize(Roles = UserRoles.Admin)]
public class CinemasController : Controller
{
    private readonly ICinemasService _service;

    public CinemasController(ICinemasService service)
    {
        _service = service;
    }
    
    // Get: Cinemas/Create
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create([Bind("Description, Logo, Name")] Cinema cinema)
    {
        // Chữa cháy
        ModelState.Remove("Movies");
        if (!ModelState.IsValid)
        {
            return View(cinema);
        }
        await _service.AddAsync(cinema);
        return RedirectToAction(nameof(Index));
    }
    
    // GET
    [AllowAnonymous]
    public async Task<IActionResult> Index()
    {
        var allCinemas = await _service.GetAllAsync();
        return View(allCinemas);
    }
    
    //GET: cinemas/details/{{id}}
    [AllowAnonymous]
    public async Task<IActionResult> Details(int id)
    {
        var cinemaDetail = await _service.GetByIdAsync(id);
        if (cinemaDetail == null)
        {
            return View("NotFound");
        }

        return View(cinemaDetail);
    }
    
    // Get: Cinemas/Edit/{{id}}
    public async Task<IActionResult> Edit(int id)
    {
        var cinemaDetail = await _service.GetByIdAsync(id);

        if (cinemaDetail == null)
        {
            return View("NotFound");
        }

        return View(cinemaDetail);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, [Bind("Id, Description, Logo, Name")] Cinema cinema)
    {
        // Chữa cháy
        ModelState.Remove("Movies");
        
        if (!ModelState.IsValid)
        {
            return View(cinema);
        }
        
        await _service.UpdateAsync(id, cinema);
        return RedirectToAction(nameof(Index));
    }
    
    // Get: cinemas/Delete/{{id}}
    public async Task<IActionResult> Delete(int id)
    {
        var cinemaDetail = await _service.GetByIdAsync(id);

        if (cinemaDetail == null)
        {
            return View("NotFound");
        }

        return View(cinemaDetail);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirm(int id)
    {
        var cinemaDetail = await _service.GetByIdAsync(id);

        if (cinemaDetail == null)
        {
            return View("NotFound");
        }

        await _service.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
}

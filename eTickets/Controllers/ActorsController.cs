using eTickets.Data;
using eTickets.Data.Services;
using eTickets.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eTickets.Data.Static;
using Microsoft.AspNetCore.Authorization;

namespace eTickets.Controllers;

[Authorize(Roles = UserRoles.Admin)]
public class ActorsController : Controller
{
    private readonly IActorsService _service;

    public ActorsController(IActorsService service)
    {
        _service = service;
    }

    // GET
    [AllowAnonymous]
    public async Task<IActionResult> Index()
    {
        var data = await _service.GetAllAsync();
        return View(data);
    }
    
    // Get: Actors/Create
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create([Bind("FullName, Bio, ProfilePictureURL")] Actor actor)
    {
        // Chữa cháy
        ModelState.Remove("Actors_Movies");
        if (!ModelState.IsValid)
        {
            return View(actor);
        }
        await _service.AddAsync(actor);
        return RedirectToAction(nameof(Index));
    }
    
    // Get: Actors/Details/{{id}}
    [AllowAnonymous]
    public async Task<IActionResult> Details(int id)
    {
        var actorDetails = await _service.GetByIdAsync(id);

        if (actorDetails == null)
        {
            return View("NotFound");
        }

        return View(actorDetails);
    }
    
    // Get: Actors/Edit/{{id}}
    public async Task<IActionResult> Edit(int id)
    {
        var actorDetail = await _service.GetByIdAsync(id);

        if (actorDetail == null)
        {
            return View("NotFound");
        }

        return View(actorDetail);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id,[Bind("FullName, Bio, ProfilePictureURL")] Actor actor)
    {
        // Chữa cháy
        ModelState.Remove("Actors_Movies");
        
        if (!ModelState.IsValid)
        {
            return View(actor);
        }
        
        await _service.UpdateAsync(id, actor);
        return RedirectToAction(nameof(Index));
    }
    
    // Get: Actors/Delete/{{id}}
    public async Task<IActionResult> Delete(int id)
    {
        var actorDetail = await _service.GetByIdAsync(id);

        if (actorDetail == null)
        {
            return View("NotFound");
        }

        return View(actorDetail);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirm(int id)
    {
        var actorDetail = await _service.GetByIdAsync(id);

        if (actorDetail == null)
        {
            return View("NotFound");
        }

        await _service.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
}

using Microsoft.EntityFrameworkCore;
using eTickets.Data;
using eTickets.Data.Services;
using eTickets.Data.Static;
using eTickets.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eTickets.Controllers;

[Authorize(Roles = UserRoles.Admin)]
public class ProducersController : Controller
{
    private readonly IProducersService _service;

    public ProducersController(IProducersService service)
    {
        _service = service;
    }
    
    // Get: Producers/Create
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create([Bind("FullName, Bio, ProfilePictureURL")] Producer producer)
    {
        // Chữa cháy
        ModelState.Remove("Movies");
        if (!ModelState.IsValid)
        {
            return View(producer);
        }
        await _service.AddAsync(producer);
        return RedirectToAction(nameof(Index));
    }
    
    // GET
    [AllowAnonymous]
    public async Task<IActionResult> Index()
    {
        var allProducers = await _service.GetAllAsync();
        return View(allProducers);
    }
    
    //GET: producers/details/{{id}}
    [AllowAnonymous]
    public async Task<IActionResult> Details(int id)
    {
        var producerDetail = await _service.GetByIdAsync(id);
        if (producerDetail == null)
        {
            return View("NotFound");
        }

        return View(producerDetail);
    }
    
    // Get: Producers/Edit/{{id}}
    public async Task<IActionResult> Edit(int id)
    {
        var producerDetail = await _service.GetByIdAsync(id);

        if (producerDetail == null)
        {
            return View("NotFound");
        }

        return View(producerDetail);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id,[Bind("Id, FullName, Bio, ProfilePictureURL")] Producer producer)
    {
        // Chữa cháy
        ModelState.Remove("Movies");
        
        if (!ModelState.IsValid)
        {
            return View(producer);
        }
        
        await _service.UpdateAsync(id, producer);
        return RedirectToAction(nameof(Index));
    }
    
    // Get: Producers/Delete/{{id}}
    public async Task<IActionResult> Delete(int id)
    {
        var producerDetail = await _service.GetByIdAsync(id);

        if (producerDetail == null)
        {
            return View("NotFound");
        }

        return View(producerDetail);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirm(int id)
    {
        var producerDetail = await _service.GetByIdAsync(id);

        if (producerDetail == null)
        {
            return View("NotFound");
        }

        await _service.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
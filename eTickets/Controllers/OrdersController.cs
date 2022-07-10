using System.Security.Claims;
using eTickets.Data.Cart;
using Microsoft.AspNetCore.Mvc;
using eTickets.Data.Services;
using eTickets.Data.Static;
using eTickets.Data.ViewModels;
using eTickets.Models;
using Microsoft.AspNetCore.Authorization;

namespace eTickets.Controllers;

[Authorize]
public class OrdersController : Controller
{
    private readonly IMoviesService _movieService;
    private readonly ShoppingCart _shoppingCart;
    private readonly IOrdersService _ordersService;
    
    public OrdersController(IMoviesService movieService, ShoppingCart shoppingCart, IOrdersService ordersService)
    {
        _movieService = movieService;
        _shoppingCart = shoppingCart;
        _ordersService = ordersService;
    }
    
    public async Task<IActionResult> Index()
    {
        string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        string userRole = User.FindFirstValue(ClaimTypes.Role);
        
        var orders = await _ordersService.GetOrderByUserIdAndRoleAsync(userId, userRole);

        return View(orders);
    }
    
    // GET
    public IActionResult ShoppingCart()
    {
        var items = _shoppingCart.GetShoppingCartItems();
        _shoppingCart.ShoppingCartItems = items;

        var response = new ShoppingCartVM()
        {
            ShoppingCart = _shoppingCart,
            ShoppingCartTotal = _shoppingCart.GetShoppingCartTotal()
        };
        
        return View(response);
    }

    public async Task<RedirectToActionResult> AddItemToShoppingCart(int id)
    {
        var item = await _movieService.GetMovieByIdAsync(id);
        if (item != null)
        {
            _shoppingCart.AddItemToCart(item);
        }

        return RedirectToAction(nameof(ShoppingCart));
    }


    public async Task<RedirectToActionResult> RemoveItemFromShoppingCart(int id)
    {
        var item = await _movieService.GetMovieByIdAsync(id);

        if (item != null)
        {
            _shoppingCart.RemoveItemFromCart(item);
        }

        return RedirectToAction(nameof(ShoppingCart));
    }

    public async Task<IActionResult> CompleteOrder()
    {
        var items = _shoppingCart.GetShoppingCartItems();
        string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        string uerEmailAddress = User.FindFirstValue(ClaimTypes.Email);;

        await _ordersService.StoreOrderAsync(items, userId, uerEmailAddress);
        await _shoppingCart.ClearShoppingCartAsync();
        
        return View("OrderCompleted");
    }
}
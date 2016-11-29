﻿using Microsoft.AspNetCore.Mvc;
using SportsStore.Filters;
using SportsStore.Models.Domain;


namespace SportsStore.Controllers
{
    [ServiceFilter(typeof(CartSessionFilter))]
    public class CartController : Controller
    {
       private readonly IProductRepository _productRepository;
    
        public CartController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

         public IActionResult Index(Cart cart)
         {
            if (cart.NumberOfLines == 0)
                return View("EmptyCart");
            ViewBag.Total = cart.TotalValue;      
            return View(cart.CartLines);
        }

        public IActionResult Add(int id, int quantity, Cart cart)
        {
            Product product = _productRepository.GetById(id);
            if (product != null)
            {
                cart.AddLine(product, quantity);
               TempData["Info"] = "Product " + product.Name + " has been added to the cart";
            }
            return RedirectToAction(nameof(Index), "Store");
        }
        
        public IActionResult Remove(int id, Cart cart)
        {
            Product product = _productRepository.GetById(id);
            cart.RemoveLine(product);
            TempData["message"] = $"{product.Name} has been removed from the basket";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Plus(int id, Cart cart)
        {
            cart.IncreaseQuantity(id);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Min(int id, Cart cart)
        {
            cart.DecreaseQuantity(id);
            return RedirectToAction(nameof(Index));
        }
      }
}
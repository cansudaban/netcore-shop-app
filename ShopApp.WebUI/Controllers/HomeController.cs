﻿using Microsoft.AspNetCore.Mvc;
using ShopApp.Business.Abstract;
using ShopApp.WebUI.Models;

namespace ShopApp.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductService _productService;

        public HomeController(IProductService productService)
        {
            this._productService = productService;

        }
        public IActionResult Index()
        {
            var productViewModel = new ProductListViewModel() 
            { 
                Products = _productService.GetHomePageProducts()
            };

            return View(productViewModel);
        }
        public IActionResult About()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View("myView");
        }
    }
}

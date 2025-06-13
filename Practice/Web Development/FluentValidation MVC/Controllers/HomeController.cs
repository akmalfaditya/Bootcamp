using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FluentValidationMVC.Models;

namespace FluentValidationMVC.Controllers
{    /// <summary>
    /// HomeController demonstrates FluentValidation implementation with Student management
    /// This controller shows how clean controllers can be when validation is handled by FluentValidation
    /// Notice how we don't need validation logic in controllers - it's all handled automatically
    /// </summary>
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Index action - displays the home page with FluentValidation demo information
        /// Shows the benefits of FluentValidation over Data Annotations
        /// </summary>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Privacy action - displays privacy policy
        /// Standard controller action for demonstration
        /// </summary>
        public IActionResult Privacy()
        {
            return View();
        }

        /// <summary>
        /// Error action - handles application errors
        /// This is the standard error handling pattern in ASP.NET Core MVC
        /// </summary>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

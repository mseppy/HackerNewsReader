using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models.Interfaces;
using System.Diagnostics;
using Web.Models;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IStoryService storyService;
        public HomeController(ILogger<HomeController> logger, IStoryService storyService)
        {
            _logger = logger;
            this.storyService = storyService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult GetTopStories()
        {
            var stories = storyService.GetTopStoriesAsync(50);
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

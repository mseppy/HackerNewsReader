using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models;
using Models.Interfaces;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Web.Models;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IStoryService storyService;

        private readonly int pageSize = 25;  //this would normally come from the page, but i couldn't get it to work ViewData

        public HomeController(ILogger<HomeController> logger, IStoryService storyService)
        {
            _logger = logger;
            this.storyService = storyService;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["SortTitle"] = "Latest Stories";
            ViewData["PageSize"] = pageSize;

            var data = await storyService.GetNewStoriesAsync(pageSize);
            var stories = data.Select(d => Map(d)).ToList();
            return View(stories);
        }

        public async Task<IActionResult> GetTop()
        {
            ViewData["SortTitle"] = "Top Stories";
            ViewData["PageSize"] = pageSize;

            var data = await storyService.GetTopStoriesAsync(pageSize);
            var stories = data.Select(d => Map(d)).ToList();
            return View("Index", stories);
        }

        public async Task<IActionResult> GetBest()
        {
            ViewData["SortTitle"] = "Best Stories";
            ViewData["PageSize"] = pageSize;

            var data = await storyService.GetBestStoriesAsync(pageSize);
            var stories = data.Select(d => Map(d)).ToList();
            return View("Index", stories);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Privacy()
        {
            return View();
        }

        private StoryViewModel Map(Story story)
        {
            return new StoryViewModel
            {
                Title = story.Title,
                Url = story.Url,
                Text = story.Text,
                Submitter = story.UserName,
                AgeDescription = CreateAgeDescription(story.Time.ToLocalTime().DateTime)
            };
        }

        private string CreateAgeDescription(DateTime submissionTime)
        {
            var age = DateTime.Now - submissionTime;
            if (age.Days > 30)
            {
                return submissionTime.ToString("d");
            }
            if (age.Days > 7)
            {
                return Math.Round(age.Days / 7.0, 0) + " weeks ago";
            }
            if (age.Days > 1)
            {
                return age.Days + " days ago";
            }
            if (age.Days >= 1 && age.Days < 2)
            {
                return "yesterday";
            }
            if (age.Hours > 0)
            {
                return age.Hours + " hours ago";
            }
            if (age.Minutes < 2)
            {
                return "just now";
            }
            return age.Minutes + " minutes ago";
        }
    }
}

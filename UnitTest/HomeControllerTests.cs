using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models;
using Models.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Controllers;
using Web.Models;

namespace UnitTests.Controllers
{
    [TestClass]
    public class HomeControllerTests
    {
        readonly HomeController HomeController;

        readonly Mock<ILogger<HomeController>> moqLogger = new Mock<ILogger<HomeController>>();
        readonly Mock<IStoryService> moqService = new Mock<IStoryService>();

        public HomeControllerTests()
        {
            moqLogger.SetupAllProperties();

            HomeController = new HomeController(moqLogger.Object, moqService.Object);
        }

        [TestMethod]
        public void GetNewStories()
        {
            // Arrange
            const int storyCount = 7;
            var testStories = GenerateTestStories(storyCount);
            moqService.Setup(r => r.GetNewStoriesAsync(It.IsAny<int>())).ReturnsAsync(testStories);
            var expectedCandidate = testStories.First();
            var expected = new StoryViewModel
            {
                Title = expectedCandidate.Title,
                Submitter = expectedCandidate.UserName,
                Url = expectedCandidate.Url,
                AgeDescription = expectedCandidate.StoryID + " hours ago"
            };

            // Act
            var result = HomeController.Index();

            //Assert
            Assert.AreEqual(TaskStatus.RanToCompletion, result.Status);
            var view = (ViewResult)result.Result;
            var actual = view.Model as List<StoryViewModel>;
            Assert.IsNotNull(actual, "Nothing returned.");
            Assert.AreEqual(storyCount, actual.Count, "Actual count different than expected.");
            AssertModel(expected, actual.First());
        }

        [TestMethod]
        public void GetBestStories_OneDayAgo()
        {
            // Arrange
            const int storyCount = 30;
            var testStories = GenerateTestStories(storyCount);
            moqService.Setup(r => r.GetBestStoriesAsync(It.IsAny<int>())).ReturnsAsync(testStories);
            var expectedCandidate = testStories.TakeLast(1).First();
            var expected = new StoryViewModel
            {
                Title = expectedCandidate.Title,
                Submitter = expectedCandidate.UserName,
                Url = expectedCandidate.Url,
                AgeDescription = "yesterday"
            };


            // Act
            var result = HomeController.GetBest();

            //Assert
            Assert.AreEqual(TaskStatus.RanToCompletion, result.Status);
            var view = (ViewResult)result.Result;
            var actual = view.Model as List<StoryViewModel>;
            Assert.IsNotNull(actual, "Nothing returned.");
            Assert.AreEqual(storyCount, actual.Count, "Actual count is different than expected.");
            AssertModel(expected, actual.TakeLast(1).First());
        }

        [TestMethod]
        public void GetTopStories_3daysAgo()
        {
            // Arrange
            const int storyCount = 60;
            var testStories = GenerateTestStories(storyCount);
            moqService.Setup(r => r.GetTopStoriesAsync(It.IsAny<int>())).ReturnsAsync(testStories);
            var expectedCandidate = testStories.TakeLast(1).First();
            var expected = new StoryViewModel
            {
                Title = expectedCandidate.Title,
                Submitter = expectedCandidate.UserName,
                Url = expectedCandidate.Url,
                AgeDescription = "2 days ago"
            };


            // Act
            var result = HomeController.GetTop();

            //Assert
            Assert.AreEqual(TaskStatus.RanToCompletion, result.Status);
            var view = (ViewResult)result.Result;
            var actual = view.Model as List<StoryViewModel>;
            Assert.IsNotNull(actual, "Nothing returned.");
            Assert.AreEqual(storyCount, actual.Count, "Actual count different than expected.");
            AssertModel(expected, actual.TakeLast(1).First());
        }


        #region Test Helpers

        private void AssertModel(StoryViewModel expected, StoryViewModel actual)
        {
            Assert.AreEqual(expected.Title, actual.Title, "Actual title is different then expected.");
            Assert.AreEqual(expected.Submitter, actual.Submitter, "Actual submitter is different then expected.");
            Assert.AreEqual(expected.Url, actual.Url, "Actual Url is different then expected.");
            Assert.AreEqual(expected.AgeDescription, actual.AgeDescription, "Actual AgeDescription is different then expected.");
        }


        private List<Story> GenerateTestStories(int count)
        {
            var stories = new List<Story>();
            for (var x = 1; x <= count; x++)
            {
                stories.Add(
                    new Story
                    {
                        StoryID = x,
                        Title = "Title " + x.ToString(),
                        Url = "www.example.com/item/" + x,
                        Time = DateTimeOffset.UtcNow.AddHours(-x),
                        UserName = "testuser" + x.ToString()
                    });
            }
            return stories;
        }
        #endregion

    }
}

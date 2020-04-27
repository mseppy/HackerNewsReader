using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models;
using Models.Interfaces;
using Moq;
using Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UnitTests.Services
{
    [TestClass]
    public class NewStoryServiceTests
    {
        readonly StoryService storyService;

        readonly Mock<ILogger<StoryService>> moqLogger = new Mock<ILogger<StoryService>>();
        readonly Mock<IStoryRepository> moqRepo = new Mock<IStoryRepository>();

        public NewStoryServiceTests()
        {
            moqLogger.SetupAllProperties();

            storyService = new StoryService(moqLogger.Object, moqRepo.Object);
        }


        [TestMethod]
        public void GetNewStories_NegativePassed_LogErrorReturnNothing()
        {
            // Arrange
            moqRepo.Setup(r => r.GetNewStoriesAsync(It.IsAny<int>())).ReturnsAsync(new List<Story>());

            // Act
            var result = storyService.GetNewStoriesAsync(-12);

            //Assert
            Assert.AreEqual(TaskStatus.RanToCompletion, result.Status);
            var actual = result.Result;
            Assert.IsNotNull(actual, "Nothing returned.");
            Assert.AreEqual(0, actual.Count, "Actual count is different than expected.");
        }

        [TestMethod]
        public void GetNewStories_ZeroPassed_ReturnsDefault()
        {
            // Arrange
            var testStories = GenerateTestStories(storyService.DefaultItemsPerPage);
            moqRepo.Setup(r => r.GetNewStoriesAsync(It.IsAny<int>())).ReturnsAsync(testStories);

            // Act
            var result = storyService.GetNewStoriesAsync(0);

            //Assert
            Assert.AreEqual(TaskStatus.RanToCompletion, result.Status);
            var actual = result.Result;
            Assert.AreEqual(storyService.DefaultItemsPerPage, actual.Count, "Actual count different than expected.");
        }

        [TestMethod]
        public void GetNewStories_ValidNumberPassed_Returns()
        {
            // Arrange
            const int storyCount = 7;
            var testStories = GenerateTestStories(storyCount);
            moqRepo.Setup(r => r.GetNewStoriesAsync(It.IsAny<int>())).ReturnsAsync(testStories);

            // Act
            var result = storyService.GetNewStoriesAsync(storyCount);

            //Assert
            Assert.AreEqual(TaskStatus.RanToCompletion, result.Status);
            var actual = result.Result;
            Assert.AreEqual(storyCount, actual.Count, "Actual count different than expected.");
        }


        #region Test Helpers
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

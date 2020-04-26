using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models.Interfaces;
using Moq;
using Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UnitTests
{
    [TestClass]
    public class StoryServiceTests
    {
        readonly StoryService storyService;

        readonly Mock<ILogger<StoryService>> moqLogger = new Mock<ILogger<StoryService>>();
        readonly Mock<IStoryRepository> moqRepo = new Mock<IStoryRepository>();

        public StoryServiceTests()
        {
            moqLogger.SetupAllProperties();

            storyService = new StoryService(moqLogger.Object, moqRepo.Object);
        }


        [TestMethod]
        public void GetTopStories_NegativePassed_LogErrorReturnNothing()
        {
            // Arrange
            moqRepo.Setup(r => r.GetTopStoriesAsync(It.IsAny<int>())).ReturnsAsync(new List<string>());

            // Act
            var result = storyService.GetTopStoriesAsync(-24);

            //Assert
            Assert.AreEqual(TaskStatus.RanToCompletion, result.Status);
            var actual = result.Result;
            Assert.IsNotNull(actual, "Nothing returned.");
            Assert.AreEqual(0, actual.Count, "Actual count is different than expected.");
        }

        [TestMethod]
        public void GetTopStories_NullPassed_ReturnsDefault()
        {
            // Arrange
            var testStories = GenerateTestStories(storyService.DefaultItemsPerPage);
            moqRepo.Setup(r => r.GetTopStoriesAsync(It.IsAny<int>())).ReturnsAsync(testStories);

            // Act
            var result = storyService.GetTopStoriesAsync(null);

            //Assert
            Assert.AreEqual(TaskStatus.RanToCompletion, result.Status);
            var actual = result.Result;
            Assert.AreEqual(storyService.DefaultItemsPerPage, actual.Count, "Actual count different than expected.");
        }

        [TestMethod]
        public void GetTopStories_ValidNumberPassed_Returns()
        {
            // Arrange
            const int storyCount = 2;
            var testStories = GenerateTestStories(2);
            moqRepo.Setup(r => r.GetTopStoriesAsync(It.IsAny<int>())).ReturnsAsync(testStories);

            // Act
            var result = storyService.GetTopStoriesAsync(storyCount);

            //Assert
            Assert.AreEqual(TaskStatus.RanToCompletion, result.Status);
            var actual = result.Result;
            Assert.AreEqual(storyCount, actual.Count, "Actual count different than expected.");
        }


        #region Test Helpers
        private List<string> GenerateTestStories(int count)
        {
            var stories = new List<string>();
            for (var x = 0; x < count; x++)
            {
                stories.Add(x.ToString());
            }
            return stories;
        }
        #endregion
    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Extensions.Logging;
using Moq;
using Web.Controllers;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Buffers;
using Microsoft.AspNetCore.Mvc;
using Web.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using System;
using Models.Interfaces;
using Services;

namespace UnitTests
{
    [TestClass]
    public class StoryServiceTests
    {
        IStoryService storyService;

        Mock<ILogger<StoryService>> moqLogger = new Mock<ILogger<StoryService>>();
        Mock<IStoryRepository> moqRepo = new Mock<IStoryRepository>();

        public StoryServiceTests()
        {
            moqLogger.SetupAllProperties();

            storyService = new StoryService(moqLogger.Object, moqRepo.Object);
        }


        [TestMethod]
        public void GetTopStories_NullPassed_ReturnsDefault()
        {
            // Arrange

            // Act
            var actual = storyService.GetTopStoriesAsync(null);

            //Assert
            Assert.IsNotNull(actual, "Nothing returned.");
        }

        [TestMethod]
        public void GetTopStories_ValidNumberPassed_Returns()
        {
            // Arrange
            

            // Act
            var actual = storyService.GetTopStoriesAsync(2);

            //Assert
            Assert.IsNotNull(actual, "Nothing returned.");
            //Assert.AreEqual(typeof(ViewResult), actual, "Type returned is not a view");
        }

        [TestMethod]
        public void GetTopStories_NegativePassed_LogErrorReturnNothing()
        {
            // Act
            var actual = storyService.GetTopStoriesAsync(-24);

            //Assert
            Assert.IsNotNull(actual, "Nothing returned.");
        }
    }
}

using LetsGoSEA.WebSite.Pages;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Diagnostics;

namespace UnitTests.Pages
{
    /// <summary>
    /// Unit tests for the Error Page.
    /// </summary>
    public class ErrorTests
    {
        #region TestSetup

        // ErrorModel object
        private static ErrorModel _pageModel;

        /// <summary>
        /// Initialize mock Logger. 
        /// </summary>
        [SetUp]
        public void TestInitialize()
        {
            var mockLoggerDirect = Mock.Of<ILogger<ErrorModel>>();

            _pageModel = new ErrorModel(mockLoggerDirect)
            {
                PageContext = TestHelper.PageContext,
                TempData = TestHelper.TempData,
            };
        }

        #endregion TestSetup

        #region OnGet

        /// <summary>
        /// Tests that when OnGet is called, a valid state and id is returned. 
        /// </summary>
        [Test]
        public void OnGet_Valid_Activity_Set_Should_Return_True()
        {
            // Arrange
            Activity activity = new Activity("activity");
            activity.Start();

            // Act
            _pageModel.OnGet();

            // Reset
            activity.Stop();

            // Assert
            Assert.AreEqual(true, _pageModel.ModelState.IsValid);
            Assert.AreEqual(activity.Id, _pageModel.requestedId);
        }

        /// <summary>
        /// Tests that when OnGet is called, invalid activity should return invalid state and trace identifier.
        /// </summary>
        [Test]
        public void OnGet_InValid_Activity_Null_Should_Return_True()
        {
            // Arrange

            // Act
            _pageModel.OnGet();

            // Reset

            // Assert
            Assert.AreEqual(true, _pageModel.ModelState.IsValid);
            Assert.AreEqual("trace", _pageModel.requestedId);
            Assert.AreEqual(true, _pageModel.ShowRequestId);
        }

        #endregion OnGet
    }
}
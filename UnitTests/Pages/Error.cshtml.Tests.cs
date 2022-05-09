using System.Diagnostics;
using LetsGoSEA.WebSite.Pages;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace UnitTests.Pages
{
    /// <summary>
    /// Unit test for Error page
    /// </summary>
    public class ErrorTests
    {
        #region TestSetup
        // ErrorModel object
        public static ErrorModel PageModel;

        /// <summary>
        /// Set up ILogger mock for testing  
        /// </summary>
        [SetUp]
        public void TestInitialize()
        {
            var mockLoggerDirect = Mock.Of<ILogger<ErrorModel>>();

            PageModel = new ErrorModel(mockLoggerDirect)
            {
                PageContext = TestHelper.PageContext,
                TempData = TestHelper.TempData,
            };
        }

        #endregion TestSetup

        #region OnGet
        /// <summary>
        /// Test GET method: valid activity should return valid state and ID
        /// </summary>
        [Test]
        public void OnGet_Valid_Activity_Set_Should_Return_RequestId()
        {
            // Arrange

            Activity activity = new Activity("activity");
            activity.Start();

            // Act
            PageModel.OnGet();

            // Reset
            activity.Stop();

            // Assert
            Assert.AreEqual(true, PageModel.ModelState.IsValid);
            Assert.AreEqual(activity.Id, PageModel.RequestId);
        }

        /// <summary>
        /// Test GET method: invalid activity should return invalid state and trace identifier
        /// </summary>
        [Test]
        public void OnGet_InValid_Activity_Null_Should_Return_TraceIdentifier()
        {
            // Arrange

            // Act
            PageModel.OnGet();

            // Reset

            // Assert
            Assert.AreEqual(true, PageModel.ModelState.IsValid);
            Assert.AreEqual("trace", PageModel.RequestId);
            Assert.AreEqual(true, PageModel.ShowRequestId);
        }
        #endregion OnGet
    }
}
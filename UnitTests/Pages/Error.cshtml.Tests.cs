using System.Diagnostics;
using LetsGoSEA.WebSite.Pages;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace UnitTests.Pages
{
    public class ErrorTests
    {
        #region TestSetup
        public static ErrorModel PageModel;

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
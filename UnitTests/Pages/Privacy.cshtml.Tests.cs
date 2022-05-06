using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using LetsGoSEA.WebSite.Pages;

namespace UnitTests.Pages
{
    public class PrivacyTests
    {
        #region TestSetup

        private static PrivacyModel _pageModel;

        [SetUp]
        public void TestInitialize()
        {
            var mockLoggerDirect = Mock.Of<ILogger<PrivacyModel>>();

            _pageModel = new PrivacyModel(mockLoggerDirect)
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

            // Act
            _pageModel.OnGet();

            // Reset

            // Assert
            Assert.AreEqual(true, _pageModel.ModelState.IsValid);
        }

        #endregion OnGet
    }
}
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using LetsGoSEA.WebSite.Pages;

namespace UnitTests.Pages
{
    /// <summary>
    /// Unit test for Privacy Page
    /// </summary>
    public class PrivacyTests
    {
        #region TestSetup
        // PrivacyModel object
        private static PrivacyModel _pageModel;

        /// <summary>
        /// Set up ILogger mock for testing 
        /// </summary>
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
        /// <summary>
        /// Test GET method: valid model state returns valid page
        /// </summary>
        [Test]
        public void OnGet_Valid_ModelState_Should_Return_True()
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
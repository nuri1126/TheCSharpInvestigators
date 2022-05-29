using LetsGoSEA.WebSite.Pages;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace UnitTests.Pages
{
    /// <summary>
    /// Unit tests for the Privacy Page.
    /// </summary>
    public class PrivacyTests
    {
        #region TestSetup

        // PrivacyModel object.
        private static PrivacyModel _pageModel;

        /// <summary>
        /// Initialize mock Logger. 
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
        /// Test that when OnGet is called, the model state is valid and returns a valid Page. 
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
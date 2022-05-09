using System.Linq;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using LetsGoSEA.WebSite.Pages;


namespace UnitTests.Pages
{
    /// <summary>
    /// Unit test for Neighborhoods Index page 
    /// </summary>
    public class IndexTests
    {
        #region TestSetup

        // IndexModel from Index.cshtml.cs for testing
        private static IndexModel _pageModel;

        /// <summary>
        /// Set up ILogger mock for testing 
        /// </summary>
        [SetUp] 
        public void TestInitialize()
        {
            var mockLoggerDirect = Mock.Of<ILogger<IndexModel>>();

            _pageModel = new IndexModel(mockLoggerDirect, TestHelper.NeighborhoodServiceObj);
        }

        #endregion TestSetup

        #region OnGet
        /// <summary>
        /// Test GET method: valid page should return valid neighborhoods
        /// </summary>
        [Test]
        public void OnGet_Valid_Should_Return_True()
        {
            // Arrange

            // Act
            _pageModel.OnGet();

            // Assert
            Assert.AreEqual(true, _pageModel.ModelState.IsValid);
            Assert.AreEqual(true, _pageModel.Neighborhoods.ToList().Any());
        }
        #endregion OnGet
    }
}
using System.Linq;

using Microsoft.Extensions.Logging;

using Moq;

using NUnit.Framework;

using LetsGoSEA.WebSite.Pages;
using LetsGoSEA.WebSite.Services;


namespace UnitTests.Pages.Index
{
    /// <summary>
    /// Unit test for Neighborhoods Index page 
    /// </summary>
    public class IndexTests
    {
        #region TestSetup

        // IndexModel from Index.cshtml.cs for testing
        public static IndexModel pageModel;

        [SetUp] // Arrange 
        public void TestInitialize()
        {
            var MockLoggerDirect = Mock.Of<ILogger<IndexModel>>();

            pageModel = new IndexModel(MockLoggerDirect, TestHelper.NeighborhoodServiceObj)
            {
            };
        }

        #endregion TestSetup

        #region OnGet
        [Test]
        public void OnGet_Valid_Should_Return_Products()
        {
            // Arrange

            // Act
            pageModel.OnGet();

            // Assert
            Assert.AreEqual(true, pageModel.ModelState.IsValid);
            Assert.AreEqual(true, pageModel.Neighborhoods.ToList().Any());
        }
        #endregion OnGet
    }
}
using LetsGoSEA.WebSite.Pages.Explore;
using NUnit.Framework;
using System.Linq;

namespace UnitTests.Pages.Explore
{
    /// <summary>
    /// Unit test for Explore Index page
    /// </summary>
    public class IndexTests
    {
        #region TestSetup

        // IndexModel object 
        private static IndexModel _pageModel;

        /// <summary>
        /// Set up IndexModel object for testing
        /// </summary>
        [SetUp]
        public void TestInitialize()
        {
            _pageModel = new IndexModel(TestHelper.NeighborhoodServiceObj);
        }

        #endregion TestSetup

        #region OnGet

        /// <summary>
        /// Test GET method: valid page should return neighborhood objects
        /// </summary>
        [Test]
        public void OnGet_Valid_Model_valid_Should_Return_True()
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
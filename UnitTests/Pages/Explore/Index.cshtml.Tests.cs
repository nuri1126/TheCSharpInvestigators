using LetsGoSEA.WebSite.Pages.Explore;
using NUnit.Framework;
using System.Linq;

namespace UnitTests.Pages.Explore
{
    /// <summary>
    /// Unit test for Explore Index page.
    /// </summary>
    public class IndexTests
    {
        #region TestSetup

        // IndexModel object 
        private static IndexModel _pageModel;

        /// <summary>
        /// Initialize IndexModel private field by passing the TestHelper's Neighborhood 
        /// Service as a parameter. 
        /// </summary>
        [SetUp]
        public void TestInitialize()
        {
            // Initialize IndexModel with a NeighborhoodService object. 
            _pageModel = new IndexModel(TestHelper.NeighborhoodServiceObj);
        }

        #endregion TestSetup

        #region OnGet

        /// <summary>
        /// Tests Index.cshtml OnGet method. A valid call to OnGet should return 
        /// the current list of Neighborhood objects. 
        /// </summary>
        [Test]
        public void OnGet_Valid_Model_valid_Should_Return_True()
        {
            // Arrange


            // Act
            _pageModel.OnGet();

            // Assert
            Assert.AreEqual(true, _pageModel.ModelState.IsValid);
            Assert.AreEqual(true, _pageModel.neighborhoods.ToList().Any());
        }

        #endregion OnGet
    }
}
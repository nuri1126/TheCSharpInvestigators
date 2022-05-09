using System.Linq;
using NUnit.Framework;
using LetsGoSEA.WebSite.Pages.Neighborhood;

namespace UnitTests.Pages.Neighborhood

{
    /// <summary>
    /// Unit test for Index page
    /// </summary>
    public class IndexTests
    {
        #region TestSetup
        // IndexModel object
        private static IndexModel _pageModel;

        /// <summary>
        /// Set up Index Model object for testing
        /// </summary>
        [SetUp]
        public void TestInitialize()
        {
            _pageModel = new IndexModel(TestHelper.NeighborhoodServiceObj);
        }

        #endregion TestSetup


        #region OnGet
        /// <summary>
        /// Test GET method: valid page should return neighborhoods
        /// </summary>
        [Test]
        public void OnGet_Valid_Should_Return_Neighborhoods()
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
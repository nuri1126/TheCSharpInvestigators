using LetsGoSEA.WebSite.Pages.Neighborhood;
using NUnit.Framework;
using System.Linq;

namespace UnitTests.Pages.Neighborhood

{
    /// <summary>
    /// Unit test for the Index Page.
    /// </summary>
    public class IndexTests
    {
        #region TestSetup

        // IndexModel object.
        private static IndexModel _pageModel;

        /// <summary>
        /// Initialize IndexModel with a NeighborhoodService object. 
        /// </summary>
        [SetUp]
        public void TestInitialize()
        {
            _pageModel = new IndexModel(TestHelper.NeighborhoodServiceObj);
        }

        #endregion TestSetup

        #region OnGet

        /// <summary>
        /// Test that when OnGet is called, a valid ModelState should return true and
        /// return all neighborhoods in the database.
        /// </summary>
        [Test]
        public void OnGet_Valid_Should_Return_True_And_Return_All_Neighborhoods()
        {
            // Arrange

            // Act
            _pageModel.OnGet();

            // Assert
            Assert.AreEqual(true, _pageModel.ModelState.IsValid);
            Assert.AreEqual(true, _pageModel.neighborhoods.ToList().Any());
        }

        /// <summary>
        /// Tests that when OnGet is called, an invalid ModelState will return false.
        /// </summary>
        [Test]
        public void OnGet_Invalid_ModelState_Should_Return_False()
        {
            // Arrange

            // Force an invalid error state.
            _pageModel.ModelState.AddModelError("InvalidState", "Neighborhood is invalid");

            // Act
            var result = _pageModel.OnGet();

            // Assert
            Assert.AreEqual(false, _pageModel.ModelState.IsValid);
        }

        #endregion OnGet

    }
}
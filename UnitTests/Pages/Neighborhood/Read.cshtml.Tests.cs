using LetsGoSEA.WebSite.Pages.Neighborhood;
using NUnit.Framework;

namespace UnitTests.Pages.Neighborhood
{
    /// <summary>
    /// Unit test for Read page
    /// </summary>
    public class ReadTests
    {
        #region TestSetup
        // ReadModel object
        private static ReadModel _pageModel;

        /// <summary>
        /// Set up Read Model object for testing 
        /// </summary>
        [SetUp]
        public void TestInitialize()
        {
            _pageModel = new ReadModel(TestHelper.NeighborhoodServiceObj);
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
            _pageModel.OnGet(1);

            // Assert
            Assert.AreEqual(true, _pageModel.ModelState.IsValid);
            Assert.AreEqual("Northgate", _pageModel.Neighborhood.Name);

        }

        #endregion OnGet

    }
}
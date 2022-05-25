using LetsGoSEA.WebSite.Pages.Neighborhood;
using NUnit.Framework;

namespace UnitTests.Pages.Neighborhood
{
    /// <summary>
    /// Unit test for the Read Page.
    /// </summary>
    public class ReadTests
    {
        #region TestSetup

        // ReadModel object
        private static ReadModel _pageModel;

        /// <summary>
        /// Initialize ReadModel with a NeighborhoodService object. 
        /// </summary>
        [SetUp]
        public void TestInitialize()
        {
            _pageModel = new ReadModel(TestHelper.NeighborhoodServiceObj);
        }

        #endregion TestSetup


        #region OnGet
        /// <summary>
        /// Tests that when OnGet is called, the selected neighborhood is returned.
        /// </summary>
        [Test]
        public void OnGet_Valid_Should_Return_Neighborhoods()
        {

            // Arrange

            // Act
            _pageModel.OnGet(1);

            // Assert
            Assert.AreEqual(true, _pageModel.ModelState.IsValid);
            Assert.AreEqual("Northgate", _pageModel.neighborhood.name);
        }

        #endregion OnGet

    }
}
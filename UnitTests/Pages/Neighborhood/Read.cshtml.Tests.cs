using NUnit.Framework;
using LetsGoSEA.WebSite.Pages.Neighborhood;

namespace UnitTests.Pages.Neighborhood
{
    public class ReadTests
    {
        #region TestSetup

        private static ReadModel _pageModel;

        [SetUp]
        public void TestInitialize()
        {
            _pageModel = new ReadModel(TestHelper.NeighborhoodServiceObj);
        }

        #endregion TestSetup


        #region OnGet

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
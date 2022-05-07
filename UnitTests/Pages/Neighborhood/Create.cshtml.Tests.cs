using System.Linq;
using NUnit.Framework;
using LetsGoSEA.WebSite.Pages.Neighborhood;

namespace UnitTests.Pages.Neighborhood
{
    public class CreateTests
    {
        #region TestSetup

        private static CreateModel _pageModel;

        [SetUp]
        public void TestInitialize()
        {
            _pageModel = new CreateModel(TestHelper.NeighborhoodServiceObj);
        }

        #endregion TestSetup


        #region OnGet

        [Test]
        public void GetNeighborhoods_Valid_Count_Should_Return_True()
        {
            // Arrange
            var oldCount = TestHelper.NeighborhoodServiceObj.GetNeighborhoods().Count();

            // Act
            _pageModel.OnGet();

            // Assert
            Assert.AreEqual(true, _pageModel.ModelState.IsValid);
            Assert.AreEqual(oldCount + 1, TestHelper.NeighborhoodServiceObj.GetNeighborhoods().Count());
        }


        #endregion OnGet
    }
}

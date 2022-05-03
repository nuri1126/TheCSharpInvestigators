using System.Linq;
using NUnit.Framework;
using LetsGoSEA.WebSite.Pages.Neighborhood;
using NUnit.Framework.Internal;

namespace UnitTests.Pages.Neighborhood
{
    public class CreateTests
    {
        #region TestSetup
        public static CreateModel PageModel;

        [SetUp]
        public void TestInitialize()
        {
            PageModel = new CreateModel(TestHelper.NeighborhoodServiceObj);
        }

        #endregion TestSetup


        #region OnGet

        [Test]
        public void OnGet_Something()
        {
            // Arrange
            var oldCount = TestHelper.NeighborhoodServiceObj.GetNeighborhoods().Count();

            // Act
            PageModel.OnGet();

            // Assert
            Assert.AreEqual(true, PageModel.ModelState.IsValid);
            Assert.AreEqual(oldCount + 1, TestHelper.NeighborhoodServiceObj.GetNeighborhoods().Count());
        }


        #endregion OnGet
    }
}

using System.Linq;
using NUnit.Framework;
using LetsGoSEA.WebSite.Pages.Neighborhood;

namespace UnitTests.Pages.Neighborhood.Read
{
    public class ReadTests
    {
        #region TestSetup
        public static ReadModel PageModel;

        [SetUp]
        public void TestInitialize()
        {
            PageModel = new ReadModel(TestHelper.NeighborhoodServiceObj);
        }

        #endregion TestSetup


        #region OnGet

        [Test]
        public void OnGet_Valid_Should_Return_Neighborhoods()
        {

            // Arrange

            // Act
            PageModel.OnGet(1);

            // Assert
            Assert.AreEqual(true, PageModel.ModelState.IsValid);
            Assert.AreEqual("Northgate", PageModel.Neighborhood.Name);

        }


        #endregion OnGet

    }
}
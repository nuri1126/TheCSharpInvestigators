using System.Linq;
using NUnit.Framework;
using LetsGoSEA.WebSite.Pages.Neighborhood;

namespace UnitTests.Pages.Neighborhood.Index

{
    public class IndexTests
    {
        #region TestSetup
        public static IndexModel PageModel;

        [SetUp]
        public void TestInitialize()
        {
            PageModel = new IndexModel(TestHelper.NeighborhoodServiceObj);
        }

        #endregion TestSetup


        #region OnGet
        [Test]
        public void OnGet_Valid_Should_Return_Neighborhoods()
        {
            // Arrange

            // Act
            PageModel.OnGet();

            // Assert
            Assert.AreEqual(true, PageModel.ModelState.IsValid);
            Assert.AreEqual(true, PageModel.Neighborhoods.ToList().Any());
        }
        #endregion OnGet
    }
}
using System.Linq;
using NUnit.Framework;
using LetsGoSEA.WebSite.Pages.Neighborhood;

namespace UnitTests.Pages.Neighborhood
{
    public class Create
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
            
            // Act
            
            // Assert
        }


        #endregion OnGet
    }
}

using System.Linq;
using NUnit.Framework;
using LetsGoSEA.WebSite.Pages.Neighborhood;

namespace UnitTests.Pages.Neighborhood
{
    public class UpdateTests
    {
        #region TestSetup
        public static UpdateModel PageModel;

        [SetUp]
        public void TestInitialize()
        {
            PageModel = new UpdateModel(TestHelper.NeighborhoodServiceObj);
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
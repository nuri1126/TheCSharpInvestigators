using System.Linq;
using NUnit.Framework;
using LetsGoSEA.WebSite.Pages.Neighborhood;

namespace UnitTests.Pages.Neighborhood
{
    public class Update
    {
        #region TestSetup
        public static UpdateModel PageModel;

        [SetUp]
        public void TestInitialize()
        {
            PageModel = new UpdateModel();
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
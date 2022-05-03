using System.Linq;
using NUnit.Framework;
using LetsGoSEA.WebSite.Pages.Neighborhood;

namespace UnitTests.Pages.Neighborhood
{
    public class IndexModel
    {
        #region TestSetup
        public static IndexModel PageModel;

        [SetUp]
        public void TestInitialize()
        {
            PageModel = new IndexModel();
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
using System.Linq;
using NUnit.Framework;
using LetsGoSEA.WebSite.Pages.Neighborhood;

namespace UnitTests.Pages.Neighborhood
{
    public class DeleteModel
    {
        #region TestSetup
        public static DeleteModel PageModel;

        [SetUp]
        public void TestInitialize()
        {
            PageModel = new DeleteModel();
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
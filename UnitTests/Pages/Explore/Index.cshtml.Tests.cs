using System.Linq;
using LetsGoSEA.WebSite.Pages.Explore;
using NUnit.Framework;

namespace UnitTests.Pages.Explore
{
    public class IndexTests
    {
        #region TestSetup

        private static IndexModel _pageModel;

        [SetUp]
        public void TestInitialize()
        {
            _pageModel = new IndexModel(TestHelper.NeighborhoodServiceObj);
        }

        #endregion TestSetup
        
        #region OnGet

        [Test]
        public void OnGet_Valid_Should_Return_Neighborhoods()
        {
            // Arrange
            
            
            // Act
            _pageModel.OnGet();
            
            // Assert
            Assert.AreEqual(true, _pageModel.ModelState.IsValid);
            Assert.AreEqual(true, _pageModel.Neighborhoods.ToList().Any());
        }
        
        #endregion OnGet
    }
}
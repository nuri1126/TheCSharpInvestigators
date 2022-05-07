using System.Linq;
using LetsGoSEA.WebSite.Pages;
using NUnit.Framework;

namespace UnitTests.Pages
{
    public class AboutUsTests
    {
        #region TestSetup

        private static AboutUsModel _pageModel;

        [SetUp]
        public void TestInitialize()
        {
            _pageModel = new AboutUsModel(TestHelper.AboutUsServiceObj);
        }

        #endregion TestSetup

        #region OnGet

        [Test]
        public void OnGet_Valid_Members_Should_Return_True()
        {
            // Arrange
            
            // Act
            _pageModel.OnGet();
            
            // Assert
            Assert.AreEqual(true, _pageModel.ModelState.IsValid);
            Assert.AreEqual(true, _pageModel.Members.ToList().Any());
            
        }

        #endregion OnGet
    }
}
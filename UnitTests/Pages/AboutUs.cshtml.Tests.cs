using System.Linq;
using LetsGoSEA.WebSite.Pages;
using NUnit.Framework;

namespace UnitTests.Pages
{
    /// <summary>
    /// Unit test for About US page
    /// </summary>
    public class AboutUsTests
    {
        #region TestSetup
        // AboutUsModel object
        private static AboutUsModel _pageModel;

        /// <summary>
        /// Set up AboutUs Model for testing
        /// </summary>
        [SetUp]
        public void TestInitialize()
        {
            _pageModel = new AboutUsModel(TestHelper.AboutUsServiceObj);
        }

        #endregion TestSetup

        #region OnGet
        /// <summary>
        /// Test GET method: valid team member objects should return valid page
        /// </summary>
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
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace UnitTests.Pages
{
    /// <summary>
    /// Unit test for About US page
    /// </summary>
    public class AboutUsTests
    {
        #region TestSetup
        // AboutUsModel object
        private static LetsGoSEA.WebSite.Pages.AboutUsModel _pageModel;

        /// <summary>
        /// Set up AboutUs Model for testing
        /// </summary>
        [SetUp]
        public void TestInitialize()
        {
            _pageModel = new LetsGoSEA.WebSite.Pages.AboutUsModel(TestHelper.AboutUsServiceObj);
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

        #region Model_Properties_Should_Be_Not_Null() 
        [Test]
        public void Model_Properties_Should_Be_Not_Null()
        {
            // Arrange
            _pageModel.OnGet();
            IEnumerable<LetsGoSEA.WebSite.Models.AboutUsModel> members = _pageModel.Members;

            // Act

            foreach (var member in members)
            {
                Assert.NotNull(member.Name);
                Assert.NotNull(member.Bio);
                Assert.NotNull(member.LinkedIn);
                Assert.NotNull(member.Image);
                Assert.NotNull(member.Bio);
            }
        }
        #endregion Model_Properties_Should_Be_Not_Null() 
    }
}
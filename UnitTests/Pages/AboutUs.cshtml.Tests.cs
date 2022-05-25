using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace UnitTests.Pages
{
    /// <summary>
    /// Unit tests for the About Us Page.
    /// </summary>
    public class AboutUsTests
    {
        #region TestSetup

        // AboutUsModel object
        private static LetsGoSEA.WebSite.Pages.AboutUsModel _pageModel;

        /// <summary>
        /// Initialize AboutUsModel for with a NeighborhoodService object. 
        /// </summary>
        [SetUp]
        public void TestInitialize()
        {
            _pageModel = new LetsGoSEA.WebSite.Pages.AboutUsModel(TestHelper.AboutUsServiceObj);
        }

        #endregion TestSetup

        #region OnGet
        /// <summary>
        /// Test that when OnGet is called, valid team member objects are returned to the page. 
        /// </summary>
        [Test]
        public void OnGet_Valid_Members_Should_Return_True()
        {
            // Arrange

            // Act
            _pageModel.OnGet();

            // Assert
            Assert.AreEqual(true, _pageModel.ModelState.IsValid);
            Assert.AreEqual(true, _pageModel.members.ToList().Any());
        }

        /// <summary>
        /// Tests that when OnGet is called, the properties of each member object are not null.
        /// </summary>
        [Test]
        public void OnGet_Model_Properties_Should_Be_Not_Null()
        {
            // Arrange

            // Act
            _pageModel.OnGet();
            IEnumerable<LetsGoSEA.WebSite.Models.AboutUsModel> members = _pageModel.members;

            // Assert
            foreach (var member in members)
            {
                Assert.NotNull(member.name);
                Assert.NotNull(member.bio);
                Assert.NotNull(member.linkedIn);
                Assert.NotNull(member.image);
                Assert.NotNull(member.bio);
            }
        }

        #endregion OnGet

    }
}
using LetsGoSEA.WebSite.Pages;
using NUnit.Framework;

namespace UnitTests.Pages
{
    /// <summary>
    /// Unit tests for the Privacy Page.
    /// </summary>
    public class PrivacyTests
    {

        // PrivacyModel object.
        private static readonly PrivacyModel _pageModel = new PrivacyModel();

        #region OnGet
        /// <summary>
        /// Test that when OnGet is called, Privacy PageModel is valid. 
        /// </summary>
        [Test]
        public void OnGet_Valid_ModelState_IsValid_Should_Return_True()
        {
            // Arrange

            // Act
            _pageModel.OnGet();

            // Reset

            // Assert
            Assert.AreEqual(true, _pageModel.ModelState.IsValid);
        }

        #endregion OnGet
    }
}
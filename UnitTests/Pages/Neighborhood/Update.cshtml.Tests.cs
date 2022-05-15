using LetsGoSEA.WebSite.Models;
using LetsGoSEA.WebSite.Pages.Neighborhood;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using System.Diagnostics;

namespace UnitTests.Pages.Neighborhood
{
    /// <summary>
    /// Unit test for Update page
    /// </summary>
    public class UpdateTests
    {
        #region TestSetup
        // UpdateModel object
        private static UpdateModel _pageModel;

        /// <summary>
        /// Set up Update Model for testing
        /// </summary>
        [SetUp]
        public void TestInitialize()
        {
            _pageModel = new UpdateModel(TestHelper.NeighborhoodServiceObj);
        }

        #endregion TestSetup


        #region OnGet
        /// <summary>
        /// Test GET method: valid page should return neighborhoods
        /// </summary>
        [Test]
        public void OnGet_Valid_Should_Return_Neighborhoods()
        {
            // Arrange

            // Act
            _pageModel.OnGet(2);

            // Assert
            Assert.AreEqual(true, _pageModel.ModelState.IsValid);
            Assert.AreEqual("Greenlake", _pageModel.Neighborhood.Name);
        }
        #endregion OnGet

        #region OnPostAsync
        /// <summary>
        /// Test POST method: valid page should return neighborhoods
        /// </summary>
        [Test]
        public void OnPostAsync_Valid_Should_Return_Neighborhoods()
        {
            // Arrange
            _pageModel.Neighborhood = new NeighborhoodModel
            {
                Id = 2,
                Name = "Greenwood",
                ShortDesc = "Welcome to the green neighborhood of Greenwood"
            };

            // Act
            var result = _pageModel.OnPost() as RedirectToPageResult;

            // Assert
            Assert.AreEqual(true, _pageModel.ModelState.IsValid);
            Debug.Assert(result != null, nameof(result) + " != null");
            Assert.AreEqual(true, result.PageName.Contains("Index"));
        }

        /// <summary>
        /// Test POST method: invalid data should make ModelState invalid
        /// </summary>
        [Test]
        public void OnPostAsync_InValid_ModelState_Should_Return_False()
        {
            // Arrange
            _pageModel.Neighborhood = new NeighborhoodModel
            {
                Id = 666,
                Name = "Invalid Name",
                ShortDesc = "Invalid Desc"
            };

            // Force an invalid error state
            _pageModel.ModelState.AddModelError("InvalidState", "Invalid Neighborhood state");

            // Act
            var result = _pageModel.OnPost() as ActionResult;

            // Assert
            Assert.AreEqual(false, _pageModel.ModelState.IsValid);
        }

        #endregion OnPostAsync
    }
}
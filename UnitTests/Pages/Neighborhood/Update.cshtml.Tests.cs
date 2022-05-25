using LetsGoSEA.WebSite.Models;
using LetsGoSEA.WebSite.Pages.Neighborhood;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using System.Diagnostics;

namespace UnitTests.Pages.Neighborhood
{
    /// <summary>
    /// Unit tests for the Update page.
    /// </summary>
    public class UpdateTests
    {
        #region TestSetup
        // UpdateModel object
        private static UpdateModel _pageModel;

        /// <summary>
        /// //Initialize UpdateModel with a NeighborhoodService object. 
        /// </summary>
        [SetUp]
        public void TestInitialize()
        {
            _pageModel = new UpdateModel(TestHelper.NeighborhoodServiceObj);
        }

        #endregion TestSetup


        #region OnGet
        /// <summary>
        /// Test that when OnGet is called, all neighborhoods in the database
        /// are returned.
        /// </summary>
        [Test]
        public void OnGet_Valid_Should_Return_Neighborhoods()
        {
            // Arrange

            // Act
            _pageModel.OnGet(2);

            // Assert
            Assert.AreEqual(true, _pageModel.ModelState.IsValid);
            Assert.AreEqual("Greenlake", _pageModel.neighborhood.name);
        }
        #endregion OnGet

        #region OnPost
        /// <summary>
        /// Test that when a neighborhood is updated and OnPost is called, the
        /// neighborhood is returned with the updates in affect.
        /// </summary>
        [Test]
        public void OnPost_Valid_Should_Return_Neighborhoods()
        {
            // Arrange
            _pageModel.neighborhood = new NeighborhoodModel
            {
                id = 2,
                name = "Greenwood",
                shortDesc = "Welcome to the green neighborhood of Greenwood"
            };

            // Act
            var result = _pageModel.OnPost() as RedirectToPageResult;

            // Assert
            Assert.AreEqual(true, _pageModel.ModelState.IsValid);
            Debug.Assert(result != null, nameof(result) + " != null");
            Assert.AreEqual(true, result.PageName.Contains("Index"));
        }

        /// <summary>
        /// Tests that when a neighborhood is trying to be updated with invalid data should
        /// the ModelState becomes invalid.
        /// </summary>
        [Test]
        public void OnPost_InValid_ModelState_Should_Return_False()
        {
            // Arrange
            _pageModel.neighborhood = new NeighborhoodModel
            {
                id = 666,
                name = "Invalid Name",
                shortDesc = "Invalid Desc"
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
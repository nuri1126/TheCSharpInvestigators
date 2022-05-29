using LetsGoSEA.WebSite.Models;
using LetsGoSEA.WebSite.Pages.Neighborhood;
using LetsGoSEA.WebSite.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using System.Diagnostics;
using System.Linq;

namespace UnitTests.Pages.Neighborhood
{
    /// <summary>
    /// Unit tests for the Update page.
    /// </summary>
    public class UpdateTests
    {

        #region TestSetup

        // Global valid name property for use in tests. 
        private static readonly string Name = "Bogusland";

        // Global valid image property for use in tests. 
        private static readonly string Image = "http://via.placeholder.com/150";

        // Global valid shortDesc property for use in tests.
        private static readonly string ShortDesc = "Test neighborhood description";

        // Global imgFiles property for use in tests. 
        private static IFormFileCollection ImgFilesNull = null;

        // Global NeighborhodService to use for all test cases. 
        private NeighborhoodService _neighborhoodService;

        // UpdateModel object.
        private static UpdateModel _pageModel;

        /// <summary>
        /// //Initialize UpdateModel with a NeighborhoodService object. 
        /// </summary>
        [SetUp]
        public void TestInitialize()
        {

            // Abstract NeighborhoodService object from TestHelper.
            _neighborhoodService = TestHelper.NeighborhoodServiceObj;

            // Initialize pageModel.
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

            // Add test neighborhood to database.
            _neighborhoodService.AddData(Name, Image, ShortDesc, ImgFilesNull);

            // Retrieve test neighborhood.
            var testNeighborhood = _neighborhoodService.GetNeighborhoods().Last();

            // Act
            _pageModel.OnGet(testNeighborhood.id);

            // Assert
            Assert.AreEqual(true, _pageModel.ModelState.IsValid);
            Assert.AreEqual(testNeighborhood.name, _pageModel.neighborhood.name);

            // TearDown
            TestHelper.NeighborhoodServiceObj.DeleteData(testNeighborhood.id);
        }

        #endregion OnGet

        #region OnPost

        /// <summary>
        /// Test that when a neighborhood is updated and OnPost is called, the
        /// neighborhood is returned with the updates in affect.
        /// </summary>
        [Test]
        public void OnPost_Valid_Should_Return_True()
        {
            // Arrange
            _pageModel.neighborhood = new NeighborhoodModel
            {
                id = 999,
                name = Name,
                shortDesc = ShortDesc
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
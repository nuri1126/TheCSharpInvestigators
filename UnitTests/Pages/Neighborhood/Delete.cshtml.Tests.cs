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
    /// Unit test for the Delete Page.
    /// </summary>
    public class DeleteTests
    {

        #region TestSetup

        // Global valid name property for use in tests. 
        private static readonly string Name = "Bogusland";

        // Global valid image property for use in tests. 
        private static readonly string Image = "http://via.placeholder.com/150";
        
        // Global valid address property for use in tests
        private static readonly string Address = "401 NE Northgate Way, Seattle, WA 98125";

        // Global valid shortDesc property for use in tests.
        private static readonly string ShortDesc = "Test neighborhood description";

        // Global imgFiles property for use in tests. 
        private static readonly IFormFileCollection ImgFilesNull = null;

        // Global NeighborhoodService to use for all test cases. 
        private NeighborhoodService _neighborhoodService;

        // DeleteModel object.
        private static DeleteModel _pageModel;

        /// <summary>
        /// Initialize DeleteModel a NeighborhoodService object. 
        /// </summary>
        [SetUp]
        public void TestInitialize()
        {
            // Abstract NeighborhoodService object from TestHelper.
            _neighborhoodService = TestHelper.NeighborhoodServiceObj;

            // Initialize pageModel. 
            _pageModel = new DeleteModel(_neighborhoodService);
        }

        #endregion TestSetup

        #region OnGet
        /// <summary>
        /// Tests that when OnGet is called on a valid neighborhood, that neighborhood is returned.
        /// </summary>
        [Test]
        public void OnGet_Valid_Should_Return_Neighborhood()
        {
            // Arrange

            // Add test neighborhood to database.
            TestHelper.NeighborhoodServiceObj.AddData(Name, Address, Image, ShortDesc, ImgFilesNull);

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
        /// Tests that when OnPost is called on a valid neighborhood, the record is deleted from the database.
        /// </summary>
        [Test]
        public void OnPost_Delete_Valid_Neighborhood_Should_Return_Neighborhoods()
        {
            // Create neighborhood object to delete. 
            _pageModel.neighborhood = _neighborhoodService.CreateID();
            _pageModel.neighborhood.name = Name;
            _pageModel.neighborhood.id = 999;
            _neighborhoodService.UpdateData(_pageModel.neighborhood, null);

            // Act
            var result = _pageModel.OnPost() as RedirectToPageResult;

            // Assert
            Assert.AreEqual(true, _pageModel.ModelState.IsValid);
            Debug.Assert(result != null, nameof(result) + " != null");
            Assert.AreEqual(true, result.PageName.Contains("Index"));

            // Confirm the item is deleted.
            Assert.AreEqual(null, TestHelper.NeighborhoodServiceObj.GetNeighborhoodById(_pageModel.neighborhood.id));

        }

        /// <summary>
        /// Test that when OnPost is called on an invalid neighborhood, the ModelState 
        /// becomes invalid. 
        /// </summary>
        [Test]
        public void OnPost_Invalid_Model_NotValid_Should_Return_False()
        {
            // Arrange
            _pageModel.neighborhood = new NeighborhoodModel
            {
                id = 999,
                name = Name,
                shortDesc = ShortDesc
            };

            // Force an invalid error state.
            _pageModel.ModelState.AddModelError("InvalidState", "Neighborhood is invalid");

            // Act
            var result = _pageModel.OnPost() as ActionResult;

            // Assert
            Assert.AreEqual(false, _pageModel.ModelState.IsValid);
        }

        #endregion OnPostAsync
    }
}
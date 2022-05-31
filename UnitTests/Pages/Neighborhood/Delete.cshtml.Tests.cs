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
        private const string Name = "Bogusland";

        // Global valid image property for use in tests. 
        private const string Image = "http://via.placeholder.com/150";

        // Global valid address property for use in tests
        private const string Address = "401 NE Northgate Way, Seattle, WA 98125";

        // Global valid shortDesc property for use in tests.
        private const string ShortDesc = "Test neighborhood description";

        // Global imgFiles property for use in tests. 
        private const IFormFileCollection ImgFilesNull = null;

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
        /// Tests that when OnGet is called on a valid neighborhood, the ModelState is valid and 
        /// the selected neighborhood is returned.
        /// </summary>
        [Test]
        public void OnGet_Valid_Should_Return_True_And_Return_Correct_Neighborhood()
        {
            // Arrange

            // Add test neighborhood to database and store test neighborhood.
            TestHelper.NeighborhoodServiceObj.AddData(Name, Address, Image, ShortDesc, ImgFilesNull);
            var testNeighborhood = _neighborhoodService.GetNeighborhoods().Last();

            // Act
            _pageModel.OnGet(testNeighborhood.id);

            // Assert
            Assert.AreEqual(true, _pageModel.ModelState.IsValid);
            Assert.AreEqual(testNeighborhood.name, _pageModel.neighborhood.name);

            // TearDown
            TestHelper.NeighborhoodServiceObj.DeleteData(testNeighborhood.id);
        }

        /// <summary>
        /// Tests that when OnGet is called, an invalid ModelState will return false and 
        /// redirect page to Index.
        /// </summary>
        [Test]
        public void OnGet_Invalid_ModelState_Should_Return_False_And_Redirect_To_Index()
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
            var result = _pageModel.OnGet(_pageModel.neighborhood.id) as RedirectToPageResult;

            // Assert
            Assert.AreEqual(false, _pageModel.ModelState.IsValid);
            Assert.AreEqual(true, result.PageName.Contains("Index"));
        }

        /// <summary>
        /// Tests that when OnGet is called, a valid ModelState with a null neighborhoood 
        /// will cause page to redirect to Index.
        /// </summary>
        [Test]
        public void OnGet_Valid_ModelState_Null_Neighborhood_Should_Redirect_To_Index()
        {
            // Arrange
            _pageModel.neighborhood = new NeighborhoodModel
            {
                id = 666,
                name = "Invalid Name",
                shortDesc = "Invalid Desc"
            };

            // Act
            var result = _pageModel.OnGet(_pageModel.neighborhood.id) as RedirectToPageResult;

            // Assert
            Assert.AreEqual(true, _pageModel.ModelState.IsValid);
            Assert.AreEqual(true, result.PageName.Contains("Index"));
        }

        #endregion OnGet

        #region OnPost
        /// <summary>
        /// Tests that when OnPost is called on a valid neighborhood, the ModeState is valid, 
        /// the record is deleted from the database and the page is redirected to index.
        /// </summary>
        [Test]
        public void OnPost_Valid_Neighborhood_Should_Return_Neighborhoods()
        {
            // Arrange

            // Add test neighborhood to database and store test neighborhood.
            _neighborhoodService.AddData(Name, Address, Image, ShortDesc, ImgFilesNull);
            _pageModel.neighborhood = _neighborhoodService.GetNeighborhoods().Last();

            // Act
            var result = _pageModel.OnPost() as RedirectToPageResult;

            // Assert model state is valid and and page is redirected.
            Assert.AreEqual(true, _pageModel.ModelState.IsValid);
            Debug.Assert(result != null, nameof(result) + " != null");
            Assert.AreEqual(true, result.PageName.Contains("Index"));

            // Confirm the item is deleted.
            Assert.AreEqual(null, _neighborhoodService.GetNeighborhoodById(_pageModel.neighborhood.id));

        }

        /// <summary>
        /// Test that when OnPost is called, an invalid ModelState should return false 
        /// redirect page to Index.
        /// </summary>
        [Test]
        public void OnPost_Invalid_ModelState_Should_Return_False()
        {
            // Arrange

            // Force an invalid error state.
            _pageModel.ModelState.AddModelError("InvalidState", "Neighborhood is invalid");

            // Act
            var result = _pageModel.OnPost() as RedirectToPageResult;

            // Assert
            Assert.AreEqual(false, _pageModel.ModelState.IsValid);
            Assert.AreEqual(true, result.PageName.Contains("Index"));
        }

        #endregion OnPostAsync
    }
}
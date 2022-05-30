using LetsGoSEA.WebSite.Models;
using LetsGoSEA.WebSite.Pages.Neighborhood;
using LetsGoSEA.WebSite.Services;
using Microsoft.AspNetCore.Http;
using NUnit.Framework;
using System.Linq;

namespace UnitTests.Pages.Neighborhood
{
    /// <summary>
    /// Unit test for the Read Page.
    /// </summary>
    public class ReadTests
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

        // ReadModel object.
        private static ReadModel _pageModel;

        /// <summary>
        /// Initialize ReadModel with a NeighborhoodService object. 
        /// </summary>
        [SetUp]
        public void TestInitialize()
        {
            // Abstract NeighborhoodService object from TestHelper.
            _neighborhoodService = TestHelper.NeighborhoodServiceObj;

            // Initialize pageModel.
            _pageModel = new ReadModel(TestHelper.NeighborhoodServiceObj);
        }

        #endregion TestSetup

        #region OnGet
        /// <summary>
        /// Tests that when OnGet is called, the selected neighborhood is returned.
        /// </summary>
        [Test]
        public void OnGet_Valid_Should_Return_True()
        {

            // Arrange

            // Add test neighborhood to database.
            _neighborhoodService.AddData(Name, Address, Image, ShortDesc, ImgFilesNull);

            // Retrieve test neighborhood.
            var testNeighborhood = _neighborhoodService.GetNeighborhoods().Last();

            // Act
            var result = _pageModel.OnGet(testNeighborhood.id);

            // Assert
            Assert.AreEqual(true, _pageModel.ModelState.IsValid);
            Assert.AreEqual(testNeighborhood.name, _pageModel.neighborhood.name);

            // TearDown
            TestHelper.NeighborhoodServiceObj.DeleteData(testNeighborhood.id);

        }

        /// <summary>
        /// Tests that when OnGet is called on an invalid neighborhood, the 
        /// Model state becomes invalid.
        /// </summary>
        [Test]
        public void OnGet_Invalid_Should_Return_False()
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
            var result = _pageModel.OnGet(_pageModel.neighborhood.id);

            // Assert
            Assert.AreEqual(false, _pageModel.ModelState.IsValid);

        }

        /// <summary>
        /// Tests that when OnGet is called on an invalid neighborhood, the 
        /// Model state becomes invalid.
        /// </summary>
        [Test]
        public void OnGet_Null_Should_Return_False()
        {
            /*
            // Arrange

            // Act
            _pageModel.neighborhood = null;

            // Force an invalid error state.
            _pageModel.ModelState.AddModelError("InvalidState", "Neighborhood is invalid");

            var result = _pageModel.OnGet(_pageModel.neighborhood.id) as RedirectToPageResult;

            // Assert
            Assert.AreEqual(false, _pageModel.ModelState.IsValid);
            //Assert.AreEqual(true, result.PageName.Contains("Index"));
            Assert.IsInstanceOf(typeof(RedirectToPageResult), result);
            */
        }

        #endregion OnGet

    }
}
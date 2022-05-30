using LetsGoSEA.WebSite.Pages.Neighborhood;
using LetsGoSEA.WebSite.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace UnitTests.Pages.Neighborhood
{
    /// <summary>
    /// Unit test for the Create Page. 
    /// </summary>
    public class CreateTests
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

        // Global NeighborhoodService to use for all test cases. 
        private NeighborhoodService _neighborhoodService;

        // CreateModel object 
        private static CreateModel _pageModel;

        /// <summary>
        /// Initialize CreateModel private field. 
        /// </summary>
        [SetUp]
        public void TestInitialize()
        {
            // Abstract NeighborhoodService object from TestHelper.
            _neighborhoodService = TestHelper.NeighborhoodServiceObj;

            // Initialize pageModel.
            _pageModel = new CreateModel(_neighborhoodService)
            {
                PageContext = TestHelper.InitiatePageContext()
            };
        }

        #endregion TestSetup

        #region TestSetupHelper
        /// <summary>
        /// Creates a mock form collection with mock data.
        /// </summary>
        /// <param name="id">The ID of the neighborhood to save mock data to.</param>
        /// <returns></returns>
        private FormCollection GetMockFormCollection(int id)
        {
            // Store mock user input in String arrays to match FormCollection Value format.
            string[] idArray = { id.ToString() };
            string[] nameArray = { Name };
            string[] imageArray = { Image };
            string[] addressArray = { Address };
            string[] shortDescArray = { ShortDesc };

            // Create a FormCollection object to hold mock form data.
            var formCol = new FormCollection(new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>
            {
                { "Neighborhood.Id", idArray},
                { "Neighborhood.Name", nameArray },
                { "Neighborhood.Image", imageArray},
                {"Neighborhood.Address", addressArray},
                { "Neighborhood.ShortDesc", shortDescArray}
            });

            return formCol;
        }
        #endregion TestSetupHelper

        #region OnPost
        /// <summary>
        /// Tests that when OnPost is called, the Create page gathers Form input and 
        /// successfully creates a new Neighborhood card with the correct information.
        /// </summary>
        [Test]
        public void OnPost_Valid_Should_Collect_Form_Input_And_Create_New_Neighborhood()
        {

            // Arrange

            // Generate new ID for test neighborhood. 
            var oldNeighborhoodCount = _neighborhoodService.GetNeighborhoods().Count();
            var newId = oldNeighborhoodCount + 1;

            // Link FormCollection object with HTTPContext.
            TestHelper.HttpContextDefault.Request.HttpContext.Request.Form = GetMockFormCollection(newId);

            // Act
            var result = _pageModel.OnPost() as RedirectToPageResult;

            // Assert a new test neighborhood was created with correct data.
            Assert.AreEqual(oldNeighborhoodCount + 1, _neighborhoodService.GetNeighborhoods().Count());
            Assert.AreEqual(Name, _neighborhoodService.GetNeighborhoods().Last().name);
            Assert.AreEqual(Image, _neighborhoodService.GetNeighborhoods().Last().image);
            Assert.AreEqual(Address, _neighborhoodService.GetNeighborhoods().Last().address);
            Assert.AreEqual(ShortDesc, _neighborhoodService.GetNeighborhoods().Last().shortDesc);
        }

        /// <summary>
        /// Tests when OnPost is called, a valid Model State should return true and redirect to Index page.
        /// </summary>
        [Test]
        public void OnPost_Valid_Model_State_Should_Return_True_And_Redirect_To_Index()
        {

            // Arrange

            // Generate new ID for test neighborhood. 
            var oldNeighborhoodCount = _neighborhoodService.GetNeighborhoods().Count();
            var newId = oldNeighborhoodCount + 1;

            // Link FormCollection object with HTTPContext.
            TestHelper.HttpContextDefault.Request.HttpContext.Request.Form = GetMockFormCollection(newId);

            // Act
            var result = _pageModel.OnPost() as RedirectToPageResult;

            // Assert page is successful.
            Assert.AreEqual(true, _pageModel.ModelState.IsValid);
            Assert.AreEqual(true, result.PageName.Contains("Index"));
        }

        /// <summary>
        /// Tests when OnPost is called, an invalid Model State should return false and redirect to Index.
        /// </summary>
        [Test]
        public void OnPost_InValid_ModelState_Should_Return_False_and_Redirect_To_Index()
        {
            // Arrange

            // Force an invalid error state
            _pageModel.ModelState.AddModelError("InvalidState", "Invalid Neighborhood state");

            // Act
            var result = _pageModel.OnPost() as RedirectToPageResult;

            // Assert
            Assert.AreEqual(false, _pageModel.ModelState.IsValid);
            Assert.AreEqual(true, result.PageName.Contains("Index"));
        }

        #endregion OnPost
    }
}
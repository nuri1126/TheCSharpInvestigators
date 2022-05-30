using LetsGoSEA.WebSite.Models;
using LetsGoSEA.WebSite.Pages.Neighborhood;
using LetsGoSEA.WebSite.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
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
        private static readonly string Image = "https://via.placeholder.com/150";
        
        // Global valid address property for use in tests
        private static readonly string Address = "401 NE Northgate Way, Seattle, WA 98125";

        // Global valid shortDesc property for use in tests.
        private static readonly string ShortDesc = "Test neighborhood description";

        // Global imgFiles property for use in tests. 
        private static readonly IFormFileCollection ImgFilesNull = null;

        // Global NeighborhoodService to use for all test cases. 
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
            _pageModel = new UpdateModel(_neighborhoodService)
            {
                PageContext = TestHelper.PageContext
            };
        }
        #endregion TestSetup

        #region TestSetup Helper 
        /// <summary>
        /// Global mock FormFileCollection generator creates ImagePath neighborhood
        /// property for use in Images region. 
        /// </summary>
        /// <param name="testImageFiles">A dictionary of test image files, K = image file name, V = image file content</param>
        private FormFileCollection GetImagePath(Dictionary<string, string> testImageFiles)
        {
            // Create a FormFileCollection.
            var imageFiles = new FormFileCollection();

            foreach (KeyValuePair<string, string> testImageFile in testImageFiles)
            {
                // Setup mock file using a memory stream.
                var imageFileNames = testImageFile.Key;
                var content = testImageFile.Value;
                var stream = new MemoryStream();
                var writer = new StreamWriter(stream);
                writer.Write(content);
                writer.Flush();
                stream.Position = 0;

                // Create FormFile with desired data.
                IFormFile file = new FormFile(stream, 0, stream.Length, "id_from_form", imageFileNames);
                imageFiles.Add(file);
            }

            return imageFiles;
        }
        #endregion TestSetup Helper 

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
            _neighborhoodService.AddData(Name, Address, Image, ShortDesc, ImgFilesNull);

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

            // Add test neighborhood to database.
            _neighborhoodService.AddData(Name, Address, Image, ShortDesc, ImgFilesNull);

            // Retrieve test neighborhood.
            var testNeighborhood = _neighborhoodService.GetNeighborhoods().Last();

            // Update test neighborhood's name and short description.
            _pageModel.neighborhood = new NeighborhoodModel
            {
                id = testNeighborhood.id,
                name = "New_Bogusland",
                shortDesc = "New_Test neighborhood description",
            };

            // Update test neighborhood by uploading test images.
            // Set up mock test-image files.
            var testImageFiles = new Dictionary<string, string>()
            {
                { "testImage.jpg", "test image content" },
            };

            // Get mock test-image paths.
            var imagePaths = GetImagePath(testImageFiles);

            // Create a FormCollection object to hold mock image paths.
            var formCol = new FormCollection(null, imagePaths);

            // Link FormCollection object with HTTPContext.
            TestHelper.HttpContextDefault.Request.HttpContext.Request.Form = formCol;


            // Act
            var result = _pageModel.OnPost() as RedirectToPageResult;

            // Assert page is successful.
            Assert.AreEqual(true, _pageModel.ModelState.IsValid);
            Assert.AreEqual(true, result != null && result.PageName.Contains("Index"));

            // Assert test neighborhood was updated with correct data.
            Assert.AreEqual("New_Bogusland", _neighborhoodService.GetNeighborhoods().Last().name);
            Assert.AreEqual("New_Test neighborhood description", _neighborhoodService.GetNeighborhoods().Last().shortDesc);
            Assert.AreEqual("image/Neighborhood/testImage.jpg", _neighborhoodService.GetNeighborhoods().Last().imagePath);

            // TearDown
            TestHelper.NeighborhoodServiceObj.DeleteData(testNeighborhood.id);
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
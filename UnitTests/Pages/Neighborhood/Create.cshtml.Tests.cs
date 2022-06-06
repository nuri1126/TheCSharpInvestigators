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
    /// Unit test for the Create Page. 
    /// </summary>
    public class CreateTests
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
        /// <param name="id">The ID of the neighborhood to save mock data to</param>
        /// <param name="imageFilesToUpload">The image files to upload</param>
        /// <returns>A FormCollection object holding a collection of form data</returns>
        private FormCollection GetMockFormCollection(int id, FormFileCollection imageFilesToUpload)
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
            }, imageFilesToUpload);
            return formCol;
        }

        /// <summary>
        /// Helper method to simulate form action of NOT uploading any new image.
        /// </summary>
        /// <returns>an empty FormFileCollection with count = 0</returns>
        private FormFileCollection UploadNoNewImages()
        {
            // Set up mock test-image files.
            var testImageFiles = new Dictionary<string, string>()
            {
            };
            // Get mock test-image paths.
            var noImageToUpload = GetImageFileCollection(testImageFiles);
            return noImageToUpload;
        }

        /// <summary>
        /// Helper method to mock user form input of uploading two new images. 
        /// </summary>
        /// <returns>a FormFileCollection object containing all the image files</returns>
        private FormFileCollection UploadTwoNewImage()
        {
            // Set up mock test-image files.
            var testImageFiles = new Dictionary<string, string>()
            {
                { "testImage_A.jpg", "test image 1 content" },
                { "testImage_B.jpg", "test image 2 content" },
            };
            // Convert mock files to image file collection.
            var imagesToUpload = GetImageFileCollection(testImageFiles);
            return imagesToUpload;
        }

        /// <summary>
        /// Global mock FormFileCollection generator converts mock image files into a FormFileCollection.
        /// </summary>
        /// <param name="testImageFiles">A dictionary of test image files, K = image file name, V = image file content</param>
        /// <returns>a FormFileCollection object containing all the image files</returns>
        private static FormFileCollection GetImageFileCollection(Dictionary<string, string> testImageFiles)
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

        #endregion TestSetupHelper

        #region OnPost

        /// <summary>
        /// Tests that when OnPost is called, the Create page gathers Form input and 
        /// successfully creates a new Neighborhood card with the correct information.
        /// In this test, NO image files are uploaded by user. 
        /// </summary>
        [Test]
        public void OnPost_Valid_Textbox_Input_No_UploadImage_Should_Create_New_Neighborhood()
        {
            // Arrange
            // Generate new ID for test neighborhood. 
            var prevNeighborhoodCount = _neighborhoodService.GetNeighborhoods().Count();
            var nextId = prevNeighborhoodCount + 1;
            // Simulate form action: collect textbox data with no new image uploaded.
            var formCol = GetMockFormCollection(nextId, UploadNoNewImages());
            // Link FormCollection object with HTTPContext.
            TestHelper.HttpContextDefault.Request.HttpContext.Request.Form = formCol;
            // Act
            _ = _pageModel.OnPost() as RedirectToPageResult;
            // Get the new test neighborhood we just created.
            var newNeighborhood = _neighborhoodService.GetNeighborhoods().Last();
            // Assert a new test neighborhood was created with correct data and no uploaded image.
            Assert.AreEqual(prevNeighborhoodCount + 1, _neighborhoodService.GetNeighborhoods().Count());
            Assert.AreEqual(Name, newNeighborhood.name);
            Assert.AreEqual(Image, newNeighborhood.image);
            Assert.AreEqual(Address, newNeighborhood.address);
            Assert.AreEqual(ShortDesc, newNeighborhood.shortDesc);
            Assert.AreEqual(0, newNeighborhood.uploadedImages.Count());
        }

        /// <summary>
        /// Tests that when OnPost is called, the Create page gathers Form input and 
        /// successfully creates a new Neighborhood card with the correct information.
        /// In this test, TWO image files are uploaded by user. 
        /// </summary>
        [Test]
        public void OnPost_Valid_Textbox_Input_Two_UploadImages_Should_Create_New_Neighborhood()
        {
            // Arrange
            // Generate new ID for test neighborhood. 
            var prevNeighborhoodCount = _neighborhoodService.GetNeighborhoods().Count();
            var nextId = prevNeighborhoodCount + 1;
            // Simulate form action: collect textbox data with no new image uploaded.
            var formCol = GetMockFormCollection(nextId, UploadTwoNewImage());
            // Link FormCollection object with HTTPContext.
            TestHelper.HttpContextDefault.Request.HttpContext.Request.Form = formCol;
            // Act
            _ = _pageModel.OnPost() as RedirectToPageResult;
            // Get the new test neighborhood we just created.
            var newNeighborhood = _neighborhoodService.GetNeighborhoods().Last();
            // Assert a new test neighborhood was created with correct data and two new uploaded image.
            Assert.AreEqual(prevNeighborhoodCount + 1, _neighborhoodService.GetNeighborhoods().Count());
            Assert.AreEqual(Name, newNeighborhood.name);
            Assert.AreEqual(Image, newNeighborhood.image);
            Assert.AreEqual(Address, newNeighborhood.address);
            Assert.AreEqual(ShortDesc, newNeighborhood.shortDesc);
            Assert.AreEqual("testImage_A.jpg", newNeighborhood.uploadedImages.First().UploadedImageName);
            Assert.AreEqual("testImage_B.jpg", newNeighborhood.uploadedImages.Last().UploadedImageName);
        }

        /// <summary>
        /// Tests when OnPost is called, creating a new neighborhood should return valid ModelState
        /// and redirect to Index page.
        /// </summary>
        [Test]
        public void OnPost_Valid_Should_Return_Valid_Model_State_And_Redirect_To_Index()
        {
            // Arrange
            // Generate new ID for test neighborhood. 
            var prevNeighborhoodCount = _neighborhoodService.GetNeighborhoods().Count();
            var nextId = prevNeighborhoodCount + 1;
            // Simulate form action: collect textbox data with no new image uploaded.
            var formCol = GetMockFormCollection(nextId, UploadNoNewImages());
            // Link FormCollection object with HTTPContext.
            TestHelper.HttpContextDefault.Request.HttpContext.Request.Form = formCol;
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
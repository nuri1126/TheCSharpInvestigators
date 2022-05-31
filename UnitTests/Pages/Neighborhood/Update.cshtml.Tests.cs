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
        /// Initialize UpdateModel with a NeighborhoodService object. 
        /// </summary>
        [SetUp]
        public void TestInitialize()
        {

            // Abstract NeighborhoodService object from TestHelper.
            _neighborhoodService = TestHelper.NeighborhoodServiceObj;

            // Initialize pageModel.
            _pageModel = new UpdateModel(_neighborhoodService)
            {
                PageContext = TestHelper.InitiatePageContext()
            };
        }
        #endregion TestSetup

        #region TestSetup Helper 
        /// <summary>
        /// This method simulates the abstraction of user input data from the Update Razor Page 
        /// and use those data to update the neighborhood object on the Update Model page.
        /// </summary>
        /// <param name="oldNeighborhood"></param>
        private void UpdateNeighborhoodWithTestData(NeighborhoodModel oldNeighborhood)
        {
            oldNeighborhood.name = "New_Bogusland";
            oldNeighborhood.address = "New_BogusAddress";
            oldNeighborhood.image = "https://via.placeholder.com/99";
            oldNeighborhood.shortDesc = "New_Test neighborhood description";
        }

        /// <summary>
        /// This method updates the neighborhood object with new ImagePath by simulating upload image action.
        /// </summary>
        private void UpdateNeighborhoodWithTestFileImage()
        {
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
        }

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
        /// Test that when OnGet is called, a valid Model State should return true 
        /// and all neighborhoods in the database are returned.
        /// </summary>
        [Test]
        public void OnGet_Valid_Should_Return_True_And_Return_All_Neighborhoods()
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

        /// <summary>
        /// Tests when OnGet is called, an invalid ModelState should return false and redirect to Index page.
        /// </summary>
        [Test]
        public void OnGet_InValid_ModelState_Should_Return_False_And_Redirect_To_Index()
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
            var result = _pageModel.OnGet(_pageModel.neighborhood.id) as RedirectToPageResult;

            // Assert
            Assert.AreEqual(false, _pageModel.ModelState.IsValid);
            Assert.AreEqual(true, result.PageName.Contains("Index"));
        }

        /// <summary>
        /// Tests when OnGet is called, a valid Model State with a null neighborhood should redirect to Index page.
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
        /// Test when a neighborhood is updated and OnPost is called, the
        /// neighborhood is returned with the updates in effect.
        /// </summary>
        [Test]
        public void OnPost_Valid_Should_Update_Neighborhood_With_New_Data()
        {
            // Arrange

            // Add test neighborhood to database.
            _neighborhoodService.AddData(Name, Address, Image, ShortDesc, ImgFilesNull);

            // Retrieve test neighborhood.
            _pageModel.neighborhood = _neighborhoodService.GetNeighborhoods().Last();

            // UPDATE TEST NEIGHBORHOOD with new name, address, image, and short description.
            UpdateNeighborhoodWithTestData(_pageModel.neighborhood);

            // UPDATE TEST NEIGHBORHOOD by uploading a test image.
            UpdateNeighborhoodWithTestFileImage();

             // Act
             var result = _pageModel.OnPost() as RedirectToPageResult;

            // Assert test neighborhood was updated with correct data.
            Assert.AreEqual("New_Bogusland", _neighborhoodService.GetNeighborhoods().Last().name);
            Assert.AreEqual("New_BogusAddress", _neighborhoodService.GetNeighborhoods().Last().address);
            Assert.AreEqual("https://via.placeholder.com/99", _neighborhoodService.GetNeighborhoods().Last().image);
            Assert.AreEqual("New_Test neighborhood description", _neighborhoodService.GetNeighborhoods().Last().shortDesc);
            Assert.AreEqual("image/Neighborhood/testImage.jpg", _neighborhoodService.GetNeighborhoods().Last().imagePath);

         
            // TearDown
            TestHelper.NeighborhoodServiceObj.DeleteData(_pageModel.neighborhood.id);
        }

        /// <summary>
        /// Test when a neighborhood is updated and OnPost is called, the Model State is valid and page is redirected to Index 
        /// upon finishing.
        /// </summary>
        [Test]
        public void OnPost_Valid_ModelState_Should_Return_True_And_Redirect_To_Index()
        {
            // Arrange

            // Add test neighborhood to database.
            _neighborhoodService.AddData(Name, Address, Image, ShortDesc, ImgFilesNull);

            // Retrieve test neighborhood.
            _pageModel.neighborhood = _neighborhoodService.GetNeighborhoods().Last();

            // UPDATE TEST NEIGHBORHOOD with new name, address, image, and short description.
            UpdateNeighborhoodWithTestData(_pageModel.neighborhood);

            // UPDATE TEST NEIGHBORHOOD by uploading a test image.
            UpdateNeighborhoodWithTestFileImage();

            // Act
            var result = _pageModel.OnPost() as RedirectToPageResult;

            // Assert page is successful.
            Assert.AreEqual(true, _pageModel.ModelState.IsValid);
            Assert.AreEqual(true, result.PageName.Contains("Index"));

            // TearDown
            TestHelper.NeighborhoodServiceObj.DeleteData(_pageModel.neighborhood.id);
        }

        /// <summary>
        /// Tests when OnPost is called, an invalid ModelState should return false and redirect to Index.
        /// </summary>
        [Test]
        public void OnPost_InValid_ModelState_Should_Return_False_And_Redirect_To_Index()
        {
            // Arrange

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
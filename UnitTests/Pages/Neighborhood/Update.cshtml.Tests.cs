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
        private const string Name = "Bogusland";

        // Global valid image property for use in tests. 
        private const string Image = "https://via.placeholder.com/150";

        // Global valid address property for use in tests
        private const string Address = "401 NE Northgate Way, Seattle, WA 98125";

        // Global valid shortDesc property for use in tests.
        private const string ShortDesc = "Test neighborhood description";

        // Global imgFiles property for use in tests. 
        private const IFormFileCollection ImgFilesNull = null;

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
        /// This method simulates the abstraction of user input data from the Update Page 
        /// and use those data to update the neighborhood object on the Update Model page.
        /// </summary>
        /// <param name="oldNeighborhood">Neighborhood to update</param>
        private static void UpdateNeighborhoodWithTestData(NeighborhoodModel oldNeighborhood)
        {
            oldNeighborhood.name = "New_Bogusland";
            oldNeighborhood.address = "New_BogusAddress";
            oldNeighborhood.image = "https://via.placeholder.com/99";
            oldNeighborhood.shortDesc = "New_Test neighborhood description";
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

            // Add test neighborhood to database and store as testNeighborhood.
            _neighborhoodService.AddData(Name, Address, Image, ShortDesc);
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

            // Force an invalid error state.
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
        /// Test Update OnPost method: when user enters VALID TEXTBOX INPUT for a valid neighborhood
        /// (WITHOUT selecting any image file to delete and WITHOUT uploading any new image), the
        /// neighborhood is updated with new textbox data in effect and no change to uploaded image count.
        /// </summary>
        [Test]
        public void OnPost_Valid_Neighborhood_Valid_Textbox_Input_Should_Update_Neighborhood_With_New_Data()
        {
            // Arrange

            // Add test neighborhood to database.
            _neighborhoodService.AddData(Name, Address, Image, ShortDesc);

            // Retrieve test neighborhood and its uploaded image count.
            _pageModel.neighborhood = _neighborhoodService.GetNeighborhoods().Last();
            var oldUploadedImageCount = _pageModel.neighborhood.uploadedImages.Count();

            // Update testNeighborhood with new name, address, image, and short description.
            UpdateNeighborhoodWithTestData(_pageModel.neighborhood);

            // Simulate form action: no image files are deleted or added.
            var formCol = new FormCollection(new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>
            {
                { "DeleteFile", DeleteNoImageFile() }
            }, UploadNoNewImages());

            // Link FormCollection object with HTTPContext.
            TestHelper.HttpContextDefault.Request.HttpContext.Request.Form = formCol;

            // Act
            var result = _pageModel.OnPost() as RedirectToPageResult;

            // Retrieve test neighborhood again.
            var updatedNeighborhood = _neighborhoodService.GetNeighborhoods().Last();

            // Assert test neighborhood was updated with correct data.
            Assert.AreEqual("New_Bogusland", updatedNeighborhood.name);
            Assert.AreEqual("New_BogusAddress", updatedNeighborhood.address);
            Assert.AreEqual("https://via.placeholder.com/99", updatedNeighborhood.image);
            Assert.AreEqual("New_Test neighborhood description", updatedNeighborhood.shortDesc);
            Assert.AreEqual(oldUploadedImageCount, updatedNeighborhood.uploadedImages.Count());

            // TearDown
            TestHelper.NeighborhoodServiceObj.DeleteData(_pageModel.neighborhood.id);
        }

        /// <summary>
        /// Test Update OnPost method: when user deletes certain uploaded images for a valid neighborhood
        /// (WITHOUT updating any textbox data and WITHOUT uploading any new image), the neighborhood is updated
        /// with selected uploaded images removed and no new images added.
        /// </summary>
        [Test]
        public void OnPost_Valid_Neighborhood_Valid_DeleteImageId_Should_Delete_Uploaded_Images()
        {
            // Arrange

            // Add test neighborhood to database.
            _neighborhoodService.AddData(Name, Address, Image, ShortDesc);

            // Retrieve test neighborhood.
            _pageModel.neighborhood = _neighborhoodService.GetNeighborhoods().Last();

            // Add test image files to test neighborhood. 
            /* NOTE: We are NOT using mock Form to add these test images.
             * We are using UploadeImageIfAvailable() method in NeighborhoodService.cs 
             * to DIRECTLY add test images to our test neighborhood.
             */
            var testImageFile = new Dictionary<string, string>()
            {
                { "testImage_1.jpg", "test image 1 content" },
                { "testImage_2.jpg", "test image 2 content" },
                { "testImage_3.jpg", "test image 2 content" },
            };
            var imagePath = GetImageFileCollection(testImageFile);
            _neighborhoodService.UploadImageIfAvailable(_pageModel.neighborhood, imagePath);

            // Simulate form action: Delete first and third uploaded images, upload no new image.
            var formCol = new FormCollection(new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>
            {
                { "DeleteFile", DeleteFirstAndThirdImageFile() }
                }, UploadNoNewImages());

            // Link FormCollection object with HTTPContext.
            TestHelper.HttpContextDefault.Request.HttpContext.Request.Form = formCol;

            // Act
            var result = _pageModel.OnPost() as RedirectToPageResult;

            // Retrieve test neighborhood again.
            var updatedNeighborhood = _neighborhoodService.GetNeighborhoods().Last();

            // Assert FIRST AND THIRD uploaded images have been successfully deleted and only 1 image is left.
            Assert.AreEqual("testImage_2.jpg", updatedNeighborhood.uploadedImages.First().UploadedImageName);
            Assert.AreEqual(1, updatedNeighborhood.uploadedImages.Count());

            // TearDown
            TestHelper.NeighborhoodServiceObj.DeleteData(_pageModel.neighborhood.id);
        }

        /// <summary>
        /// Test Update OnPost method: when user uploads new image files for a valid neighborhood
        /// (WITHOUT updating any textbox data and WITHOUT deleting any image file), the neighborhood is updated
        /// with new uploaded images with correct count.
        /// </summary>
        [Test]
        public void OnPost_Valid_Neighborhood_Valid_NewUpload_Should_Add_Uploaded_Images()
        {
            // Arrange

            // Add test neighborhood to database.
            _neighborhoodService.AddData(Name, Address, Image, ShortDesc);

            // Retrieve test neighborhood.
            _pageModel.neighborhood = _neighborhoodService.GetNeighborhoods().Last();

            // Simulate form action: upload two new image files, delete none.
            var formCol = new FormCollection(new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>
            {
                { "DeleteFile", DeleteNoImageFile() }
            }, UploadTwoNewImage());

            // Link FormCollection object with HTTPContext.
            TestHelper.HttpContextDefault.Request.HttpContext.Request.Form = formCol;

            // Act
            var result = _pageModel.OnPost() as RedirectToPageResult;

            // Retrieve test neighborhood again.
            var updatedNeighborhood = _neighborhoodService.GetNeighborhoods().Last();

            // Assert correct image files are uploaded with correct count.
            Assert.AreEqual("testImage_A.jpg", updatedNeighborhood.uploadedImages.First().UploadedImageName);
            Assert.AreEqual("testImage_B.jpg", updatedNeighborhood.uploadedImages.Last().UploadedImageName);
            Assert.AreEqual(2, _neighborhoodService.GetNeighborhoods().Last().uploadedImages.Count());

            // TearDown
            TestHelper.NeighborhoodServiceObj.DeleteData(_pageModel.neighborhood.id);
        }

        /// <summary>
        /// Test Update OnPost: when no change is made to a valid neighborhood, the Model State is still valid and page is redirected to Index 
        /// upon finishing.
        /// </summary>
        [Test]
        public void OnPost_Valid_Neighborhood_No_Change_Should_Return_Valid_Model_State_And_Redirect_To_Index()
        {
            // Arrange

            // Add test neighborhood to database.
            _neighborhoodService.AddData(Name, Address, Image, ShortDesc);

            // Retrieve test neighborhood.
            _pageModel.neighborhood = _neighborhoodService.GetNeighborhoods().Last();

            // Simulate form action: no image files are deleted or added.
            var formCol = new FormCollection(new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>
            {
                { "DeleteFile", DeleteNoImageFile() }
            }, UploadNoNewImages());

            // Link FormCollection object with HTTPContext.
            TestHelper.HttpContextDefault.Request.HttpContext.Request.Form = formCol;

            // Act
            var result = _pageModel.OnPost() as RedirectToPageResult;

            // Assert page is successful.
            Assert.AreEqual(true, _pageModel.ModelState.IsValid);
            Assert.AreEqual(true, result.PageName.Contains("Index"));

            // TearDown
            TestHelper.NeighborhoodServiceObj.DeleteData(_pageModel.neighborhood.id);
        }

        /// <summary>
        /// Test Update OnPost: a valid Model state with a NULL neighborhood should redirect page to Index. 
        /// </summary>
        [Test]
        public void OnPost_Null_Neighborhood_Valid_ModelState_Should_Redirect_To_Index()
        {
            // Arrange a null neighborhood.
            _pageModel.neighborhood = null;

            // Form action does not matter here.
            var formCol = new FormCollection(new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>
            {
                { "DeleteFile", DeleteNoImageFile() }
            }, UploadNoNewImages());

            // Link FormCollection object with HTTPContext.
            TestHelper.HttpContextDefault.Request.HttpContext.Request.Form = formCol;

            // Act
            var result = _pageModel.OnPost() as RedirectToPageResult;

            // Assert page is successful.
            Assert.AreEqual(true, _pageModel.ModelState.IsValid);
            Assert.AreEqual(true, result.PageName.Contains("Index"));
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
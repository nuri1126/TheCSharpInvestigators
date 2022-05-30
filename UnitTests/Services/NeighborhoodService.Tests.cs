using LetsGoSEA.WebSite.Models;
using LetsGoSEA.WebSite.Services;
using Microsoft.AspNetCore.Http;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace UnitTests.Services
{
    /// <summary>
    /// NeighborhoodServiceTests Tests.
    /// </summary>
    public class NeighborhoodServiceTests
    {
        // Global invalid id property for use in tests. 
        private static readonly int InvalidId = -1;

        // Global valid name property for use in tests. 
        private static readonly string Name = "Bogusland";
        
        // Global valid address property for use in tests
        private static readonly string Address = "401 NE Northgate Way, Seattle, WA 98125";

        // Global valid image property for use in tests. 
        private static readonly string Image = "http://via.placeholder.com/150";

        // Global valid shortDesc property for use in tests.
        private static readonly string ShortDesc = "Test neighborhood description";

        // Global imgFiles property for use in tests. 
        private static readonly IFormFileCollection ImgFilesNull = null;

        // Global valid Rating for use in AddRatings region.
        private static readonly int ValidRating = 5;

        // Global valid comment input for use in Comments region.
        private static readonly string ValidComment = "Bogus";

        // Global NeighborhoodService to use for all test cases. 
        private NeighborhoodService _neighborhoodService;

        /// <summary>
        /// Stores the TestHelper's Neighborhood service. 
        /// </summary>
        [SetUp]
        public void TestInitialize()
        {
            // Abstract NeighborhoodService object from TestHelper.
            _neighborhoodService = TestHelper.NeighborhoodServiceObj;
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

            foreach(KeyValuePair<string, string> testImageFile in testImageFiles)
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

        #region GetNeighborhoodData

        /// <summary>
        /// Tests GetNeighborhoods returns not null when called.
        /// </summary>
        [Test]
        public void GetNeighborhoods_Not_Null_Returns_True()
        {
            // Arrange

            // Act
            var result = _neighborhoodService.GetNeighborhoods();

            //Assert
            Assert.NotNull(result);
        }

        /// <summary>
        /// Tests GetNeighborhoods returns an IEnumerable. 
        /// </summary>
        [Test]
        public void GetNeighborhoods_Returns_IEnumerable_Returns_True()
        {
            // Arrange

            // Act
            var result = _neighborhoodService.GetNeighborhoods();

            //Assert
            Assert.IsInstanceOf(typeof(IEnumerable<NeighborhoodModel>), result);
        }

        /// <summary>
        /// Tests GetNeighborhoodById by retrieving the first neighborhood and confirming not null. 
        /// </summary>
        [Test]
        public void GetNeighborhoodById_Valid_Id_Should_Return_True()
        {
            // Arrange

            // Add the test Neighborhood to the database. 
            var testNeighborhood = TestHelper.NeighborhoodServiceObj.AddData(Name, Address, Image, ShortDesc, ImgFilesNull);

            //Act
            var result = _neighborhoodService.GetNeighborhoodById(testNeighborhood.id);

            //Assert
            Assert.NotNull(result);

            // TearDown
            TestHelper.NeighborhoodServiceObj.DeleteData(testNeighborhood.id);
        }

        /// <summary>
        /// Tests GetNeighborhoodById catches out of bounds input and returns null. 
        /// </summary>
        [Test]
        public void GetNeighborhoodById_Invalid_Id_Should_Return_True()
        {
            // Arrange

            //Act
            var invalidResult = _neighborhoodService.GetNeighborhoodById(InvalidId);

            //Assert
            Assert.Null(invalidResult);
        }

        #endregion GetNeighborhoodData

        #region Ratings
        /// <summary>
        /// Tests AddRating, null neighborhood should return false.
        /// </summary>
        [Test]
        public void AddRating_Null_Neighborhood_Should_Return_False()
        {
            // Arrange

            // Act
            var result = _neighborhoodService.AddRating(null, ValidRating);

            // Assert
            Assert.AreEqual(false, result);
        }

        /// <summary>
        /// Test AddRating on Neighborhood with no existing ratings returns true. 
        /// </summary>
        [Test]
        public void AddRating_Valid_Neighborhood_No_Ratings_Returns_True()
        {
            // Arrange

            // Add test neighborhood to database.
            TestHelper.NeighborhoodServiceObj.AddData(Name, Address, Image, ShortDesc, ImgFilesNull);

            // Retrieve test neighborhood.
            var testNeighborhood = _neighborhoodService.GetNeighborhoods().Last();

            // Act
            var result = _neighborhoodService.AddRating(testNeighborhood, ValidRating);

            // Assert
            Assert.AreEqual(true, result);

            // TearDown
            _neighborhoodService.DeleteData(testNeighborhood.id);
        }

        /// <summary>
        /// Test AddRating on Neighborhood with existing ratings returns true. 
        /// </summary>
        [Test]
        public void AddRating_Valid_Neighborhood_Existing_Ratings_Returns_True()
        {
            // Arrange

            // Add test neighborhood to database.
            TestHelper.NeighborhoodServiceObj.AddData(Name, Address, Image, ShortDesc, ImgFilesNull);

            // Retrieve test neighborhood.
            var testNeighborhood = _neighborhoodService.GetNeighborhoods().Last();

            // Add rating. 
            _neighborhoodService.AddRating(testNeighborhood, ValidRating);

            // Act
            var result = _neighborhoodService.AddRating(testNeighborhood, ValidRating);

            // Assert
            Assert.AreEqual(true, result);

            // TearDown
            _neighborhoodService.DeleteData(testNeighborhood.id);
        }

        /// <summary>
        /// Test AddRating on Neighborhood with no existing ratings returns a count of the Ratings
        /// property equal to 1. 
        /// </summary>
        [Test]
        public void AddRating_Valid_Neighborhood_No_Ratings_Count_Equals_1_Returns_True()
        {
            // Arrange

            // Initialize valid test NeighborhoodModel object. 
            // Add test neighborhood to database.
            _neighborhoodService.AddData(Name, Address, Image, ShortDesc, ImgFilesNull);

            // Retrieve test neighborhood.
            var testNeighborhood = _neighborhoodService.GetNeighborhoods().Last();

            // Act
            _neighborhoodService.AddRating(testNeighborhood, ValidRating);
            var result = testNeighborhood.ratings.Count();

            // Assert
            Assert.AreEqual(1, result);

            // TearDown
            _neighborhoodService.DeleteData(testNeighborhood.id);
        }

        /// <summary>
        /// Test AddRating on Neighborhood with existing ratings returns a count of the previous Ratings
        /// property + 1.         
        /// /// </summary>
        [Test]
        public void AddRating_Valid_Neighborhood_Existing_Ratings_Count_Returns_True()
        {
            // Arrange
            // Add test neighborhood to database.
            _neighborhoodService.AddData(Name, Address, Image, ShortDesc, ImgFilesNull);

            // Retrieve test neighborhood.
            var testNeighborhood = _neighborhoodService.GetNeighborhoods().Last();

            // Add rating and store existing rating count. 
            _neighborhoodService.AddRating(testNeighborhood, ValidRating);
            var existingRatingCount = testNeighborhood.ratings.Count();

            // Act
            _neighborhoodService.AddRating(testNeighborhood, ValidRating);
            var result = testNeighborhood.ratings.Count();

            // Assert
            Assert.AreEqual(existingRatingCount + 1, result);

            // TearDown
            _neighborhoodService.DeleteData(testNeighborhood.id);
        }

        /// <summary>
        /// Test lower and upperbounds of AddRating input. Out of bounds input (low and high)
        /// should return false. 
        /// </summary>
        [Test]
        public void AddRating_Out_of_Bounds_Input_Return_False()
        {
            // Arrange

            // Invalid ratings outside of 0 - 5. 
            int[] outOfBoundsRatings = new int[] { -2, -1, 6, 7 };

            // Add test neighborhood to database.
            _neighborhoodService.AddData(Name, Address, Image, ShortDesc, ImgFilesNull);

            // Retrieve test neighborhood.
            var testNeighborhood = TestHelper.NeighborhoodServiceObj.GetNeighborhoods().Last();

            // Act
            var result1 = TestHelper.NeighborhoodServiceObj.AddRating(testNeighborhood, outOfBoundsRatings[0]);
            var result2 = TestHelper.NeighborhoodServiceObj.AddRating(testNeighborhood, outOfBoundsRatings[1]);
            var result3 = TestHelper.NeighborhoodServiceObj.AddRating(testNeighborhood, outOfBoundsRatings[2]);
            var result4 = TestHelper.NeighborhoodServiceObj.AddRating(testNeighborhood, outOfBoundsRatings[3]);

            // Assert
            Assert.AreEqual(false, result1);
            Assert.AreEqual(false, result2);
            Assert.AreEqual(false, result3);
            Assert.AreEqual(false, result4);

            // TearDown
            _neighborhoodService.DeleteData(testNeighborhood.id);
        }

        /// <summary>
        /// Test AddRating input within valid bounds returns true. 
        /// </summary>
        [Test]
        public void AddRating_In_Bounds_Input_Return_True()
        {
            // Arrange

            // Valid ratings from 0-5. 
            var validRatings = new int[] { 0, 1, 2, 3, 4, 5 };

            // Add test neighborhood to database. 
            _neighborhoodService.AddData(Name, Address, Image, ShortDesc, ImgFilesNull);

            // Retrieve test neighborhood.
            var testNeighborhood = _neighborhoodService.GetNeighborhoods().Last();

            // Act
            var result1 = TestHelper.NeighborhoodServiceObj.AddRating(testNeighborhood, validRatings[0]);
            var result2 = TestHelper.NeighborhoodServiceObj.AddRating(testNeighborhood, validRatings[1]);
            var result3 = TestHelper.NeighborhoodServiceObj.AddRating(testNeighborhood, validRatings[2]);
            var result4 = TestHelper.NeighborhoodServiceObj.AddRating(testNeighborhood, validRatings[3]);
            var result5 = TestHelper.NeighborhoodServiceObj.AddRating(testNeighborhood, validRatings[4]);
            var result6 = TestHelper.NeighborhoodServiceObj.AddRating(testNeighborhood, validRatings[5]);

            // Assert
            Assert.AreEqual(true, result1);
            Assert.AreEqual(true, result2);
            Assert.AreEqual(true, result3);
            Assert.AreEqual(true, result4);
            Assert.AreEqual(true, result5);
            Assert.AreEqual(true, result6);

            // TearDown
            _neighborhoodService.DeleteData(testNeighborhood.id);
        }

        #endregion Ratings

        #region Comments

        /// <summary>
        /// Tests AddComment where a valid neighborhood and valid comment return true and update data successfully.
        /// </summary>
        [Test]
        public void AddComment_Valid_Neighborhood_Valid_Comment_Should_Return_True()
        {
            // Arrange

            // Add test neighborhood to database. 
            _neighborhoodService.AddData(Name, Address, Image, ShortDesc, ImgFilesNull);

            // Retrieve test neighborhood.
            var testNeighborhood = _neighborhoodService.GetNeighborhoods().Last();

            // Act
            var result = _neighborhoodService.AddComment(testNeighborhood, ValidComment);

            // Assert
            Assert.AreEqual(true, result);

            // TearDown
            _neighborhoodService.DeleteData(testNeighborhood.id);
        }

        /// <summary>
        /// Test AddComment on Neighborhood with no existing ratings comments a count of the comments
        /// property equal to 1. 
        /// </summary>
        [Test]
        public void AddComment_Valid_Neighborhood_No_Comments_Count_Equals_1_Should_Return_True()
        {
            // Arrange

            // Add test neighborhood to database. 
            _neighborhoodService.AddData(Name, Address, Image, ShortDesc, ImgFilesNull);

            // Retrieve test neighborhood.
            var testNeighborhood = _neighborhoodService.GetNeighborhoods().Last();

            // Act
            _neighborhoodService.AddComment(testNeighborhood, ValidComment);
            var result = testNeighborhood.comments.Count();

            // Assert
            Assert.AreEqual(1, result);

            // TearDown
            _neighborhoodService.DeleteData(testNeighborhood.id);
        }


        /// <summary>
        /// Test AddComments on Neighborhood with existing comments returns a count of the previous
        /// comments property + 1.         
        /// /// </summary>
        [Test]
        public void AddComment_Valid_Neighborhood_Existing_Ratings_Count_Should_Return_True()
        {
            // Arrange
            // Add test neighborhood to database.
            _neighborhoodService.AddData(Name, Address, Image, ShortDesc, ImgFilesNull);

            // Retrieve test neighborhood.
            var testNeighborhood = _neighborhoodService.GetNeighborhoods().Last();

            // Add comment and store existing comment count. 
            _neighborhoodService.AddComment(testNeighborhood, ValidComment);
            var existingCommentCount = testNeighborhood.comments.Count();

            // Act
            _neighborhoodService.AddComment(testNeighborhood, ValidComment);
            var result = testNeighborhood.comments.Count();

            // Assert
            Assert.AreEqual(existingCommentCount + 1, result);

            // TearDown
            _neighborhoodService.DeleteData(testNeighborhood.id);
        }

        /// <summary>
        /// Tests AddComment returns false given a null comment.
        /// </summary>
        [Test]
        public void AddComment_Null_Comment_Should_Return_False()
        {
            // Arrange
            // Add test neighborhood to database.
            _neighborhoodService.AddData(Name, Address, Image, ShortDesc, ImgFilesNull);

            // Retrieve test neighborhood.
            var testNeighborhood = _neighborhoodService.GetNeighborhoods().Last();

            // Act
            var result = _neighborhoodService.AddComment(testNeighborhood, null);

            // Assert
            Assert.AreEqual(false, result);

            // TearDown
            _neighborhoodService.DeleteData(testNeighborhood.id);
        }

        /// <summary>
        /// Tests AddComment returns false given an empty "" comment. 
        /// </summary>
        [Test]
        public void AddComment_Empty_Comment_Should_Return_False()
        {
            // Arrange
            // Add test neighborhood to database.
            _neighborhoodService.AddData(Name, Address, Image, ShortDesc, ImgFilesNull);

            // Retrieve test neighborhood.
            var testNeighborhood = _neighborhoodService.GetNeighborhoods().Last();

            // Act
            var result = _neighborhoodService.AddComment(testNeighborhood, "");

            // Assert
            Assert.AreEqual(false, result);

            // TearDown
            _neighborhoodService.DeleteData(testNeighborhood.id);
        }

        /// <summary>
        /// Tests AddComment returns false given a null neighborhood.  
        /// </summary>
        [Test]
        public void AddComment_Null_Neighborhood_Should_Return_False()
        {
            // Arrange

            // Act
            var result = _neighborhoodService.AddComment(null, ValidComment);

            // Assert
            Assert.AreEqual(false, result);
        }

        /// <summary>
        /// Test DeleteComment returns false given an invalid commentId. 
        /// </summary>
        [Test]
        public void DeleteComment_Null_Neighborhood_Should_Return_False()
        {
            // Arrange

            // Act
            var result = _neighborhoodService.DeleteComment(null, ValidComment);

            // Assert
            Assert.AreEqual(false, result);
        }

        /// <summary>
        /// Tests DeleteComment returns a true given valid input parameters. 
        /// </summary>
        [Test]
        public void DeleteComment_ValidNeighborhood_ValidCommentId_Should_Return_True()
        {
            // Arrange

            // Add test neighborhood to database.
            _neighborhoodService.AddData(Name, Address, Image, ShortDesc, ImgFilesNull);

            // Retrieve test neighborhood.
            var testNeighborhood = _neighborhoodService.GetNeighborhoods().Last();

            // Add valid comment. 
            _neighborhoodService.AddComment(testNeighborhood, ValidComment);

            // Store the commentId of the newly stored comment. 
            var commentId = testNeighborhood.comments.Last().CommentId;

            // Act
            var result = _neighborhoodService.DeleteComment(testNeighborhood, commentId);

            // Assert
            Assert.AreEqual(true, result);
        }

        /// <summary>
        /// Tests that upon DeleteComment, count of comments stored in the neighborhood
        /// has decreased by 1. 
        /// </summary>
        [Test]
        public void DeleteComment_Comments_Count_Decrease_By_1_Should_Return_True()
        {
            // Arrange

            // Add test neighborhood to database.
            _neighborhoodService.AddData(Name, Address, Image, ShortDesc, ImgFilesNull);

            // Retrieve test neighborhood.
            var testNeighborhood = _neighborhoodService.GetNeighborhoods().Last();

            // Add valid comment, store count of comments, store the comment's id. 
            _neighborhoodService.AddComment(testNeighborhood, ValidComment);
            var commentCount = testNeighborhood.comments.Count();
            var commentId = testNeighborhood.comments.Last().CommentId;

            // Act
            _neighborhoodService.DeleteComment(testNeighborhood, commentId);

            // Assert
            Assert.AreEqual(commentCount - 1, testNeighborhood.comments.Count());
        }

        /// <summary>
        /// Tests that DeleteComment returns false when given an invalid commendId. 
        /// </summary>
        [Test]
        public void DeleteComment_InvalidId_Should_Return_False()
        {
            // Arrange

            // Add test neighborhood to database.
            _neighborhoodService.AddData(Name, Address, Image, ShortDesc, ImgFilesNull);

            // Retrieve test neighborhood.
            var testNeighborhood = _neighborhoodService.GetNeighborhoods().Last();

            var validNeighborhood = _neighborhoodService.GetNeighborhoodById(1);
            string invalidId0 = "-1";
            string invalidId1 = "0";
            string invalidId2 = "bogus";

            // Act
            var result1 = _neighborhoodService.DeleteComment(validNeighborhood, invalidId0);
            var result2 = _neighborhoodService.DeleteComment(validNeighborhood, invalidId1);
            var result3 = _neighborhoodService.DeleteComment(validNeighborhood, invalidId2);

            // Assert
            Assert.AreEqual(false, result1);
            Assert.AreEqual(false, result2);
            Assert.AreEqual(false, result3);

            // TearDown
            _neighborhoodService.DeleteData(testNeighborhood.id);
        }

        #endregion Comments

        #region Images
        /// <summary>
        /// Use AddData() function to test that multiple image files can be successfully uploaded. 
        /// </summary>
        [Test]
        public void AddData_UploadImage_Valid_Should_Return_True()
        {
            // Arrange
            var testImageFile = new Dictionary<string, string>()
            {
                { "testImage_1.jpg", "test image 1 content" },
                { "testImage_2.jpg", "test image 2 content" },
                { "testImage_3.jpg", "test image 2 content" },
            };

            var imagePath = GetImagePath(testImageFile);

            var expectedImagePaths = 
                "image/Neighborhood/testImage_1.jpg," +
                "image/Neighborhood/testImage_2.jpg," +
                "image/Neighborhood/testImage_3.jpg";

            // Act

            // Add test neighborhood to database.
            _neighborhoodService.AddData(Name, Address, "", ShortDesc, imagePath);

            // Retrieve test neighborhood.
            var testNeighborhood = _neighborhoodService.GetNeighborhoods().Last();

            // Assert 
            Assert.AreEqual(Name, testNeighborhood.name);
            Assert.AreEqual("Default", testNeighborhood.image);
            Assert.AreEqual(ShortDesc, testNeighborhood.shortDesc);
            Assert.AreEqual(expectedImagePaths, testNeighborhood.imagePath);

            // TearDown
            _neighborhoodService.DeleteData(testNeighborhood.id);
        }

        /// <summary>
        /// Test GetAllImages returns "no_image.jpg" when called on a neighborhood with null Image and 
        /// ImagePath properties. 
        /// </summary>
        [Test]
        public void GetAllImages_No_URLImage_No_FileImage_Should_Return_Placeholder_Image()
        {
            // Arrange

            // Add test neighborhood to database with NO IMAGE URL and NO IMAGE FILE.
            _neighborhoodService.AddData(Name, Address,"", ShortDesc, null);

            // Retrieve test neighborhood.
            var testNeighborhood = _neighborhoodService.GetNeighborhoods().Last();

            // Act
            var result = _neighborhoodService.GetAllImages(testNeighborhood);

            // Assert
            Assert.AreEqual("/image/no_image.jpg", result.First());

            // TearDown
            _neighborhoodService.DeleteData(testNeighborhood.id);
        }

        /// <summary>
        /// Test that the count of images is increased by 1 when called GetAllImages 
        /// on a neighborhood with null Image and ImagePath properties. 
        /// </summary>
        [Test]
        public void GetAllImages_No_URLImage_No_FileImage_Count_Should_Return_One_Placeholder_ImageCount()
        {
            // Arrange

            // Add test neighborhood to database with NO IMAGE URL and NO IMAGE FILE.
            _neighborhoodService.AddData(Name, Address, "", ShortDesc, null);

            // Retrieve test neighborhood.
            var testNeighborhood = _neighborhoodService.GetNeighborhoods().Last();

            // Act
            var result = _neighborhoodService.GetAllImages(testNeighborhood);

            // Assert
            Assert.AreEqual(1, result.Count());

            // TearDown
            _neighborhoodService.DeleteData(testNeighborhood.id);
        }

        /// <summary>
        /// Test GetAllImages returns a greater than 0 count of images when the neighborhood has no file images 
        /// and only URL images.
        /// </summary>
        [Test]
        public void GetAllImages_No_FileImage_Count_Should_Return_NumberOf_URLImages()
        {
            // Add test neighborhood to database with only Image property (no ImagePath property). 
            _neighborhoodService.AddData(Name, Address, Image, ShortDesc, null);

            // Retrieve test neighborhood.
            var testNeighborhood = _neighborhoodService.GetNeighborhoods().Last();

            var numOfURLImage = testNeighborhood.image.Split(",").Length;

            // Act
            var result = _neighborhoodService.GetAllImages(testNeighborhood);

            // Assert
            Assert.AreEqual(numOfURLImage, result.Count());

            // TearDown
            _neighborhoodService.DeleteData(testNeighborhood.id);
        }

        /// <summary>
        /// Test GetAllImages returns all images when called on a neighborhood with valid Image and
        /// ImagePath properties.
        /// </summary>
        [Test]
        public void GetAllImages_Has_URLImage_Has_FileImage_Should_Return_Correct_TotalCount()
        {
            // Arrange
            var testImageFiles = new Dictionary<string, string>()
            {
                { "testImage.jpg", "test image content" },
            };
            var imagePath = GetImagePath(testImageFiles);

            // Add test neighborhood to database all valid parameters. 
            _neighborhoodService.AddData(Name, Address, Image, ShortDesc, imagePath);

            // Retrieve test neighborhood.
            var testNeighborhood = _neighborhoodService.GetNeighborhoods().Last();

            // Store count of images in Image property and count of uploaded images in ImagePath property.
            var countOfURLImage = testNeighborhood.image.Split(",").Length;
            var countOfFileImage = testNeighborhood.imagePath.Split(",").Length;

            // Act
            var result = _neighborhoodService.GetAllImages(testNeighborhood);

            // Assert 
            Assert.True(countOfURLImage > 0);
            Assert.True(countOfFileImage > 0);
            Assert.AreEqual(countOfURLImage + countOfFileImage, result.Count());

            // TearDown
            _neighborhoodService.DeleteData(testNeighborhood.id);
        }

        #endregion Images

        #region UpdateData
        /// <summary>
        /// Test UpdateData function when the first parameter "data" has null image (eg. if user erased all the URL links),
        /// the image property will be re-assigned to "Default" to match Model initialization.
        /// </summary>
        [Test]
        public void UpdateData_Null_Image_Property_Reassigned_To_Default()
        {
            // Arrange

            // Add test neighborhood to database with GLOBALLY DEFINED IMAGE LINK.
            _neighborhoodService.AddData(Name, Address, Image, ShortDesc, ImgFilesNull);

            // Retrieve test neighborhood as the NEIGHBORHOOD TO BE UPDATED.
            var testNeighborhood = _neighborhoodService.GetNeighborhoods().Last();

            // Create a mock neighborood to be passed as the first parameter in UpdateData() function holding NULL IMAGE.
            var data = new NeighborhoodModel()
            {
                id = testNeighborhood.id,
                image = null
            };

            // Act
            var result = _neighborhoodService.UpdateData(data, null);

            // Assert
            Assert.AreEqual("Default", _neighborhoodService.GetNeighborhoods().Last().image);

            // TearDown
            _neighborhoodService.DeleteData(testNeighborhood.id);
        }
        #endregion
    }
}
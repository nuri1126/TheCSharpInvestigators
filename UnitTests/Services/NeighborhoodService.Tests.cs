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

        #region TestSetup
        // Global invalid id property for use in tests. 
        private const int InvalidId = -1;

        // Global valid name property for use in tests. 
        private const string Name = "Bogusland";

        // Global valid address property for use in tests
        private const string Address = "401 NE Northgate Way, Seattle, WA 98125";

        // Global valid image property for use in tests. 
        private const string Image = "http://via.placeholder.com/150";

        // Global valid shortDesc property for use in tests.
        private const string ShortDesc = "Test neighborhood description";

        // Global imgFiles property for use in tests. 
        private const IFormFileCollection ImgFilesNull = null;

        // Global valid Rating for use in AddRatings region.
        private const int ValidRating = 5;

        // Global valid comment input for use in Comments region.
        private const string ValidComment = "Bogus";

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
        private static FormFileCollection GetImagePath(Dictionary<string, string> testImageFiles)
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

        #endregion Test Setup

        #region GetNeighborhoods

        /// <summary>
        /// Tests GetNeighborhoods returns not null when database is seeded.
        /// </summary>
        [Test]
        public void GetNeighborhoods_Valid_Should_Return_Not_Null()
        {
            // Arrange

            // Add neighborhood to database and store it as testNeighborhood.
            _neighborhoodService.AddData(Name, Address, Image, ShortDesc);
            var testNeighborhood = _neighborhoodService.GetNeighborhoods().Last();

            // Act
            var result = _neighborhoodService.GetNeighborhoods();

            //Assert
            Assert.NotNull(result);

            // TearDown
            _neighborhoodService.DeleteData(testNeighborhood.id);
        }

        /// <summary>
        /// Tests GetNeighborhoods returns an IEnumerable. 
        /// </summary>
        [Test]
        public void GetNeighborhoods_Valid_Should_Return_IEnumerable()
        {
            // Arrange

            // Add neighborhood to database and store it as testNeighborhood.
            _neighborhoodService.AddData(Name, Address, Image, ShortDesc);
            var testNeighborhood = _neighborhoodService.GetNeighborhoods().Last();

            // Act
            var result = _neighborhoodService.GetNeighborhoods();

            //Assert
            Assert.IsInstanceOf(typeof(IEnumerable<NeighborhoodModel>), result);

            // TearDown
            _neighborhoodService.DeleteData(testNeighborhood.id);
        }

        #endregion GetNeighborhoods

        #region GetNeighborhoodsById

        /// <summary>
        /// Tests GetNeighborhoodById by retrieving the first neighborhood and confirming not null. 
        /// </summary>
        [Test]
        public void GetNeighborhoodById_Valid_Should_Return_Not_Null()
        {
            // Arrange

            // Add neighborhood to database and store it as testNeighborhood.
            _neighborhoodService.AddData(Name, Address, Image, ShortDesc);
            var testNeighborhood = _neighborhoodService.GetNeighborhoods().Last();

            //Act
            var result = _neighborhoodService.GetNeighborhoodById(testNeighborhood.id);

            //Assert
            Assert.NotNull(result);

            // TearDown
            _neighborhoodService.DeleteData(testNeighborhood.id);
        }

        /// <summary>
        /// Tests GetNeighborhoodById catches out of bounds input and returns null. 
        /// </summary>
        [Test]
        public void GetNeighborhoodById_Invalid_Should_Return_Null()
        {
            // Arrange

            //Act
            var invalidResult = _neighborhoodService.GetNeighborhoodById(InvalidId);

            //Assert
            Assert.Null(invalidResult);
        }

        #endregion GetNeighborhoodsById

        #region CreateID

        /// <summary>
        /// Tests that CreateID creates a temporary neighborhood object only and 
        /// there is no change in the database.
        /// /// </summary>
        [Test]
        public void CreateID_Valid_Should_Return_Same_Database_Object_Count()
        {
            // Arrange

            // Add neighborhood to database and store it as testNeighborhood.
            _neighborhoodService.AddData(Name, Address, Image, ShortDesc);
            var testNeighborhood = _neighborhoodService.GetNeighborhoods().Last();

            // Store number of objects in database prior to CreateId. 
            var prevDatabaseObjectCount = _neighborhoodService.GetNeighborhoods().Count();

            // Act
            _neighborhoodService.CreateID();
            var currDatabaseObjectCount = _neighborhoodService.GetNeighborhoods().Count();

            // Assert
            Assert.AreEqual(prevDatabaseObjectCount, currDatabaseObjectCount);

            // TearDown
            _neighborhoodService.DeleteData(testNeighborhood.id);
        }

        /// <summary>
        /// Tests that CreateID returns an id that is 1 larger than the previous id. 
        /// /// </summary>
        [Test]
        public void CreateID_Valid_Should_Return_ID_Increase_By_One()
        {
            // Arrange

            // Add neighborhood to database and store it as testNeighborhood.
            _neighborhoodService.AddData(Name, Address, Image, ShortDesc);
            var testNeighborhood = _neighborhoodService.GetNeighborhoods().Last();

            // Store number of objects in database prior to CreateId. 
            var prevID = _neighborhoodService.GetNeighborhoods().Last().id;

            // Act
            NeighborhoodModel data = _neighborhoodService.CreateID();
            var currID = data.id;

            // Assert
            Assert.AreEqual(prevID + 1, currID);

            // TearDown
            _neighborhoodService.DeleteData(testNeighborhood.id);
        }

        #endregion CreateID

        #region AddRating

        /// <summary>
        /// Tests AddRating returns null when null neighborhood provided.
        /// </summary>
        [Test]
        public void AddRating_Invalid_Null_Neighborhood_Should_Return_False()
        {
            // Arrange

            // Act
            var result = _neighborhoodService.AddRating(null, ValidRating);

            // Assert
            Assert.AreEqual(false, result);
        }

        /// <summary>
        /// Test AddRating on neighborhood with no existing ratings returns true. 
        /// </summary>
        [Test]
        public void AddRating_Valid_Neighborhood_No_Ratings_Returns_True()
        {
            // Arrange

            // Add neighborhood to database and store it as testNeighborhood.
            _neighborhoodService.AddData(Name, Address, Image, ShortDesc);
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

            // Add neighborhood to database and store it as testNeighborhood.
            _neighborhoodService.AddData(Name, Address, Image, ShortDesc);
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

            // Add neighborhood to database and store it as testNeighborhood.
            _neighborhoodService.AddData(Name, Address, Image, ShortDesc);
            var testNeighborhood = _neighborhoodService.GetNeighborhoods().Last();

            // Act
            _neighborhoodService.AddRating(testNeighborhood, ValidRating);
            var result = testNeighborhood.ratings.Length;

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

            // Add neighborhood to database and store it as testNeighborhood.
            _neighborhoodService.AddData(Name, Address, Image, ShortDesc);
            var testNeighborhood = _neighborhoodService.GetNeighborhoods().Last();

            // Add rating and store existing rating count. 
            _neighborhoodService.AddRating(testNeighborhood, ValidRating);
            var existingRatingCount = testNeighborhood.ratings.Count();

            // Act
            _neighborhoodService.AddRating(testNeighborhood, ValidRating);
            var result = testNeighborhood.ratings.Length;

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

            // Add neighborhood to database and store it as testNeighborhood.
            _neighborhoodService.AddData(Name, Address, Image, ShortDesc);
            var testNeighborhood = _neighborhoodService.GetNeighborhoods().Last();

            // Act
            var result1 = _neighborhoodService.AddRating(testNeighborhood, outOfBoundsRatings[0]);
            var result2 = _neighborhoodService.AddRating(testNeighborhood, outOfBoundsRatings[1]);
            var result3 = _neighborhoodService.AddRating(testNeighborhood, outOfBoundsRatings[2]);
            var result4 = _neighborhoodService.AddRating(testNeighborhood, outOfBoundsRatings[3]);

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

            // Add neighborhood to database and store it as testNeighborhood.
            _neighborhoodService.AddData(Name, Address, Image, ShortDesc);
            var testNeighborhood = _neighborhoodService.GetNeighborhoods().Last();

            // Act
            var result1 = _neighborhoodService.AddRating(testNeighborhood, validRatings[0]);
            var result2 = _neighborhoodService.AddRating(testNeighborhood, validRatings[1]);
            var result3 = _neighborhoodService.AddRating(testNeighborhood, validRatings[2]);
            var result4 = _neighborhoodService.AddRating(testNeighborhood, validRatings[3]);
            var result5 = _neighborhoodService.AddRating(testNeighborhood, validRatings[4]);
            var result6 = _neighborhoodService.AddRating(testNeighborhood, validRatings[5]);

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

        #endregion AddRating

        #region AddComment

        /// <summary>
        /// Tests AddComment where a valid neighborhood and string should return true.
        /// </summary>
        [Test]
        public void AddComment_Valid_Comment_Should_Return_True()
        {
            // Arrange

            // Add neighborhood to database and store it as testNeighborhood.
            _neighborhoodService.AddData(Name, Address, Image, ShortDesc);
            var testNeighborhood = _neighborhoodService.GetNeighborhoods().Last();

            // Act
            var result = _neighborhoodService.AddComment(testNeighborhood, ValidComment);

            // Assert
            Assert.AreEqual(true, result);

            // TearDown
            _neighborhoodService.DeleteData(testNeighborhood.id);
        }

        /// <summary>
        /// Test AddComment on neighborhood with no stored comments a count of the comments
        /// property equal to 1. 
        /// </summary>
        [Test]
        public void AddComment_Valid_No_Stored_Comments_Count_Should_Return_1()
        {
            // Arrange

            // Add neighborhood to database and store it as testNeighborhood.
            _neighborhoodService.AddData(Name, Address, Image, ShortDesc);
            var testNeighborhood = _neighborhoodService.GetNeighborhoods().Last();

            // Act
            _neighborhoodService.AddComment(testNeighborhood, ValidComment);
            var result = testNeighborhood.comments.Count;

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
        public void AddComment_Valid_Existing_Ratings_Count_Should_Return_True()
        {
            // Arrange

            // Add neighborhood to database and store it as testNeighborhood.
            _neighborhoodService.AddData(Name, Address, Image, ShortDesc);
            var testNeighborhood = _neighborhoodService.GetNeighborhoods().Last();

            // Add comment and store existing comment count. 
            _neighborhoodService.AddComment(testNeighborhood, ValidComment);
            var prevCommentCount = testNeighborhood.comments.Count;

            // Act
            _neighborhoodService.AddComment(testNeighborhood, ValidComment);
            var result = testNeighborhood.comments.Count;

            // Assert
            Assert.AreEqual(prevCommentCount + 1, result);

            // TearDown
            _neighborhoodService.DeleteData(testNeighborhood.id);
        }

        /// <summary>
        /// Tests AddComment returns false given a null comment.
        /// </summary>
        [Test]
        public void AddComment_Invalid_Null_Comment_Should_Return_False()
        {
            // Arrange

            // Add neighborhood to database and store it as testNeighborhood.
            _neighborhoodService.AddData(Name, Address, Image, ShortDesc);
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

            // Add neighborhood to database and store it as testNeighborhood.
            _neighborhoodService.AddData(Name, Address, Image, ShortDesc);
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
        public void AddComment_Invalid_Null_Neighborhood_Should_Return_False()
        {
            // Arrange

            // Act
            var result = _neighborhoodService.AddComment(null, ValidComment);

            // Assert
            Assert.AreEqual(false, result);
        }

        #endregion AddComment

        #region DeleteComment

        /// <summary>
        /// Test DeleteComment returns false given valid neighborhood and null string.
        /// </summary>
        [Test]
        public void DeleteComment_Invalid_Null_Neighborhood_Should_Return_False()
        {
            // Arrange

            // Add neighborhood to database and store it as testNeighborhood.
            _neighborhoodService.AddData(Name, Address, Image, ShortDesc);
            var testNeighborhood = _neighborhoodService.GetNeighborhoods().Last();

            // Act
            var result = _neighborhoodService.DeleteComment(testNeighborhood, null);

            // Assert
            Assert.AreEqual(false, result);

            // TearDown
            _neighborhoodService.DeleteData(testNeighborhood.id);
        }

        /// <summary>
        /// Test DeleteComment returns false given invalid neighborhood and valid string.
        /// </summary>
        [Test]
        public void DeleteComment_Invalid_Null_String__Should_Return_False()
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
        public void DeleteComment_Valid_Should_Return_True()
        {
            // Arrange

            // Add neighborhood to database and store it as testNeighborhood.
            _neighborhoodService.AddData(Name, Address, Image, ShortDesc);
            var testNeighborhood = _neighborhoodService.GetNeighborhoods().Last();

            // Add valid comments and store the commentId of the last stored comment. 
            _neighborhoodService.AddComment(testNeighborhood, ValidComment);
            _neighborhoodService.AddComment(testNeighborhood, ValidComment);
            _neighborhoodService.AddComment(testNeighborhood, ValidComment);

            var commentId = testNeighborhood.comments.Last().CommentId;

            // Act
            var result = _neighborhoodService.DeleteComment(testNeighborhood, commentId);

            // Assert
            Assert.AreEqual(true, result);
        }

        /// <summary>
        /// Tests after DeleteComment, count of comments stored in the neighborhood
        /// has decreased by 1. 
        /// </summary>
        [Test]
        public void DeleteComment_Valid_Count_Decrease_By_1_Should_Return_True()
        {
            // Arrange

            // Add neighborhood to database and store it as testNeighborhood.
            _neighborhoodService.AddData(Name, Address, Image, ShortDesc);
            var testNeighborhood = _neighborhoodService.GetNeighborhoods().Last();

            // Add valid comment, store count of comments, store the comment's id. 
            _neighborhoodService.AddComment(testNeighborhood, ValidComment);
            var commentCount = testNeighborhood.comments.Count;
            var commentId = testNeighborhood.comments.Last().CommentId;

            // Act
            _neighborhoodService.DeleteComment(testNeighborhood, commentId);

            // Assert
            Assert.AreEqual(commentCount - 1, testNeighborhood.comments.Count);
        }

        /// <summary>
        /// Tests that DeleteComment returns false when given an invalid commendId. 
        /// </summary>
        [Test]
        public void DeleteComment_InvalidId_Should_Return_False()
        {
            // Arrange

            // Add test neighborhood to database and store it as testNeighborhood.
            _neighborhoodService.AddData(Name, Address, Image, ShortDesc);
            var testNeighborhood = _neighborhoodService.GetNeighborhoods().Last();

            string invalidId0 = "-1";
            string invalidId1 = "0";
            string invalidId2 = "bogus";

            // Act
            var result1 = _neighborhoodService.DeleteComment(testNeighborhood, invalidId0);
            var result2 = _neighborhoodService.DeleteComment(testNeighborhood, invalidId1);
            var result3 = _neighborhoodService.DeleteComment(testNeighborhood, invalidId2);

            // Assert
            Assert.AreEqual(false, result1);
            Assert.AreEqual(false, result2);
            Assert.AreEqual(false, result3);

            // TearDown
            _neighborhoodService.DeleteData(testNeighborhood.id);
        }

        #endregion DeleteComment

        #region AddData_UploadImages

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

            // Act

            // Add test neighborhood to database and store it as testNeighborhood.
            var testNeighborhood = _neighborhoodService.AddData(Name, Address, "", ShortDesc);
            _neighborhoodService.UploadImageIfAvailable(testNeighborhood, imagePath);

            // Assert 
            Assert.AreEqual(Name, testNeighborhood.name);
            Assert.AreEqual("", testNeighborhood.image);
            Assert.AreEqual(ShortDesc, testNeighborhood.shortDesc);
            Assert.AreEqual("testImage_1.jpg", testNeighborhood.uploadedImages.First().UploadedImageName);
            Assert.AreEqual("testImage_2.jpg", testNeighborhood.uploadedImages.ElementAt(1).UploadedImageName);
            Assert.AreEqual("testImage_3.jpg", testNeighborhood.uploadedImages.Last().UploadedImageName);

            // TearDown
            _neighborhoodService.DeleteData(testNeighborhood.id);
        }

        #endregion AddData_UploadImages

        #region GetAllImages

        /// <summary>
        /// Test GetAllImages returns "no_image.jpg" when called on a neighborhood with null Image and 
        /// ImagePath properties. 
        /// </summary>
        [Test]
        public void GetAllImages_Valid_No_URLImage_No_FileImage_Should_Return_Placeholder_Image()
        {
            // Arrange

            // Add test neighborhood to database with NO IMAGE URL and NO IMAGE FILE.
            _neighborhoodService.AddData(Name, Address, "", ShortDesc);
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
            _neighborhoodService.AddData(Name, Address, "", ShortDesc);
            var testNeighborhood = _neighborhoodService.GetNeighborhoods().Last();

            // Act
            var result = _neighborhoodService.GetAllImages(testNeighborhood);

            // Assert
            Assert.AreEqual(1, result.Count);

            // TearDown
            _neighborhoodService.DeleteData(testNeighborhood.id);
        }

        /// <summary>
        /// Test GetAllImages returns a greater than 0 count of images when the neighborhood has no file images 
        /// and only URL images.
        /// </summary>
        [Test]
        public void GetAllImages_Valid_No_FileImage_Count_Should_Return_NumberOf_URLImages()
        {
            // Add test neighborhood to database with only Image property (no ImagePath property). 
            _neighborhoodService.AddData(Name, Address, Image, ShortDesc);
            var testNeighborhood = _neighborhoodService.GetNeighborhoods().Last();

            var numOfURLImage = testNeighborhood.image.Split(",").Length;

            // Act
            var result = _neighborhoodService.GetAllImages(testNeighborhood);

            // Assert
            Assert.AreEqual(numOfURLImage, result.Count);

            // TearDown
            _neighborhoodService.DeleteData(testNeighborhood.id);
        }

        /// <summary>
        /// Test GetAllImages returns all images when called on a neighborhood with valid Image and
        /// ImagePath properties.
        /// </summary>
        [Test]
        public void GetAllImages_Valid_Has_URLImage_Has_FileImage_Should_Return_Correct_TotalCount()
        {
            // Arrange
            var testImageFiles = new Dictionary<string, string>()
            {
                { "testImage.jpg", "test image content" },
            };
            var imagePath = GetImagePath(testImageFiles);

            // Add test neighborhood to database all valid parameters. 
            var testNeighborhood = _neighborhoodService.AddData(Name, Address, Image, ShortDesc);
            _neighborhoodService.UploadImageIfAvailable(testNeighborhood, imagePath);

            // Store count of images in Image property and count of uploaded images in ImagePath property.
            var countOfURLImage = testNeighborhood.image.Split(",").Length;
            var countOfFileImage = testNeighborhood.uploadedImages.Count();

            // Act
            var result = _neighborhoodService.GetAllImages(testNeighborhood);

            // Assert 
            Assert.True(countOfURLImage > 0);
            Assert.True(countOfFileImage > 0);
            Assert.AreEqual(countOfURLImage + countOfFileImage, result.Count);

            // TearDown
            _neighborhoodService.DeleteData(testNeighborhood.id);
        }

        #endregion GetAllImages

        #region DeleteUploadedImageand DeletePhysicalImageFile

        // null check neighborhood 

        /// <summary>
        /// Tests DeleteUploadedImage returns null given a null NeighborhoodModel. 
        /// </summary>
        [Test]
        public void DeleteUploadedImage_Invalid_Null_Neighborhorhood_Should_Return_Null()
        {
            // Arrange

            // Dummy valid input 
            string[] imageIds = { "1", "2", "3" };

            // Act 
            var result = _neighborhoodService.DeleteUploadedImage(null, imageIds);

            // Assert
            Assert.AreEqual(null, result);

        }

        // null check deleteImageIds
        /// <summary>
        /// Tests that DeleteUploadedImage returns null when given null deleteImageIds. 
        /// </summary>
        [Test]
        public void DeleteUploadedImage_Invalid_Null_ImageIds_Should_Return_Null()
        {
            // Arrange

            // Add neighborhood to database and store it as testNeighborhood.
            _neighborhoodService.AddData(Name, Address, Image, ShortDesc);
            var testNeighborhood = _neighborhoodService.GetNeighborhoods().Last();

            // Act 
            var result = _neighborhoodService.DeleteUploadedImage(testNeighborhood, null);

            // Assert
            Assert.AreEqual(null, result);


        }

        /// <summary>
        /// Tests that the count of uploaded images in the database decreases by the number
        /// of deleteImg Ids provided to DeleteUploadedImage. 
        /// </summary>
        [Test]
        public void DeleteUploadedImage_Valid_Count_Uploaded_Images_Should_Decrease_By_Size_ImageIds()
        {

            // Add neighborhood to database and store it as testNeighborhood.
            _neighborhoodService.AddData(Name, Address, Image, ShortDesc);

            // Get shallow copy of neighborhood. 
            var testNeighborhood = _neighborhoodService.GetNeighborhoods().Last();

            // Setup dummy imagePath to represent an image upload and add it. 
            var testImageFile = new Dictionary<string, string>()
            {
                { "testImage_1.jpg", "test image 1 content" }
            };

            var imagePath = GetImagePath(testImageFile);

            _neighborhoodService.UploadImageIfAvailable(testNeighborhood, imagePath);

            // Get the latest deepcopy of the neighborhood and store count of saved images. 
            testNeighborhood = _neighborhoodService.GetNeighborhoodById(testNeighborhood.id);
            var prevUploadedImgCount = testNeighborhood.uploadedImages.Count;

            // Get the imageId and create a deleteImgIds array. 
            var id = testNeighborhood.uploadedImages.First().UploadedImageId.ToString();
            string[] deleteImageIds = { id };

            // Act 
            _neighborhoodService.DeleteUploadedImage(testNeighborhood, deleteImageIds);

            // Get the latest deepcopy of the neighborhood. 
            testNeighborhood = _neighborhoodService.GetNeighborhoodById(testNeighborhood.id);

            var currUploadedImgCount = testNeighborhood.uploadedImages.Count;

            // Assert
            Assert.AreEqual(prevUploadedImgCount - imagePath.Count, currUploadedImgCount);

        }

        /// <summary>
        /// Tests DeleteUploadedImages deletes the correct images. 
        /// </summary>
        [Test]
        public void DeleteUploadedImage_Valid_Correct_Img_Should_Be_Deleted()
        {

            // Add neighborhood to database and store it as testNeighborhood.
            _neighborhoodService.AddData(Name, Address, Image, ShortDesc);

            // Get shallow copy of neighborhood. 
            var testNeighborhood = _neighborhoodService.GetNeighborhoods().Last();

            // Setup dummy imagePath to represent an image upload and add it. 
            var testImageFile = new Dictionary<string, string>()
            {
                { "testImage_1.jpg", "test image 1 content" },
                { "testImage_2.jpg", "test image 2 content" }
            };

            var imagePath = GetImagePath(testImageFile);

            _neighborhoodService.UploadImageIfAvailable(testNeighborhood, imagePath);

            // Get the latest deepcopy of the neighborhood and store count of saved images. 
            testNeighborhood = _neighborhoodService.GetNeighborhoodById(testNeighborhood.id);

            // Get the imageId and create a deleteImgIds array. 
            var id = testNeighborhood.uploadedImages.First().UploadedImageId.ToString();
            string[] deleteImageIds = { id };

            // Act 
            _neighborhoodService.DeleteUploadedImage(testNeighborhood, deleteImageIds);

            // Get the latest deepcopy of the neighborhood. 
            testNeighborhood = _neighborhoodService.GetNeighborhoodById(testNeighborhood.id);

            // Check that the first img is not there. 
            var result = testNeighborhood.uploadedImages.First().UploadedImageId; 

            // Assert
            Assert.AreNotEqual(id, result);
        }


        // Tests DeleteUploadedImages does not delete other images whose ids are not included
        // in deleteImgIds. 
        [Test]
        public void DeleteUploadedImage_Valid_Other_Imgs_Should_Exist()
        {

            // Add neighborhood to database and store it as testNeighborhood.
            _neighborhoodService.AddData(Name, Address, Image, ShortDesc);

            // Get shallow copy of neighborhood. 
            var testNeighborhood = _neighborhoodService.GetNeighborhoods().Last();

            // Setup dummy imagePath to represent an image upload and add it. 
            var testImageFile = new Dictionary<string, string>()
            {
                { "testImage_1.jpg", "test image 1 content" },
                { "testImage_2.jpg", "test image 2 content" }
            };

            var imagePath = GetImagePath(testImageFile);

            _neighborhoodService.UploadImageIfAvailable(testNeighborhood, imagePath);

            // Get the latest deepcopy of the neighborhood and store count of saved images. 
            testNeighborhood = _neighborhoodService.GetNeighborhoodById(testNeighborhood.id);

            // Get the imageId and create a deleteImgIds array. 
            var imageIdDeleteId = testNeighborhood.uploadedImages.First().UploadedImageId.ToString();
            var imageToKeepId = testNeighborhood.uploadedImages.Last().UploadedImageId.ToString();
            string[] deleteImageIds = { imageIdDeleteId };


            // Act 
            _neighborhoodService.DeleteUploadedImage(testNeighborhood, deleteImageIds);

            // Get the latest deepcopy of the neighborhood. 
            testNeighborhood = _neighborhoodService.GetNeighborhoodById(testNeighborhood.id);

            // Check that the second img is still saved. 
            var result = testNeighborhood.uploadedImages.Last().UploadedImageId;

            // Assert
            Assert.AreEqual(imageToKeepId, result);
        }

        #endregion DeleteUploadedImageand DeletePhysicalImageFile

        #region UpdateData
        /// <summary>
        /// Test UpdateData function when the first parameter "data" has null image (eg. if user erased all the URL links),
        /// the image property will be re-assigned to "Default" to match Model initialization.
        /// </summary>
        [Test]
        public void UpdateData_Valid_Null_Image_Property_Reassigned_To_Default()
        {
            // Arrange

            // Add test neighborhood to database with GLOBALLY DEFINED IMAGE LINK.
            _neighborhoodService.AddData(Name, Address, null, ShortDesc);
            var testNeighborhood = _neighborhoodService.GetNeighborhoods().Last();


            // Create a mock neighborood to be passed as the first parameter in UpdateData() function holding NULL IMAGE.
            var data = new NeighborhoodModel()
            {
                id = testNeighborhood.id,
                image = null
            };

            // Act
            var result = _neighborhoodService.UpdateData(data);

            // Assert
            Assert.AreEqual("", _neighborhoodService.GetNeighborhoods().Last().image);

            // TearDown
            _neighborhoodService.DeleteData(testNeighborhood.id);
        }

        /// <summary>
        /// Test UpdateData function when the first parameter "data" does not find a matching
        /// neighborhood in the database to update, it will return null.
        /// </summary>
        [Test]
        public void UpdateData_Invalid_Neighborhood_Return_Null()
        {
            // Arrange
            var invalidNeighborhood = new NeighborhoodModel
            {
                id = 666,
                name = "Invalid Name",
                shortDesc = "Invalid Desc"
            };

            // Act
            var result = _neighborhoodService.UpdateData(invalidNeighborhood);

            // Assert
            Assert.AreEqual(null, result);
        }

        #endregion UpdateData

    }
}
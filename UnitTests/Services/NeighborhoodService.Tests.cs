using LetsGoSEA.WebSite.Models;
using Microsoft.AspNetCore.Http;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace UnitTests.Services
{
    /// <summary>
    /// NeighborhoodServiceTests class contains unit tests for GetNeighborhoods, and
    /// GetNeighborhoodsById. Tests for CRUD service methods are contained in their 
    /// respective .cshtml.cs file. 
    /// </summary>
    public class NeighborhoodServiceTests
    {

        #region GetNeighborhoods

        /// <summary>
        /// Tests GetNeighborhoods() by retrieving IEnumberable and confirming not null. 
        /// </summary>
        [Test]
        public void GetNeighborhoods()
        {
            // Arrange

            // Act
            var result = TestHelper.NeighborhoodServiceObj.GetNeighborhoods();

            //Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf(typeof(IEnumerable<NeighborhoodModel>), result);
        }

        #endregion GetNeighborhoods


        #region GetNeighborhoodById_Valid_Id_Should_Return_True
        /// <summary>
        /// Tests GetNeighborhoodById by retrieving the first neighborhood and confirming not null. 
        /// </summary>
        [Test]
        public void GetNeighborhoodById_Valid_Id_Should_Return_True()
        {
            // Arrange
            var validId = 1;

            //Act
            var validResult = TestHelper.NeighborhoodServiceObj.GetNeighborhoodById(validId);

            //Assert
            Assert.NotNull(validResult);
        }
        #endregion GetNeighborhoodById_Invalid_Id_Should_Return_True


        #region GetNeighborhoodById_Invalid_Id_Should_Return_True
        /// <summary>
        /// Tests GetNeighborhoodById catches out of bounds input and returns null. 
        /// </summary>
        [Test]
        public void GetNeighborhoodById_Invalid_Id_Should_Return_True()
        {
            // Arrange
            var invalidId = 1000;

            //Act
            var invalidResult = TestHelper.NeighborhoodServiceObj.GetNeighborhoodById(invalidId);

            //Assert
            Assert.Null(invalidResult);
        }
        #endregion GetNeighborhoodById_Invalid_Id_Should_Return_True

        #region AddRating
        /// <summary>
        /// Tests AddRating, invalid neighborhood should return false.
        /// </summary>
        [Test]
        public void AddRating_InValid_Neighborhood_Should_Return_False()
        {
            // Arrange
            var neighborhoodService = TestHelper.NeighborhoodServiceObj;
            var invalidId = 95;
            var invalidNeighborhood = neighborhoodService.GetNeighborhoodById(invalidId);
            var validRating = 1;

            // Act
            var result1 = neighborhoodService.AddRating(null, validRating);
            var result2 = neighborhoodService.AddRating(invalidNeighborhood, validRating);

            // Assert
            Assert.AreEqual(false, result1);
            Assert.AreEqual(false, result2);
        }

        /// <summary>
        /// Test AddRating, valid neighborhood and valid rating should return true and correct rating count.
        /// </summary>
        [Test]
        public void AddRating_Valid_Neighborhood_Valid_Rating_Should_Return_True()
        {
            // Arrange
            // Get the First neighborhood
            var neighborhoodService = TestHelper.NeighborhoodServiceObj;
            var Id = 1;
            var neighborhood = neighborhoodService.GetNeighborhoodById(Id);
            var oldRatingCount = neighborhood.ratings.Length;
            var newRating = 5;

            // Act
            var result = neighborhoodService.AddRating(neighborhood, newRating);

            // Assert
            Assert.AreEqual(true, result);
            Assert.AreEqual(oldRatingCount + 1, neighborhood.ratings.Length);
            Assert.AreEqual(5, neighborhood.ratings.Last());
        }

        /// <summary>
        /// Test AddRating, where a rating entered out of the high bound returns false.
        /// </summary>
        [Test]
        public void AddRating_Invalid_Rating_High_Return_False()
        {
            // Arrange
            var neighborhoodService = TestHelper.NeighborhoodServiceObj;
            var validID = 1;
            var validNeighborhood = neighborhoodService.GetNeighborhoodById(validID);
            var invalidHighRating = 6;

            // Act
            var result = neighborhoodService.AddRating(validNeighborhood, invalidHighRating);

            // Assert
            Assert.AreEqual(false, result);
        }

        /// <summary>
        /// Test AddRating, where a rating entered out of the low bound returns false.
        /// </summary>
        [Test]
        public void AddRating_Invalid_Rating_Low_Return_False()
        {
            // Arrange
            var neighborhoodService = TestHelper.NeighborhoodServiceObj;
            var validID = 1;
            var validNeighborhood = neighborhoodService.GetNeighborhoodById(validID);
            var invalidLowRating = -1;

            // Act
            var result = neighborhoodService.AddRating(validNeighborhood, invalidLowRating);

            // Assert
            Assert.AreEqual(false, result);
        }

        /// <summary>
        /// Tests AddRating where valid ratings return true.
        /// </summary>
        [Test]
        public void AddRating_Valid_Rating_Return_True()
        {
            // Arrange
            var neighborhoodService = TestHelper.NeighborhoodServiceObj;
            var validID = 1;
            var validNeighborhood = neighborhoodService.GetNeighborhoodById(validID);
            var validRatingLowerBound = 0;
            var validRatingMiddle = 2;
            var validRatingUpperBound = 5;

            // Act
            var result1 = neighborhoodService.AddRating(validNeighborhood, validRatingLowerBound);
            var result2 = neighborhoodService.AddRating(validNeighborhood, validRatingMiddle);
            var result3 = neighborhoodService.AddRating(validNeighborhood, validRatingUpperBound);

            // Assert
            Assert.AreEqual(true, result1);
            Assert.AreEqual(true, result2);
            Assert.AreEqual(true, result3);
        }

        /// <summary>
        /// Test AddRating where an empty rating returns true.
        /// </summary>
        [Test]
        public void AddRating_Empty_Rating_Return_True()
        {
            // Arrange
            // Pick a neighborhood with no rating.
            var neighborhoodService = TestHelper.NeighborhoodServiceObj;
            var idWithNoRating = 15;
            var neighborhoodWithNoRating = neighborhoodService.GetNeighborhoodById(idWithNoRating);
            var validRating = 1;

            // Act
            var result = neighborhoodService.AddRating(neighborhoodWithNoRating, validRating);

            // Assert
            Assert.AreEqual(true, result);
            Assert.NotNull(neighborhoodWithNoRating.ratings);
        }

        #endregion AddRating

        #region AddComments

        /// <summary>
        /// Tests AddComments where an invalid neighborhood should return false.
        /// </summary>
        [Test]
        public void AddComment_InValid_Neighborhood_Should_Return_False()
        {
            // Arrange
            var neighborhoodService = TestHelper.NeighborhoodServiceObj;
            var invalidId = 95;
            var invalidNeighborhood = neighborhoodService.GetNeighborhoodById(invalidId);
            var validComment = "Good job";

            // Act
            var result1 = neighborhoodService.AddComment(null, validComment);
            var result2 = neighborhoodService.AddComment(invalidNeighborhood, validComment);

            // Assert
            Assert.AreEqual(false, result1);
            Assert.AreEqual(false, result2);
        }

        /// <summary>
        /// Tests AddComment where a null or empty comment should return false.
        /// </summary>
        [Test]
        public void AddComment_Null_Or_Empty_Comment_Return_False()
        {
            // Arrange
            var neighborhoodService = TestHelper.NeighborhoodServiceObj;
            var validID = 1;
            var validNeighborhood = neighborhoodService.GetNeighborhoodById(validID);
            var emptyComment = "";

            // Act
            var result1 = neighborhoodService.AddComment(validNeighborhood, null);
            var result2 = neighborhoodService.AddComment(validNeighborhood, emptyComment);

            // Assert
            Assert.AreEqual(false, result1);
            Assert.AreEqual(false, result2);
        }

        /// <summary>
        /// Tests AddComment where a valid neighborhood and valid comment return true and update data successfully.
        /// </summary>
        [Test]
        public void AddComment_Valid_Neighborhood_Valid_Comment_Return_True()
        {
            // Arrange
            var neighborhoodService = TestHelper.NeighborhoodServiceObj;
            var validID = 1;
            var validNeighborhood = neighborhoodService.GetNeighborhoodById(validID);
            var validComment = "CSI Rocks";
            var oldCommentCount = validNeighborhood.comments.Count();

            // Act
            var result = neighborhoodService.AddComment(validNeighborhood, validComment);

            // Assert
            Assert.AreEqual(true, result);
            Assert.AreEqual(oldCommentCount + 1, validNeighborhood.comments.Count());
            Assert.AreEqual(validComment, validNeighborhood.comments.Last().Comment);
        }

        /// <summary>
        /// Tests AddComment where empty comments return true.
        /// </summary>
        [Test]
        public void AddComment_Empty_Comments_Return_True()
        {
            // Arrange
            // Pick a neighborhood with no comment.
            var neighborhoodService = TestHelper.NeighborhoodServiceObj;
            var idWithNoComment = 15;
            var neighborhoodWithNoComment = neighborhoodService.GetNeighborhoodById(idWithNoComment);
            var newComment = "Unit testing is fun";

            // Act
            var result = neighborhoodService.AddComment(neighborhoodWithNoComment, newComment);

            // Assert
            Assert.AreEqual(true, result);
            Assert.IsNotEmpty(neighborhoodWithNoComment.comments);
        }
        #endregion AddComments

        #region AddData_UploadImage
        /// <summary>
        /// Use AddData() function to test that an image file can be successfully uploaded.
        /// </summary>
        [Test]
        public void AddData_UploadImage_Valid_Successful()
        {
            // Arrange
            var neighborhoodService = TestHelper.NeighborhoodServiceObj;
            var oldNeighborhoodCount = neighborhoodService.GetNeighborhoods().Count();
            var validName = "validName";
            var validDesc = "validDesc";

            //Setup mock file using a memory stream
            var content = "Random content";
            var imageFileName = "test.jpg";
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(content);
            writer.Flush();
            stream.Position = 0;

            //create FormFile with desired data
            var imageFiles = new FormFileCollection();
            IFormFile file = new FormFile(stream, 0, stream.Length, "id_from_form", imageFileName);
            imageFiles.Add(file);

            // Act
            neighborhoodService.AddData(validName, "", validDesc, imageFiles);

            // Assert that a new neighborhood has been successfully created 
            Assert.AreEqual(oldNeighborhoodCount + 1, neighborhoodService.GetNeighborhoods().Count());

            // Assert the fields of this new neighborhood are correct 
            var newNeighborhood = neighborhoodService.GetNeighborhoodById(oldNeighborhoodCount + 1);
            Assert.AreEqual(validName, newNeighborhood.name);
            Assert.AreEqual(0, newNeighborhood.image.Count());
            Assert.AreEqual(validDesc, newNeighborhood.shortDesc);
            Assert.AreEqual("image/Neighborhood/test.jpg", newNeighborhood.imagePath);

        }
        #endregion AddData_UploadImage

        #region GetAllImages
        /// <summary>
        /// Test GetAllImages() function: simulate creation of a new neighborhood with no user input image, should return no_image.jpg.
        /// </summary>
        [Test]
        public void GetAllImages_No_URLImage_No_FileImage_Return_No_Image_Path()
        {
            // Arrange: Generate a new neighborhood object with NO IMAGE URL and NO IMAGE FILE
            var neighborhoodService = TestHelper.NeighborhoodServiceObj;
            var oldNeighborhoodCount = neighborhoodService.GetNeighborhoods().Count();
            neighborhoodService.AddData("validName", null, "validDesc", null);
            var newNeighborhood = neighborhoodService.GetNeighborhoodById(oldNeighborhoodCount + 1);

            // Act
            var result = neighborhoodService.GetAllImages(newNeighborhood);

            // Assert
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual("/image/no_image.jpg", result.First());
        }

        /// <summary>
        /// Test GetAllImages() function: pick a neighborhood from database that has both URL image and File image, 
        /// should return all images.
        /// </summary>
        [Test]
        public void GetAllImages_Has_URLImage_Has_FileImage_Return_AllImages()
        {
            // Arrange: Pick Downtown (ID = 11) that has both URL image and file image
            var neighborhoodService = TestHelper.NeighborhoodServiceObj;
            var pickedID = 11;
            var neighborhood = neighborhoodService.GetNeighborhoodById(pickedID);
            var numOfURLImage = neighborhood.image.Split(",").Length;
            var numOfFileImage = neighborhood.imagePath.Split(",").Length;

            // Act
            var result = neighborhoodService.GetAllImages(neighborhood);

            // Assert 
            Assert.True(numOfURLImage > 0);
            Assert.True(numOfFileImage > 0);
            Assert.AreEqual(numOfURLImage + numOfFileImage, result.Count());
        }

        /// <summary>
        /// Test GetAllImages() function: pick a neighborhood that has no URL images and only file images,
        /// should return only file images.
        /// </summary>
        [Test]
        public void GetAllImages_No_URLImage_Return_Only_FileImages()
        {
            // Arrange: Pick West Seattle (ID = 17) that has no URL image but has File images
            var neighborhoodService = TestHelper.NeighborhoodServiceObj;
            var pickedID = 17;
            var neighborhood = neighborhoodService.GetNeighborhoodById(pickedID);
            var numOfFileImage = neighborhood.imagePath.Split(",").Length;

            // Act
            var result = neighborhoodService.GetAllImages(neighborhood);

            // Assert
            Assert.True(numOfFileImage > 0);
            Assert.AreEqual(numOfFileImage, result.Count());
        }

        /// <summary>
        /// Test GetAllImages() function: pick a neighborhood that has no file images and only URL images,
        /// should return only URL images
        /// </summary>
        [Test]
        public void GetAllImages_No_FileImage_Return_Only_URLImages()
        {
            // Arrange: Pick Green Lake (ID = 2) that has no File image but has URL images
            var neighborhoodService = TestHelper.NeighborhoodServiceObj;
            var pickedID = 2;
            var neighborhood = neighborhoodService.GetNeighborhoodById(pickedID);
            var numOfURLImage = neighborhood.image.Split(",").Length;

            // Act
            var result = neighborhoodService.GetAllImages(neighborhood);

            // Assert
            Assert.True(numOfURLImage > 0);
            Assert.AreEqual(numOfURLImage, result.Count());
        }
        #endregion GetAllImages

        #region DeleteComment
        /// <summary>
        /// An invalid CommentId should return false. 
        /// </summary>
        [Test]
        public void DeleteComment_InvalidId_Should_Return_False()
        {
            // Arrange
            var neighborhoodService = TestHelper.NeighborhoodServiceObj;
            var validNeighborhood = neighborhoodService.GetNeighborhoodById(1);
            string invalidId0 = "-1";
            string invalidId1 = "0";
            string invalidId2 = "bogus";

            // Act
            var result1 = neighborhoodService.DeleteComment(validNeighborhood, invalidId0);
            var result2 = neighborhoodService.DeleteComment(validNeighborhood, invalidId1);
            var result3 = neighborhoodService.DeleteComment(validNeighborhood, invalidId2);

            // Assert
            Assert.AreEqual(false, result1);
            Assert.AreEqual(false, result2);
            Assert.AreEqual(false, result3);
        }

        /// <summary>
        /// An invalid CommentId should return false. 
        /// </summary>
        [Test]
        public void DeleteComment_Invalid_Neighborhood_Should_Return_False()
        {
            // Arrange
            var neighborhoodService = TestHelper.NeighborhoodServiceObj;
            var invalidId = 95;
            var invalidNeighborhood = neighborhoodService.GetNeighborhoodById(invalidId);
            var validCommentId = "1";

            // Act
            var result1 = neighborhoodService.DeleteComment(null, validCommentId);
            var result2 = neighborhoodService.AddComment(invalidNeighborhood, validCommentId);

            // Assert
            Assert.AreEqual(false, result1);
            Assert.AreEqual(false, result2);
        }

        /// <summary>
        /// Tests DeleteComment returns a true after successful call. 
        /// </summary>
        [Test]
        public void DeleteComment_ValidNeighborhood_ValidId_Should_Return_True()
        {
            // Arrange
            var neighborhoodService = TestHelper.NeighborhoodServiceObj;
            var validNeighborhood = neighborhoodService.GetNeighborhoodById(1);
            var validComment = "bogus";
            neighborhoodService.AddComment(validNeighborhood, validComment);
            var commentId = validNeighborhood.comments.Last().CommentId;
            var commentCount = validNeighborhood.comments.Count();

            // Act
            var result = neighborhoodService.DeleteComment(validNeighborhood, commentId);

            // Assert
            Assert.AreEqual(true, result);
            Assert.AreEqual(commentCount - 1, validNeighborhood.comments.Count());
        }

        #endregion DeleteComment
    }
}
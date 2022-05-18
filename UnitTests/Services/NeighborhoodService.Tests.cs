using LetsGoSEA.WebSite.Models;
using NUnit.Framework;
using System.Collections.Generic;
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
        #region TestSetup
        /// <summary>
        /// See TestHelper.cs 
        /// </summary>
        [SetUp]
        public void TestInitialize()
        {
        }
        #endregion TestSetup

        #region GetNeighborhoods

        /// <summary>
        /// Tests GetNeighborhoods() by retrieving IEnumberable and confirming not null. 
        /// </summary>
        [Test]
        public void GetNeighborhoods()
        {
            // Arrange - TestHelper.cs

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
        /// Test AddRating: invalid neighborhood should return false
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
        /// Test AddRating: valid neighborhood and valid rating should return true and correct rating count
        /// </summary>
        [Test]
        public void AddRating_Valid_Neighborhood_Valid_Rating_Should_Return_True()
        {
            // Arrange
            // Get the First neighborhood
            var neighborhoodService = TestHelper.NeighborhoodServiceObj;
            var Id = 1;
            var neighborhood = neighborhoodService.GetNeighborhoodById(Id);
            var oldRatingCount = neighborhood.Ratings.Length;
            var newRating = 5;

            // Act
            var result = neighborhoodService.AddRating(neighborhood, newRating);

            // Assert
            Assert.AreEqual(true, result);
            Assert.AreEqual(oldRatingCount + 1, neighborhood.Ratings.Length);
            Assert.AreEqual(5, neighborhood.Ratings.Last());
        }

        /// <summary>
        /// Test AddRating: too high of a rating return false
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
        /// Test AddRating: too low of a rating return false
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
        /// Test AddRating: valid ratings return true 
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
        /// Test AddRating: empty rating return true
        /// </summary>
        [Test]
        public void AddRating_Empty_Rating_Return_True()
        {
            // Arrange
            // Pick a neighborhood with no rating 
            var neighborhoodService = TestHelper.NeighborhoodServiceObj;
            var idWithNoRating = 15;
            var neighborhoodWithNoRating = neighborhoodService.GetNeighborhoodById(idWithNoRating);
            var validRating = 1;
            // Act
            var result = neighborhoodService.AddRating(neighborhoodWithNoRating, validRating);
            // Assert
            Assert.AreEqual(true, result);
            Assert.NotNull(neighborhoodWithNoRating.Ratings);
        }

        #endregion AddRating

        #region AddComments

        /// <summary>
        /// Test AddComment: invalid neighborhood should return false
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
        /// Test AddComment: null or empty comment should return false
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
        /// Test AddComment: valid neighborhood and valid comment return true and update data successfully
        /// </summary>
        [Test]
        public void AddComment_Valid_Neighborhood_Valid_Comment_Return_True()
        {
            // Arrange
            var neighborhoodService = TestHelper.NeighborhoodServiceObj;
            var validID = 1;
            var validNeighborhood = neighborhoodService.GetNeighborhoodById(validID);
            var validComment = "CSI Rocks";
            var oldCommentCount = validNeighborhood.Comments.Count();

            // Act
            var result = neighborhoodService.AddComment(validNeighborhood, validComment);

            // Assert
            Assert.AreEqual(true, result);
            Assert.AreEqual(oldCommentCount + 1, validNeighborhood.Comments.Count());
            Assert.AreEqual(validComment, validNeighborhood.Comments.Last().Comment);
        }

        /// <summary>
        /// Test AddComment: empty comments return true
        /// </summary>
        [Test]
        public void AddComment_Empty_Comments_Return_True()
        {
            // Arrange
            // Pick a neighborhood with no comment
            var neighborhoodService = TestHelper.NeighborhoodServiceObj;
            var idWithNoComment = 15;
            var neighborhoodWithNoComment = neighborhoodService.GetNeighborhoodById(idWithNoComment);
            var newComment = "Unit testing is fun";

            // Act
            var result = neighborhoodService.AddComment(neighborhoodWithNoComment, newComment);

            // Assert
            Assert.AreEqual(true, result);
            Assert.IsNotEmpty(neighborhoodWithNoComment.Comments);
        }
        #endregion AddComments
    }
}
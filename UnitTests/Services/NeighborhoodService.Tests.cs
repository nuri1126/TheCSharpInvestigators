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
            var invalidId = 95;
            var invalidNeighborhood = TestHelper.NeighborhoodServiceObj.GetNeighborhoodById(invalidId);
            var validRating = 1;

            // Act
            var result1 = TestHelper.NeighborhoodServiceObj.AddRating(null, validRating);
            var result2 = TestHelper.NeighborhoodServiceObj.AddRating(invalidNeighborhood, validRating);

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
            var Id = 1;
            var neighborhood = TestHelper.NeighborhoodServiceObj.GetNeighborhoodById(Id);
            var oldRatingCount = neighborhood.Ratings.Length;
            var newRating = 5;

            // Act
            var result = TestHelper.NeighborhoodServiceObj.AddRating(neighborhood, newRating);

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
            var validID = 1;
            var validNeighborhood = TestHelper.NeighborhoodServiceObj.GetNeighborhoodById(validID);
            var invalidHighRating = 6;

            // Act
            var result = TestHelper.NeighborhoodServiceObj.AddRating(validNeighborhood, invalidHighRating);
            
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
            var validID = 1;
            var validNeighborhood = TestHelper.NeighborhoodServiceObj.GetNeighborhoodById(validID);
            var invalidLowRating = -1;

            // Act
            var result = TestHelper.NeighborhoodServiceObj.AddRating(validNeighborhood, invalidLowRating);
            
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
            var validID = 1;
            var validNeighborhood = TestHelper.NeighborhoodServiceObj.GetNeighborhoodById(validID);
            var validRatingLowerBound = 0;
            var validRatingMiddle = 2;
            var validRatingUpperBound = 5;
            // Act
            var result1 = TestHelper.NeighborhoodServiceObj.AddRating(validNeighborhood, validRatingLowerBound);
            var result2 = TestHelper.NeighborhoodServiceObj.AddRating(validNeighborhood, validRatingMiddle);
            var result3 = TestHelper.NeighborhoodServiceObj.AddRating(validNeighborhood, validRatingUpperBound);
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
            var idWithNoRating = 15;
            var neighborhoodWithNoRating = TestHelper.NeighborhoodServiceObj.GetNeighborhoodById(idWithNoRating);
            var validRating = 1;
            // Act
            var result = TestHelper.NeighborhoodServiceObj.AddRating(neighborhoodWithNoRating, validRating);
            // Assert
            Assert.AreEqual(true, result);
        }

        #endregion AddRating
    }
}
using LetsGoSEA.WebSite.Models;
using NUnit.Framework;
using System.Collections.Generic;

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



        // Implement after AddRating feature is implemented.
        #region AddRating
        /*
        [Test]
        public void AddRating_InValid_Product_Null_Should_Return_False()
        {
            // Arrange

            // Act
            //var result = TestHelper.ProductService.AddRating(null, 1);

            // Assert
           // Assert.AreEqual(false, result);
        }

        [Test]
        public void AddRating_InValid_()
        {
            // Arrange

            // Act
            //var result = TestHelper.ProductService.AddRating(null, 1);

            // Assert
            //Assert.AreEqual(false, result);
        }

        // ....

        [Test]
        public void AddRating_Valid_Product_Valid_Rating_Valid_Should_Return_True()
        {
            // Arrange

            // Get the First data item
            //var data = TestHelper.ProductService.GetAllData().First();
            //var countOriginal = data.Ratings.Length;

            // Act
            //var result = TestHelper.ProductService.AddRating(data.Id, 5);
            //var dataNewList = TestHelper.ProductService.GetAllData().First();

            // Assert
            //Assert.AreEqual(true, result);
            //Assert.AreEqual(countOriginal + 1, dataNewList.Ratings.Length);
            //Assert.AreEqual(5, dataNewList.Ratings.Last());
        }


        [Test]
        public void AddRating_InValid_Product_Should_Return_False()
        {
            // Arrange
            //var result = TestHelper.ProductService.AddRating("95", 1);
            //Assert.AreEqual(false, result);

        }

        [Test]
        public void AddRating_Product_Not_Found_Return_False()
        {
            // Arrange
            //var result = TestHelper.ProductService.AddRating("95", 1);
            //Assert.AreEqual(false, result);

        }

        [Test]
        public void AddRating_Invalid_Rating_High()
        {
// Students will do
        }

        [Test]
        public void Ratings_Empty_Return_True()
        {
// Students will do

        }

// Students will do any others that are required

        #endregion AddRating
     */
        #endregion TestRating
    }
}
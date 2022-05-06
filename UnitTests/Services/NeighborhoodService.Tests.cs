
using Microsoft.AspNetCore.Mvc;

using NUnit.Framework;

using LetsGoSEA.WebSite.Pages.Neighborhood;
using LetsGoSEA.WebSite.Models;
using System.Linq;

namespace UnitTests.Services
{
    public class NeighborhoodServiceTests
    {
        #region TestSetup

        [SetUp]
        public void TestInitialize()
        {
        }

        #endregion TestSetup

        #region AddRating
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

        // ....

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
    }
}
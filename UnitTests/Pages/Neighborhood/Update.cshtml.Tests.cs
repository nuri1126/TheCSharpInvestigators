using System.Diagnostics;
using LetsGoSEA.WebSite.Models;
using NUnit.Framework;
using LetsGoSEA.WebSite.Pages.Neighborhood;
using Microsoft.AspNetCore.Mvc;

namespace UnitTests.Pages.Neighborhood
{
    public class UpdateTests
    {
        #region TestSetup

        private static UpdateModel _pageModel;

        [SetUp]
        public void TestInitialize()
        {
            _pageModel = new UpdateModel(TestHelper.NeighborhoodServiceObj);
        }

        #endregion TestSetup


        #region OnGet

        [Test]
        public void OnGet_Valid_Should_Return_Products()
        {
            // Arrange
            
            // Act
            _pageModel.OnGet(2);
            
            // Assert
            Assert.AreEqual(true, _pageModel.ModelState.IsValid);
            Assert.AreEqual("Greenlake", _pageModel.Neighborhood.Name);
        }
        #endregion OnGet
        
        #region OnPostAsync

        [Test]
        public void OnPostAsync_Valid_Should_Return_Products()
        {
            // Arrange
            _pageModel.Neighborhood = new NeighborhoodModel
            {
                Id = 2,
                Name = "Greenwood",
                ShortDesc = "Welcome to the green neighborhood of Greenwood"
            };
            
            // Act
            var result = _pageModel.OnPost() as RedirectToPageResult;
            
            // Assert
            Assert.AreEqual(true, _pageModel.ModelState.IsValid);
            Debug.Assert(result != null, nameof(result) + " != null");
            Assert.AreEqual(true, result.PageName.Contains("Index"));
        }

        [Test]
        public void OnPostAsync_InValid_Model_NotValid_Return_Page()
        {
            // Arrange
            _pageModel.Neighborhood = new NeighborhoodModel
            {
                Id = 666,
                Name = "Invalid Name",
                ShortDesc = "Invalid Desc"
            };
            
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
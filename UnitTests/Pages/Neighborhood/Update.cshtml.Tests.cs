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
        public static UpdateModel PageModel;

        [SetUp]
        public void TestInitialize()
        {
            PageModel = new UpdateModel(TestHelper.NeighborhoodServiceObj);
        }

        #endregion TestSetup


        #region OnGet

        [Test]
        public void OnGet_Valid_Should_Return_Products()
        {
            // Arrange
            
            // Act
            PageModel.OnGet(2);
            
            // Assert
            Assert.AreEqual(true, PageModel.ModelState.IsValid);
            Assert.AreEqual("Greenlake", PageModel.Neighborhood.Name);
        }
        #endregion OnGet
        
        #region OnPostAsync

        [Test]
        public void OnPostAsync_Valid_Should_Return_Products()
        {
            // Arrange
            PageModel.Neighborhood = new NeighborhoodModel
            {
                Id = 2,
                Name = "Greenwood",
                ShortDesc = "Welcome to the green neighborhood of Greenwood"
            };
            
            // Act
            var result = PageModel.OnPost() as RedirectToPageResult;
            
            // Assert
            Assert.AreEqual(true, PageModel.ModelState.IsValid);
            Debug.Assert(result != null, nameof(result) + " != null");
            Assert.AreEqual(true, result.PageName.Contains("Index"));
        }

        [Test]
        public void OnPostAsync_InValid_Model_NotValid_Return_Page()
        {
            // Arrange
            PageModel.Neighborhood = new NeighborhoodModel
            {
                Id = 666,
                Name = "Invalid Name",
                ShortDesc = "Invalid Desc"
            };
            
            // Force an invalid error state
            PageModel.ModelState.AddModelError("InvalidState", "Invalid Neighborhood state");
            
            // Act
            var result = PageModel.OnPost() as ActionResult;
            
            // Assert
            Assert.AreEqual(false, PageModel.ModelState.IsValid);
        }
        
        #endregion OnPostAsync
    }
}
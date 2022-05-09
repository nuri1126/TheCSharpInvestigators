using System.Diagnostics;
using LetsGoSEA.WebSite.Models;
using NUnit.Framework;
using LetsGoSEA.WebSite.Pages.Neighborhood;
using Microsoft.AspNetCore.Mvc;

namespace UnitTests.Pages.Neighborhood
{
    /// <summary>
    /// Unit test for Delete page
    /// </summary>
    public class DeleteTests
    {
        #region TestSetup
        // DeleteModel object
        private static DeleteModel _pageModel;

        /// <summary>
        /// Set up Delete Model object for testing
        /// </summary>
        [SetUp]
        public void TestInitialize()
        {
            _pageModel = new DeleteModel(TestHelper.NeighborhoodServiceObj);
        }

        #endregion TestSetup


        #region OnGet
        /// <summary>
        /// Test GET method: valid page should return neighborhood
        /// </summary>
        [Test]
        public void OnGet_Valid_Should_Return_Neighborhood()
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
        /// <summary>
        /// Test POST method: valid page should be able to delete a record and return neighborhoods 
        /// </summary>
        [Test]
        public void OnPostAsync_Valid_Should_Return_Products()
        {
            // Creating product to delete
            _pageModel.Neighborhood = TestHelper.NeighborhoodServiceObj.CreateID();
            _pageModel.Neighborhood.Name = "Neighborhood to Delete";
            _pageModel.Neighborhood.Id = 999;
            TestHelper.NeighborhoodServiceObj.UpdateData(_pageModel.Neighborhood);
            
            // Act
            var result = _pageModel.OnPost() as RedirectToPageResult;
            
            // Assert
            Assert.AreEqual(true, _pageModel.ModelState.IsValid);
            Debug.Assert(result != null, nameof(result) + " != null");
            Assert.AreEqual(true, result.PageName.Contains("Index"));
            
            // Confirm the item is deleted
            Assert.AreEqual(null, TestHelper.NeighborhoodServiceObj.GetNeighborhoodById(_pageModel.Neighborhood.Id));
        }

        /// <summary>
        /// Test POST method: invalid data will make ModelState invalid
        /// </summary>
        [Test]
        public void OnPostAsync_Invalid_Model_NotValid_ReturnPage()
        {
            // Arrange
            _pageModel.Neighborhood = new NeighborhoodModel
            {
                Id = 999,
                Name = "Invalid",
                ShortDesc = "Invalid"
            };
            
            // Force an invalid error state
            _pageModel.ModelState.AddModelError("InvalidState", "Neighborhood is invalid");
            
            // Act
            var result = _pageModel.OnPost() as ActionResult;
            
            // Assert
            Assert.AreEqual(false, _pageModel.ModelState.IsValid);
        }
        
        #endregion OnPostAsync
    }
}
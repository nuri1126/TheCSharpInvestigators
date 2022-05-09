using System.Diagnostics;
using LetsGoSEA.WebSite.Pages.Explore;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;

namespace UnitTests.Pages.Explore
{
    /// <summary>
    /// Unit test for Explore Neighborhood Page
    /// </summary>
    public class NeighborhoodTests
    {
        #region TestSetup
        // Neighborhood Model object
        private static NeighborhoodModel _pageModel;

        /// <summary>
        /// Set Up Neighborhood Model for testing 
        /// </summary>
        [SetUp]
        public void TestInitialize()
        {
            _pageModel = new NeighborhoodModel(TestHelper.NeighborhoodServiceObj);
        }

        #endregion TestSetup
        
        #region Onget
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
            Assert.AreEqual("Greenlake", _pageModel.CurrentNeighborhood.Name);
        }

        /// <summary>
        /// Test GET method: invalid model should return index page 
        /// </summary>
        [Test]
        public void OnGet_Invalid_Model_NotValid_ReturnIndex()
        {
            // Arrange
            _pageModel.CurrentNeighborhood = new LetsGoSEA.WebSite.Models.NeighborhoodModel()
            {
                Id = 666,
                Name = "Invalid Name"
            };
            
            // Force an invalid error state
            _pageModel.ModelState.AddModelError("InvalidState", "Neighborhood is Invalid");
            
            // Act
            var result = _pageModel.OnGet(_pageModel.CurrentNeighborhood.Id) as RedirectToPageResult;
            
            // Assert
            Assert.AreEqual(false, _pageModel.ModelState.IsValid);
            Assert.AreEqual(true, result.PageName.Contains("Index"));
        }

        /// <summary>
        /// Test GET method: invalid ID should lead to invalid model which should return index page
        /// </summary>
        [Test]
        public void OnGet_Invalid_Id_NotValid_ReturnExplore()
        {
            // Arrange
            _pageModel.CurrentNeighborhood = new LetsGoSEA.WebSite.Models.NeighborhoodModel()
            {
                Id = 666,
                Name = "Invalid Name"
            };

            // Act
            var result = _pageModel.OnGet(_pageModel.CurrentNeighborhood.Id) as RedirectToPageResult;
            
            // Assert
            Assert.AreEqual(true, _pageModel.ModelState.IsValid);
            Debug.Assert(result != null, nameof(result) + " != null");
            Assert.AreEqual(true, result.PageName.Contains("Index"));
        }
        #endregion OnGet
    }
}
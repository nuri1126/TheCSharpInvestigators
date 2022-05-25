using LetsGoSEA.WebSite.Models;
using LetsGoSEA.WebSite.Pages.Neighborhood;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using System.Diagnostics;

namespace UnitTests.Pages.Neighborhood
{
    /// <summary>
    /// Unit test for Delete functionality.
    /// </summary>
    public class DeleteTests
    {
        #region TestSetup

        // DeleteModel object
        private static DeleteModel _pageModel;

        /// <summary>
        /// Initialize DeleteModel a NeighborhoodService object. 
        /// </summary>
        [SetUp]
        public void TestInitialize()
        {
            _pageModel = new DeleteModel(TestHelper.NeighborhoodServiceObj);
        }

        #endregion TestSetup


        #region OnGet
        /// <summary>
        /// Tests that when OnGet is called on a valid neighborhood, that neighborhood is returned.
        /// </summary>
        [Test]
        public void OnGet_Valid_Should_Return_Neighborhood()
        {
            // Arrange

            // Act
            _pageModel.OnGet(2);

            // Assert
            Assert.AreEqual(true, _pageModel.ModelState.IsValid);
            Assert.AreEqual("Greenlake", _pageModel.neighborhood.name);
        }

        #endregion OnGet

        #region OnPost
        /// <summary>
        /// Tests that when OnPost is called on a valid neighborhood, the record is deleted from the database.
        /// </summary>
        [Test]
        public void OnPost_Delete_Valid_Neighborhood_Should_Return_Neighborhoods()
        {
            // Create neighborhood object to delete. 
            _pageModel.neighborhood = TestHelper.NeighborhoodServiceObj.CreateID();
            _pageModel.neighborhood.name = "Neighborhood to Delete";
            _pageModel.neighborhood.id = 999;
            TestHelper.NeighborhoodServiceObj.UpdateData(_pageModel.neighborhood);

            // Act
            var result = _pageModel.OnPost() as RedirectToPageResult;

            // Assert
            Assert.AreEqual(true, _pageModel.ModelState.IsValid);
            Debug.Assert(result != null, nameof(result) + " != null");
            Assert.AreEqual(true, result.PageName.Contains("Index"));

            // Confirm the item is deleted.
            Assert.AreEqual(null, TestHelper.NeighborhoodServiceObj.GetNeighborhoodById(_pageModel.neighborhood.id));
        }

        /// <summary>
        /// Test that when OnPost is called on an invalid neighborhood, the ModelState 
        /// becomes invalid. 
        /// </summary>
        [Test]
        public void OnPost_Invalid_Model_NotValid_ReturnPage()
        {
            // Arrange
            _pageModel.neighborhood = new NeighborhoodModel
            {
                id = 999,
                name = "Invalid",
                shortDesc = "Invalid"
            };

            // Force an invalid error state.
            _pageModel.ModelState.AddModelError("InvalidState", "Neighborhood is invalid");

            // Act
            var result = _pageModel.OnPost() as ActionResult;

            // Assert
            Assert.AreEqual(false, _pageModel.ModelState.IsValid);
        }

        #endregion OnPostAsync
    }
}
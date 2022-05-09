using System.Linq;
using NUnit.Framework;
using LetsGoSEA.WebSite.Pages.Neighborhood;
using LetsGoSEA.WebSite.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Diagnostics;

namespace UnitTests.Pages.Neighborhood
{
    public class CreateTests
    {
        #region TestSetup

        private static CreateModel _pageModel;

        [SetUp]
        public void TestInitialize()
        {
            _pageModel = new CreateModel(TestHelper.NeighborhoodServiceObj)
            {
                PageContext = TestHelper.PageContext
            };
        }

        #endregion TestSetup

        #region OnGet

        [Test]
        public void OnGet_Valid_Should_Not_Increment_Model_Count()
        {
            // Arrange
            var oldCount = TestHelper.NeighborhoodServiceObj.GetNeighborhoods().Count();

            // Act
            _pageModel.OnGet();

            // Assert
            Assert.AreEqual(true, _pageModel.ModelState.IsValid);
            Assert.AreEqual(oldCount, TestHelper.NeighborhoodServiceObj.GetNeighborhoods().Count());
        }

        [Test]
        public void OnGet_Valid_Should_Create_New_ID()
        {
            // Arrange
            var oldCount = TestHelper.NeighborhoodServiceObj.GetNeighborhoods().Count();

            // Act
            _pageModel.OnGet();

            // Assert
            Assert.AreEqual(true, _pageModel.ModelState.IsValid);
            Assert.AreEqual(oldCount + 1, _pageModel.Neighborhood.Id);
        }

        #endregion OnGet

        #region OnPostAsync

        [Test]
        public void OnPostAsync_Valid_Should_Get_From_Input_and_Return_Index_Page()
        {

            // ARRANGE: create fake user input data
            var oldCount = TestHelper.NeighborhoodServiceObj.GetNeighborhoods().Count();
            var newCount = oldCount + 1;
            var newName = "newName";
            var newImage = "https://newURL";
            var newShortDesc = "newDesc";

            // Put them in String arrays to match FormCollection Value format
            string[] idArray = { newCount.ToString() };
            string[] nameArray = { newName };
            string[] imageArray = { newImage };
            string[] shortDescArray = { newShortDesc };

            // Create a FromCollection object to hold fake form data
            var formCol = new FormCollection(new Dictionary<string,
            Microsoft.Extensions.Primitives.StringValues>
            {
                { "Neighborhood.Id", idArray},
                { "Neighborhood.Name", nameArray },
                { "Neighborhood.Image", imageArray},
                { "Neighborhood.ShortDesc", shortDescArray}
            });

            // Link FormCollection object with HTTPContext 
            TestHelper.HttpContextDefault.Request.HttpContext.Request.Form = formCol;

            // ACT
            var result = _pageModel.OnPost() as RedirectToPageResult;

            // ASSERT
            Assert.IsNotNull(formCol); 
            Assert.AreEqual(formCol["Neighborhood.Id"][0], newCount.ToString());
            Assert.AreEqual(formCol["Neighborhood.Name"][0], newName);
            Assert.AreEqual(formCol["Neighborhood.Image"][0], newImage);
            Assert.AreEqual(formCol["Neighborhood.ShortDesc"][0], newShortDesc);

            // If success, return Index page
            Assert.NotNull(result);
            Assert.AreEqual(true, result.PageName.Contains("Index"));
        }

        #endregion OnPost
    }
}

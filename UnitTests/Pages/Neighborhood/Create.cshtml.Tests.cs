using LetsGoSEA.WebSite.Pages.Neighborhood;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace UnitTests.Pages.Neighborhood
{
    /// <summary>
    /// Unit test for the Create Page. 
    /// </summary>
    public class CreateTests
    {
        #region TestSetup

        // CreateModel object 
        private static CreateModel _pageModel;

        /// <summary>
        /// Initialize CreateModel private field. 
        /// </summary>
        [SetUp]
        public void TestInitialize()
        {
            _pageModel = new CreateModel(TestHelper.NeighborhoodServiceObj)
            {
                PageContext = TestHelper.PageContext
            };
        }

        #endregion TestSetup

        #region OnPost
        /// <summary>
        /// Tests that when OnPost is called, the Create page gathers Form input and 
        /// returns the Index Page.
        /// </summary>
        [Test]
        public void OnPost_Valid_Should_Get_Form_Input_Should_Return_True()
        {

            // Arrange

            // Create mock user input data. 
            var oldNeighborhoodCount = TestHelper.NeighborhoodServiceObj.GetNeighborhoods().Count();
            var newNeighborhoodCount = oldNeighborhoodCount + 1;
            var newNeighborhoodName = "bogusName";
            var newNeighborhoodImg = "https://via.placeholder.com/150";
            var newShortDesc = "bogusDesc";

            // Store mock user input in String arrays to match FormCollection Value format.
            string[] idArray = { newNeighborhoodCount.ToString() };
            string[] nameArray = { newNeighborhoodName };
            string[] imageArray = { newNeighborhoodImg };
            string[] shortDescArray = { newShortDesc };

            // Create a FormCollection object to hold mock form data.
            var formCol = new FormCollection(new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>
            {
                { "Neighborhood.Id", idArray},
                { "Neighborhood.Name", nameArray },
                { "Neighborhood.Image", imageArray},
                { "Neighborhood.ShortDesc", shortDescArray}
            });

            // Link FormCollection object with HTTPContext.
            TestHelper.HttpContextDefault.Request.HttpContext.Request.Form = formCol;

            // Act
            var result = _pageModel.OnPost() as RedirectToPageResult;

            // Assert
            Assert.IsNotNull(formCol);
            Assert.AreEqual(formCol["Neighborhood.Id"][0], newNeighborhoodCount.ToString());
            Assert.AreEqual(formCol["Neighborhood.Name"][0], newNeighborhoodName);
            Assert.AreEqual(formCol["Neighborhood.Image"][0], newNeighborhoodImg);
            Assert.AreEqual(formCol["Neighborhood.ShortDesc"][0], newShortDesc);

            // If success, return Index page. 
            Assert.NotNull(result);
            Assert.AreEqual(true, result.PageName.Contains("Index"));
        }

        #endregion OnPost
    }
}
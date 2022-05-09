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

    }
}

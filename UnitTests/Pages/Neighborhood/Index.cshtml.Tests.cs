﻿using LetsGoSEA.WebSite.Pages.Neighborhood;
using NUnit.Framework;
using System.Linq;

namespace UnitTests.Pages.Neighborhood

{
    /// <summary>
    /// Unit test for the Index Page.
    /// </summary>
    public class IndexTests
    {
        #region TestSetup

        // IndexModel object.
        private static IndexModel _pageModel;

        /// <summary>
        /// Initialize IndexModel with a NeighborhoodService object. 
        /// </summary>
        [SetUp]
        public void TestInitialize()
        {
            _pageModel = new IndexModel(TestHelper.NeighborhoodServiceObj);
        }

        #endregion TestSetup

        #region OnGet
        /// <summary>
        /// Test that when OnGet is called, all neighborhoods in the database
        /// are returned.
        /// </summary>
        [Test]
        public void OnGet_Valid_Should_Return_Neighborhoods()
        {
            // Arrange

            // Act
            _pageModel.OnGet();

            // Assert
            Assert.AreEqual(true, _pageModel.ModelState.IsValid);
            Assert.AreEqual(true, _pageModel.neighborhoods.ToList().Any());
        }
        #endregion OnGet
    }
}
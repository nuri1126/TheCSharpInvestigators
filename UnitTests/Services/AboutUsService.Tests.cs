
using Microsoft.AspNetCore.Mvc;

using NUnit.Framework;

namespace UnitTests.Services
{
    /// <summary>
    /// Unit test for AboutUs service
    /// </summary>
    public class AboutUsService_Tests
    {
        #region TestSetup
        /// <summary>
        /// SetUp is done in TestHelper.cs file
        /// </summary>
        [SetUp]
        public void TestInitialize()
        {
        }

        #endregion TestSetup

        #region AddRating
        /// <summary>
        /// Valid data model should return true
        /// </summary>
        [Test]
        public void GetAboutUS_Valid_Data_Model_Should_Return_True()
        {
            // Arrange

            // Act
            //Get data model of about us 
            var aboutUs = TestHelper.AboutUsServiceObj.GetAboutUs();

            // Assert
            Assert.AreEqual("LetsGoSEA.WebSite.Models.AboutUsModel[]", aboutUs.ToString());
        }
        #endregion AddRating

    }
}
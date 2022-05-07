
using Microsoft.AspNetCore.Mvc;

using NUnit.Framework;

namespace UnitTests.Services
{
    public class AboutUsService_Tests
    {
        #region TestSetup

        [SetUp]
        public void TestInitialize()
        {
        }

        #endregion TestSetup

        #region AddRating
        [Test]
        public void GetAboutUS_InValid_Data_Model_Should_Return_True()
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
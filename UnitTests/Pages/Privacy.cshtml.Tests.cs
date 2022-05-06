using System.Linq;

using Microsoft.Extensions.Logging;

using Moq;

using NUnit.Framework;

using LetsGoSEA.WebSite.Pages;

namespace UnitTests.Pages
{
    public class PrivacyTests
    {
        #region TestSetup
        public static PrivacyModel pageModel;

        [SetUp]
        public void TestInitialize()
        {
            var MockLoggerDirect = Mock.Of<ILogger<PrivacyModel>>();

            pageModel = new PrivacyModel(MockLoggerDirect)
            {
                PageContext = TestHelper.PageContext,
                TempData = TestHelper.TempData,
            };
        }

        #endregion TestSetup

        #region OnGet
        [Test]
        public void OnGet_Valid_Activity_Set_Should_Return_RequestId()
        {
            // Arrange

            // Act
            pageModel.OnGet();

            // Reset

            // Assert
            Assert.AreEqual(true, pageModel.ModelState.IsValid);
        }

        #endregion OnGet
    }
}
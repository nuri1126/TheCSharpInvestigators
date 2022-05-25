using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace UnitTests.Pages.Startup
{
    public class StartupTests
    {
        #region TestSetup

        [SetUp]
        public void TestInitialize()
        {
        }

        public class Startup : LetsGoSEA.WebSite.Startup
        {
            public Startup(IConfiguration config) : base(config)
            {
            }
        }
        #endregion TestSetup

        #region ConfigureServices

        /// <summary>
        /// Tests that ASP WebHost service is created and not null after ConfigureServices.
        /// </summary>
        [Test]
        public void Startup_ConfigureServices_Valid_Defaut_Should_Pass()
        {
            var webHost = Microsoft.AspNetCore.WebHost.CreateDefaultBuilder().UseStartup<Startup>().Build();
            Assert.IsNotNull(webHost);
        }
        #endregion ConfigureServices

        #region Configure
        /// <summary>
        /// Tests that ASP WebHost service is created and not null.
        /// </summary>
        [Test]
        public void Startup_Configure_Valid_Defaut_Should_Pass()
        {
            var webHost = Microsoft.AspNetCore.WebHost.CreateDefaultBuilder().UseStartup<Startup>().Build();
            Assert.IsNotNull(webHost);
        }

        #endregion Configure
    }
}
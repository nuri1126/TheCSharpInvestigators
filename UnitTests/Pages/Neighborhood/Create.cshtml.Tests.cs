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

    }
}

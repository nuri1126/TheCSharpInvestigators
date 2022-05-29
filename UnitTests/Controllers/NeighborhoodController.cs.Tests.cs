using LetsGoSEA.WebSite.Controllers;
using LetsGoSEA.WebSite.Services;
using NUnit.Framework;

namespace UnitTests.Controllers
{
    /// <summary>
    /// Unit test for NeighborhoodController.
    /// </summary>
    class NeighborhoodControllerTests
    {
        // Global NeighborhodService to use for all test cases. 
        NeighborhoodService _neighborhoodService;

        /// <summary>
        /// Stores the TestHelper's Neighborhood service. 
        /// </summary>
        [SetUp]
        public void TestInitialize()
        {
            _neighborhoodService = TestHelper.NeighborhoodServiceObj;
        }

        #region Constructor

        /// <summary>
        /// Tests that a NeighborhoodController constructor creates a new NeighborhoodController
        /// instance when called when valid input parameter is provided.
        /// </summary>
        [Test]
        public void NeighborhoodController_Valid_New_Controller_Not_Null_Should_Return_True()
        {
            //Arrange

            //Act
            var controller = new NeighborhoodController(_neighborhoodService);

            //Assert
            Assert.NotNull(controller);
        }

        #endregion Constructor

        #region PrivateNeighborhoodService

        /// <summary>
        /// Tests the NeighborhoodController's Service is not null upon instantiation. 
        /// </summary>
        [Test]
        public void NeighborhoodController_Valid_New_Controller_Service_Not_Null_Should_Return_True()
        {
            //Arrange
            var controller = new NeighborhoodController(_neighborhoodService);

            //Act
            var service = controller.neighborhoodService;

            //Assert 
            Assert.NotNull(service);
        }

        #endregion PrivateNeighborhoodService
    }
}
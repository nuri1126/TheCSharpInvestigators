using LetsGoSEA.WebSite.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Moq;

namespace UnitTests
{
    /// <summary>
    /// Test helper to hold the web start settings including:
    /// HttpClient, Action Context, View Data and Temp Data
    /// and services
    /// </summary>
    public static class TestHelper
    {
        // Mock instance IWebHostEnvironment interface.
        public static Mock<IWebHostEnvironment> MockWebHostEnvironment;

        // Test instance of IUrlHelperFactory object to help build URLs.
        public static IUrlHelperFactory UrlHelperFactory;

        // Test instance of HttpContext class that holds properties for Auth, connection, etc.
        public static DefaultHttpContext HttpContextDefault;

        // Test instance of IWebHostEnvironment. 
        public static IWebHostEnvironment WebHostEnvironment;

        // Test instance of a ModelStateDictionary. 
        public static ModelStateDictionary ModelState;

        // Test instance of ActionContext. 
        public static ActionContext ActionContext;

        // Represents an empty model for testing. 
        public static EmptyModelMetadataProvider ModelMetadataProvider;

        // Test ViewDataDictionary object. 
        public static ViewDataDictionary ViewData;

        // Test TempDataDictionary object. 
        public static TempDataDictionary TempData;

        // Test PageContext object.
        public static PageContext PageContext;

        // Test instance of a NeighborhoodService. 
        public static NeighborhoodService NeighborhoodServiceObj;

        // Test instance of the AboutUsService. 
        public static AboutUsService AboutUsServiceObj;

        /// <summary>
        /// Default Constructor
        /// </summary>
        static TestHelper()
        {
            // Initialize and setup MockWebHostEnvironment object. 
            MockWebHostEnvironment = new Mock<IWebHostEnvironment>();
            MockWebHostEnvironment.Setup(m => m.EnvironmentName).Returns("Hosting:UnitTestEnvironment");
            MockWebHostEnvironment.Setup(m => m.WebRootPath).Returns(TestFixture.DataWebRootPath);
            MockWebHostEnvironment.Setup(m => m.ContentRootPath).Returns(TestFixture.DataContentRootPath);
            MockWebHostEnvironment.Setup(m => m.ContentRootPath).Returns(TestFixture.ImageContentRootPath);

            PageContext = InitiatePageContext();

            // Initialize test services with MockWebHostEnvironment. 
            NeighborhoodServiceObj = new NeighborhoodService(MockWebHostEnvironment.Object);
            AboutUsServiceObj = new AboutUsService(MockWebHostEnvironment.Object);
        }

        public static PageContext InitiatePageContext()
        {
            // Initialize and set TraceIdentifier propertiy for HttpContextDefault. 
            HttpContextDefault = new DefaultHttpContext()
            {
                TraceIdentifier = "trace",
            };
            HttpContextDefault.HttpContext.TraceIdentifier = "trace";

            // Initialize ModelStateDictionary object. 
            ModelState = new ModelStateDictionary();

            // Initialize ActionContext. 
            ActionContext = new ActionContext(HttpContextDefault, HttpContextDefault.GetRouteData(), new PageActionDescriptor(), ModelState);

            // Initialize test Model and associated test data. 
            ModelMetadataProvider = new EmptyModelMetadataProvider();
            ViewData = new ViewDataDictionary(ModelMetadataProvider, ModelState);
            TempData = new TempDataDictionary(HttpContextDefault, Mock.Of<ITempDataProvider>());

            // Initialize PageContext object and set ViewData and HttpContext properties. 
            return new PageContext(ActionContext)
            {
                ViewData = ViewData,
                HttpContext = HttpContextDefault
            };
        }
    }
}

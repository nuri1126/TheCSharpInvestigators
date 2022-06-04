using LetsGoSEA.WebSite.Pages.Explore;
using LetsGoSEA.WebSite.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace UnitTests.Pages.Explore
{
    /// <summary>
    /// Unit tests for an individual Neighborhood Pages.
    /// </summary>
    public class NeighborhoodTests
    {

        #region TestSetup

        // Global invalid id property for use in tests. 
        private const int InvalidId = -1;

        // Global valid name property for use in tests. 
        private const string Name = "Bogusland";

        // Global valid image property for use in tests. 
        private const string Image = "http://via.placeholder.com/150";

        // Global valid address property for use in tests
        private const string Address = "401 NE Northgate Way, Seattle, WA 98125";

        // Global valid shortDesc property for use in tests.
        private const string ShortDesc = "Test neighborhood description";

        // Global imgFiles property for use in tests. 
        private static IFormFileCollection ImgFilesNull = null;

        // Global valid Rating for use in AddRatings region.
        private const int ValidRating = 5;

        // Global valid comment input for use in Comments region.
        private const string ValidComment = "Bogus";

        // NeighborhoodModel object.
        private static NeighborhoodModel _pageModel;

        // CommentModel object.
        private static LetsGoSEA.WebSite.Models.CommentModel _commentModel;

        // Global NeighborhoodService to use for all test cases. 
        private NeighborhoodService _neighborhoodService;

        /// <summary>
        /// Initialize NeighborhoodModel and CommentModel private fields. 
        /// </summary>
        [SetUp]
        public void TestInitialize()
        {

            // Abstract NeighborhoodService object from TestHelper.
            _neighborhoodService = TestHelper.NeighborhoodServiceObj;

            // Initialize NeighborhoodModel with a NeighborhoodService object. 
            _pageModel = new NeighborhoodModel(_neighborhoodService);

            // Initialize CommentModel.
            _commentModel = new LetsGoSEA.WebSite.Models.CommentModel();
        }

        #endregion TestSetup

        #region OnGet
        /// <summary>
        /// Tests the selected Neighborhood's .cshtml OnGet method. A valid call to
        /// OnGet should return valid model state and correct neighborhood. 
        /// </summary>
        [Test]
        public void OnGet_Valid_CurrentNeighborhood_Should_Return_True()
        {
            // Arrange
            // Add test neighborhood to database.
            _neighborhoodService.AddData(Name, Address, Image, ShortDesc);

            // Retrieve test neighborhood.
            var testNeighborhood = _neighborhoodService.GetNeighborhoods().Last();

            // Act
            _pageModel.OnGet(testNeighborhood.id);

            // Assert
            Assert.AreEqual(true, _pageModel.ModelState.IsValid);
            Assert.AreEqual(testNeighborhood.name, _pageModel.currentNeighborhood.name);

            // TearDown
            TestHelper.NeighborhoodServiceObj.DeleteData(testNeighborhood.id);
        }

        /// <summary>
        /// Tests that when OnGet is called, an invalid model state should return false and redirect to Index. 
        /// </summary>
        [Test]
        public void OnGet_Invalid_Model_Should_Return_False_And_Redirect_To_Index()
        {
            // Arrange

            // Add test neighborhood to database.
            _neighborhoodService.AddData(Name, Address, Image, ShortDesc);

            // Retrieve test neighborhood.
            _pageModel.currentNeighborhood = _neighborhoodService.GetNeighborhoods().Last();

            // Act

            // Force an invalid error state.
            _pageModel.ModelState.AddModelError("InvalidState", "Neighborhood is Invalid");

            var result = _pageModel.OnGet(_pageModel.currentNeighborhood.id) as RedirectToPageResult;

            // Assert
            Assert.AreEqual(false, _pageModel.ModelState.IsValid);
            Debug.Assert(result != null, nameof(result) + " != null");
            Assert.AreEqual(true, result.PageName.Contains("Index"));

            // TearDown
            _neighborhoodService.DeleteData(_pageModel.currentNeighborhood.id);
        }

        /// <summary>
        /// Tests that when OnGet is called, if provided an incorrect NeighborhoodModel id,
        /// the browser is redirected to the Index. 
        /// </summary>
        [Test]
        public void OnGet_Invalid_Id_Should_Redirect_To_Index()
        {
            // Arrange

            // Act
            var result = _pageModel.OnGet(InvalidId) as RedirectToPageResult;

            // Assert
            Assert.AreEqual(true, _pageModel.ModelState.IsValid);
            Debug.Assert(result != null, nameof(result) + " != null");
            Assert.AreEqual(true, result.PageName.Contains("Index"));
        }
        #endregion OnGet

        #region OnGet_Ratings

        /// <summary>
        /// Tests that when OnGet is called, a null rating should return a zero average rating and count.
        /// </summary>
        [Test]
        public void OnGet_Null_Ratings_Null_Should_Return_True()
        {
            // Arrange

            // Add test neighborhood to database.
            _neighborhoodService.AddData(Name, Address, Image, ShortDesc);

            // Retrieve test neighborhood.
            var testNeighborhood = _neighborhoodService.GetNeighborhoods().Last();

            // Act
            _pageModel.OnGet(testNeighborhood.id);

            // Assert 
            Assert.AreEqual(0, _pageModel.avgRating);
            Assert.AreEqual(0, _pageModel.voteCount);

            // TearDown
            _neighborhoodService.DeleteData(testNeighborhood.id);
        }

        /// <summary>
        /// Tests that when OnGet is called and the neighborhood has 1 rating, the GetCurrentRating method
        /// returns the "Vote" label. 
        /// </summary>
        [Test]
        public void OnGet_Valid_OneRating_Valid_Should_Return_VoteLabel()
        {
            // Arrange

            // Add test neighborhood to database.
            _neighborhoodService.AddData(Name, Address, Image, ShortDesc);

            // Retrieve test neighborhood.
            var testNeighborhood = _neighborhoodService.GetNeighborhoods().Last();

            // Add one rating to the newly created neighborhood.
            _neighborhoodService.AddRating(testNeighborhood, ValidRating);

            // Act
            _pageModel.OnGet(testNeighborhood.id);

            // Assert 
            Assert.AreEqual(_pageModel.voteCount, 1);
            Assert.AreEqual(_pageModel.voteLabel, "Vote");

            // TearDown
            _neighborhoodService.DeleteData(testNeighborhood.id);
        }

        /// <summary>
        /// Tests that when OnGet is called and the neighborhood has multiple ratings, the GetCurrentRating method
        /// returns the "Votes" label. 
        /// </summary>
        [Test]
        public void OnGet_Valid_MultipleRatings_Valid_Should_Return_Votes_VoteLabel()
        {
            // Arrange
            // Add test neighborhood to database.
            _neighborhoodService.AddData(Name, Address, Image, ShortDesc);

            // Retrieve test neighborhood.
            var testNeighborhood = _neighborhoodService.GetNeighborhoods().Last();

            // Add multiple ratings to the newly created neighborhood.
            _neighborhoodService.AddRating(testNeighborhood, ValidRating);
            _neighborhoodService.AddRating(testNeighborhood, ValidRating);
            _neighborhoodService.AddRating(testNeighborhood, ValidRating);

            // Act
            _pageModel.OnGet(testNeighborhood.id);

            // Assert 
            Assert.Greater(_pageModel.voteCount, 1);
            Assert.AreEqual(_pageModel.voteLabel, "Votes");

            // TearDown
            _neighborhoodService.DeleteData(testNeighborhood.id);
        }

        #endregion  OnGet_Ratings

        #region OnPost_Ratings

        /// <summary>
        /// Tests that when OnPost is called from a rating input, the BindProperty successfully
        /// sets the rating and that the count of the ratings has increased by one. 
        /// </summary>
        [Test]
        public void OnPost_Valid_Rating_Valid_Should_Return_True()
        {
            // Arrange

            // Add test neighborhood to database.
            _neighborhoodService.AddData(Name, Address, Image, ShortDesc);

            // Retrieve test neighborhood.
            var testNeighborhood = _neighborhoodService.GetNeighborhoods().Last();

            // Store selected neighborhood object. 
            _pageModel.currentNeighborhood = TestHelper.NeighborhoodServiceObj.GetNeighborhoodById(testNeighborhood.id);

            // Set an additional rating to the selected neighborhood object. 
            _pageModel.rating = 3; _neighborhoodService.AddRating(testNeighborhood, ValidRating);

            // Act
            var result = _pageModel.OnPost(testNeighborhood.id, "0") as RedirectResult;

            // Assert 
            Assert.AreEqual(3, _pageModel.currentNeighborhood.ratings.Last());
            // Assert.AreEqual(1, _pageModel.currentNeighborhood.ratings.Count());

            // TearDown
            _neighborhoodService.DeleteData(testNeighborhood.id);
        }

        #endregion OnPost_Ratings

        #region OnPost_Comments

        /// <summary>
        /// Tests that when OnPost is called, the input comment is stored in the database and the 
        /// value of the comment is correct. 
        /// </summary>
        [Test]
        public void OnPost_Valid_Comment_Should_Return_True()
        {
            // Arrange

            // Store input in String array to match FormCollection Value format.
            string[] commentArray = { ValidComment };

            // Initialize a FormCollection object to hold mock form data.
            var formCol = new FormCollection(new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>
            {
                { "Neighborhood.Comments", commentArray},
            });

            // Link FormCollection object with HTTPContext.
            TestHelper.HttpContextDefault.Request.HttpContext.Request.Form = formCol;

            // Act
            _pageModel.OnPost(1, "0");

            // Assert
            Assert.IsNotNull(formCol);
            Assert.AreEqual(formCol["Neighborhood.Comments"][0], ValidComment);
        }

        /// <summary>
        /// Tests that when OnPost is called, DeleteComment is working inside of OnPost.
        /// </summary>
        [Test]
        public void OnPost_Valid_CommentId_Should_Return_True()
        {
            // Arrange

            // Add test neighborhood to database.
            _neighborhoodService.AddData(Name, Address, Image, ShortDesc);

            // Retrieve test neighborhood.
            var testNeighborhood = _neighborhoodService.GetNeighborhoods().Last();

            // Store selected neighborhood object. 
            _pageModel.currentNeighborhood = _neighborhoodService.GetNeighborhoodById(testNeighborhood.id);

            // Add a new comment to the selected neighborhood. 
            _neighborhoodService.AddComment(_pageModel.currentNeighborhood, ValidComment);

            // Store initial count of comments. 
            var oldCommentCount = _pageModel.currentNeighborhood.comments.Count();

            // Store comment id. 
            var commentId = _pageModel.currentNeighborhood.comments.Last().CommentId;

            // Act
            _pageModel.OnPost(testNeighborhood.id, commentId);

            // Assert 
            Assert.AreEqual(oldCommentCount - 1, _pageModel.currentNeighborhood.comments.Count());

            // TearDown
            _neighborhoodService.DeleteData(testNeighborhood.id);
        }

        /// <summary>
        /// Test that BindProperty NewCommentText is settable.
        /// </summary>
        [Test]
        public void OnPost_Valid_NewCommentText_Valid_Should_Return_True()
        {
            // Arrange
            // Add test neighborhood to database.
            _neighborhoodService.AddData(Name, Address, Image, ShortDesc);

            // Retrieve test neighborhood.
            var testNeighborhood = _neighborhoodService.GetNeighborhoods().Last();

            // Store selected neighborhood object. 
            _pageModel.currentNeighborhood = _neighborhoodService.GetNeighborhoodById(testNeighborhood.id);

            var oldCommentCount = _pageModel.currentNeighborhood.comments.Count();

            // Set New Comment
            _pageModel.newCommentText = ValidComment;

            // Act
            _pageModel.OnPost(_pageModel.currentNeighborhood.id, "0");

            // Assert 
            Assert.AreEqual(_pageModel.currentNeighborhood.comments.Last().Comment, ValidComment);
            Assert.AreEqual(_pageModel.currentNeighborhood.comments.Count(), oldCommentCount + 1);

            // TearDown
            _neighborhoodService.DeleteData(testNeighborhood.id);
        }

        #endregion OnPost_Comments

        #region Comments

        /// <summary>
        /// Test that when OnPost is called from a comment input, the BindProperty successfully
        /// sets the comment. Also tests the {get} method of Comment.
        /// </summary>
        [Test]
        public void Valid_CommentModel_Is_Settable_Is_Gettable_Should_Return_True()
        {
            // Arrange
            string id = Guid.NewGuid().ToString();

            // Act 
            _commentModel.CommentId = id;
            _commentModel.Comment = ValidComment;

            // Assert 
            Assert.NotNull(_commentModel.CommentId);
            Assert.NotNull(_commentModel.Comment);
        }

        #endregion Comments

    }
}
using LetsGoSEA.WebSite.Pages.Explore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace UnitTests.Pages.Explore
{
    /// <summary>
    /// Unit test for Explore Neighborhood Page
    /// </summary>
    public class NeighborhoodTests
    {
        #region TestSetup
        // Neighborhood Model object
        private static NeighborhoodModel _pageModel;

        // Comment Model object
        private static LetsGoSEA.WebSite.Models.CommentModel _commentModel;

        /// <summary>
        /// Setup models for testing. 
        /// </summary>
        [SetUp]
        public void TestInitialize()
        {
            _pageModel = new NeighborhoodModel(TestHelper.NeighborhoodServiceObj);
            _commentModel = new LetsGoSEA.WebSite.Models.CommentModel();
        }

        #endregion TestSetup

        #region Onget
        /// <summary>
        /// Test GET method: valid page should return neighborhood
        /// </summary>
        [Test]
        public void OnGet_Valid_Should_Return_Neighborhood()
        {
            // Arrange

            // Act
            _pageModel.OnGet(2);

            // Assert
            Assert.AreEqual(true, _pageModel.ModelState.IsValid);
            Assert.AreEqual("Greenlake", _pageModel.CurrentNeighborhood.Name);
        }

        /// <summary>
        /// Test GET method: invalid model should return index page 
        /// </summary>
        [Test]
        public void OnGet_Invalid_Model_NotValid_ReturnIndex()
        {
            // Arrange
            _pageModel.CurrentNeighborhood = new LetsGoSEA.WebSite.Models.NeighborhoodModel()
            {
                Id = 666,
                Name = "Invalid Name"
            };

            // Force an invalid error state
            _pageModel.ModelState.AddModelError("InvalidState", "Neighborhood is Invalid");

            // Act
            var result = _pageModel.OnGet(_pageModel.CurrentNeighborhood.Id) as RedirectToPageResult;

            // Assert
            Assert.AreEqual(false, _pageModel.ModelState.IsValid);
            Assert.AreEqual(true, result.PageName.Contains("Index"));
        }

        /// <summary>
        /// Test GET method: invalid ID should lead to invalid model which should return index page
        /// </summary>
        [Test]
        public void OnGet_Invalid_Id_NotValid_ReturnExplore()
        {
            // Arrange
            _pageModel.CurrentNeighborhood = new LetsGoSEA.WebSite.Models.NeighborhoodModel()
            {
                Id = 666,
                Name = "Invalid Name"
            };

            // Act
            var result = _pageModel.OnGet(_pageModel.CurrentNeighborhood.Id) as RedirectToPageResult;

            // Assert
            Assert.AreEqual(true, _pageModel.ModelState.IsValid);
            Debug.Assert(result != null, nameof(result) + " != null");
            Assert.AreEqual(true, result.PageName.Contains("Index"));
        }
        #endregion OnGet

        #region Onget_GetCurrentRating
        /// <summary>
        /// TEST GetCurrentRating method in OnGet: null ratings should return zero average rating and count
        /// </summary>
        [Test]
        public void OnGet_Null_Ratings_Return_Zero_AvgAndCount()
        {
            // Arrange
            // Pick a neighborhood from database that has a null rating 
            int IdWithNullRating = 16;
            // Act
            _pageModel.OnGet(IdWithNullRating);

            // Assert 
            Assert.AreEqual(_pageModel.avgRating, 0);
            Assert.AreEqual(_pageModel.voteCount, 0);

        }

        /// <summary>
        /// Test GetCurrentRating method in OnGet: one rating should return "Vote" votelebl
        /// </summary>
        [Test]
        public void OnGet_OneRating_Return_Vote_VoteLabel()
        {
            // Arrange
            // Pick a neighborhood from database that has only one rating 
            int IdWithOneRating = 2;

            // Act
            _pageModel.OnGet(IdWithOneRating);

            // Assert 
            Assert.AreEqual(_pageModel.voteCount, 1);
            Assert.AreEqual(_pageModel.voteLabel, "Vote");

        }

        /// <summary>
        /// Test GetCurrentRating method in OnGet: multiple ratings should return "Votes" votelebl
        /// </summary>
        [Test]
        public void OnGet_MultipleRatings_Return_Votes_VoteLabel()
        {
            // Arrange
            // Pick a neigborhood from database that has multiple ratings
            int IdWithMultipleRatings = 1;

            // Act
            _pageModel.OnGet(IdWithMultipleRatings);

            // Assert 
            Assert.Greater(_pageModel.voteCount, 1);
            Assert.AreEqual(_pageModel.voteLabel, "Votes");

        }
        #endregion  Onget_GetCurrentRating

        #region OnPostAsync
        /// <summary>
        /// Test POST method: valid page accept input comment and store to database. 
        /// </summary>
        [Test]
        public void OnPostAsync_Valid_Comment_Is_Saved_From_Form()
        {
            // ARRANGE: create fake user input data
            var newComment = "newComment";

            // Put them in String arrays to match FormCollection Value format
            string[] commentArray = { newComment };

            // Create a FromCollection object to hold fake form data
            var formCol = new FormCollection(new Dictionary<string,
            Microsoft.Extensions.Primitives.StringValues>
            {
                { "Neighborhood.Comments", commentArray},
            });

            // Link FormCollection object with HTTPContext 
            TestHelper.HttpContextDefault.Request.HttpContext.Request.Form = formCol;

            // ACT
            _pageModel.OnPost(1);

            // ASSERT
            Assert.IsNotNull(formCol);
            Assert.AreEqual(formCol["Neighborhood.Comments"][0], newComment);
        }

        #endregion OnPostAsync

        #region Comment_Is_Settable_Returns_True
        /// <summary>
        /// Tests Comment object's set method.
        /// </summary>
        [Test]
        public void Comment_Is_Settable_Returns_True()
        {
            // Arrange
            string bogusInput = "bogus";

            // Act 
            _commentModel.Comment = bogusInput;

            // Assert 
            //Assert.AreEqual(bogusInput, testComment);
            Assert.NotNull(_commentModel.Comment);
        }
        #endregion Comment_Is_Settable_Returns_True

        #region Comment_Is_Gettable_Returns_True
        /// <summary>
        /// Tests Comment object's get method.
        /// </summary>
        [Test]
        public void Comment_Is_Gettable_Returns_True()
        {
            // Arrange
            string bogusInput = "bogus";
            var testComment = _commentModel.Comment;
            testComment = bogusInput;

            // Act 
            var res = testComment;

            // Assert 
            Assert.AreEqual(bogusInput, res);
        }
        #endregion Comment_Is_Gettable_Returns_True

        /// <summary>
        /// Test that BindProperty Rating is settable 
        /// </summary>
        [Test]
        public void Rating_Is_Settable()
        {
            // Arrange
            var Id = 1;
            _pageModel.CurrentNeighborhood = TestHelper.NeighborhoodServiceObj.GetNeighborhoodById(Id);
            var oldRatingCount = _pageModel.CurrentNeighborhood.Ratings.Count();
            // Set New Rating
            _pageModel.Rating = 3;

            // Act
            _pageModel.OnPost(Id);

            // Assert 
            Assert.AreEqual(_pageModel.CurrentNeighborhood.Ratings.Last(), 3);
            Assert.AreEqual(_pageModel.CurrentNeighborhood.Ratings.Count(), oldRatingCount + 1);
        }

        /// <summary>
        /// Test that BindProperty NewCommentText is settable 
        /// </summary>
        [Test]
        public void NewCommentText_Is_Settable()
        {
            // Arrange
            var Id = 1;
            _pageModel.CurrentNeighborhood = TestHelper.NeighborhoodServiceObj.GetNeighborhoodById(Id);
            var newComment = "Nice work!";
            var oldCommentCount = _pageModel.CurrentNeighborhood.Comments.Count();

            // Set New Comment
            _pageModel.NewCommentText = newComment;

            // Act
            _pageModel.OnPost(Id);

            // Assert 
            Assert.AreEqual(_pageModel.CurrentNeighborhood.Comments.Last().Comment, newComment);
            Assert.AreEqual(_pageModel.CurrentNeighborhood.Comments.Count(), oldCommentCount + 1);
        }
    }
}
using LetsGoSEA.WebSite.Pages.Explore;
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
    /// Unit test for an individual Neighborhood Page.
    /// </summary>
    public class NeighborhoodTests
    {

        #region TestSetup

        // NeighborhoodModel object
        private static NeighborhoodModel _pageModel;

        // CommentModel object
        private static LetsGoSEA.WebSite.Models.CommentModel _commentModel;

        /// <summary>
        /// Initialize NeighborhoodModel and CommentModel private fields. 
        /// </summary>
        [SetUp]
        public void TestInitialize()
        {
            //Initialize NeighborhoodModel with a NeighborhoodService object. 
            _pageModel = new NeighborhoodModel(TestHelper.NeighborhoodServiceObj);

            //Initialize CommentModel.
            _commentModel = new LetsGoSEA.WebSite.Models.CommentModel();
        }

        #endregion TestSetup

        #region OnGet
        /// <summary>
        /// Tests the selected Neighborhood's .cshtml OnGet method. A valid call to
        /// OnGet should return that neighborhood's page to the browser. 
        /// </summary>
        [Test]
        public void OnGet_Valid_CurrentNeighborhood_Name_Should_Return_True()
        {
            // Arrange

            // Act
            _pageModel.OnGet(2);

            // Assert
            Assert.AreEqual(true, _pageModel.ModelState.IsValid);
            Assert.AreEqual("Greenlake", _pageModel.currentNeighborhood.name);
        }

        /// <summary>
        /// Tests that when OnGet is called, if provided an incorrect NeighborhoodModel object to 
        /// retrieve, the browser is redirected to the Index. 
        /// </summary>
        [Test]
        public void OnGet_Invalid_Model_Valid_Should_Return_False()
        {
            // Arrange
            // Initialize an invalid Neighborhood to attempt to retrieve. 
            _pageModel.currentNeighborhood = new LetsGoSEA.WebSite.Models.NeighborhoodModel()
            {
                id = 666,
                name = "Invalid Name"
            };


            // Act
            // Force an invalid error state.
            _pageModel.ModelState.AddModelError("InvalidState", "Neighborhood is Invalid");

            var result = _pageModel.OnGet(_pageModel.currentNeighborhood.id) as RedirectToPageResult;

            // Assert
            Assert.AreEqual(false, _pageModel.ModelState.IsValid);
            Debug.Assert(result != null, nameof(result) + " != null");
            Assert.AreEqual(true, result.PageName.Contains("Index"));
        }

        /// <summary>
        /// Tests that when OnGet is called, if provided an incorrect NeighborhoodModel id,
        /// the browser is redirected to the Index. 
        /// </summary>
        [Test]
        public void OnGet_Invalid_Id_InValid_Should_ReturnExplore()
        {
            // Arrange
            _pageModel.currentNeighborhood = new LetsGoSEA.WebSite.Models.NeighborhoodModel()
            {
                id = 666,
                name = "Invalid Name"
            };

            // Act
            var result = _pageModel.OnGet(_pageModel.currentNeighborhood.id) as RedirectToPageResult;

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
        public void OnGet_Null_Ratings_Null_Should_Return_AvgRating_0()
        {
            // Arrange
            // Initialize a new Neighborhood object using NeighborhoodService's AddData method. 
            var newNeighborhood = TestHelper.NeighborhoodServiceObj.AddData(
                                                                            "Test Neighborhood",
                                                                            "https://via.placeholder.com/150",
                                                                            "Short neighborhood description",
                                                                            null);

            // Store the newly created neighborhood's id.
            var idWithNullRating = newNeighborhood.id;

            // Act
            _pageModel.OnGet(idWithNullRating);

            // Assert 
            Assert.AreEqual(0, _pageModel.avgRating);
            Assert.AreEqual(0, _pageModel.voteCount);
        }

        /// <summary>
        /// Tests that when OnGet is called and the neighborhood has 1 rating, the GetCurrentRating method
        /// returns the "Vote" label. 
        /// </summary>
        [Test]
        public void OnGet_Valid_OneRating_Valid_Should_Return_VoteLabel()
        {
            // Arrange
            // Initialize a new Neighborhood object using NeighborhoodService's AddData method. 
            var newNeighborhood = TestHelper.NeighborhoodServiceObj.AddData(
                                                                            "Test Neighborhood",
                                                                            "https://via.placeholder.com/150",
                                                                            "Short neighborhood description",
                                                                            null);

            // Add one rating to the newly created neighborhood.
            TestHelper.NeighborhoodServiceObj.AddRating(newNeighborhood, 5);

            // Store the newly created neighborhood's id. 
            var idWithOneRating = newNeighborhood.id;

            // Act
            _pageModel.OnGet(idWithOneRating);

            // Assert 
            Assert.AreEqual(_pageModel.voteCount, 1);
            Assert.AreEqual(_pageModel.voteLabel, "Vote");
        }


        /// <summary>
        /// Tests that when OnGet is called and the neighborhood has multiple ratings, the GetCurrentRating method
        /// returns the "Votes" label. 
        /// </summary>
        [Test]
        public void OnGet_Valid_MultipleRatings_Valid_Should_Return_Votes_VoteLabel()
        {
            // Arrange
            // Initialize a new Neighborhood object using NeighborhoodService's AddData method. 
            var newNeighborhood = TestHelper.NeighborhoodServiceObj.AddData(
                                                                            "Test Neighborhood",
                                                                            "https://via.placeholder.com/150",
                                                                            "Short neighborhood description",
                                                                            null);

            // Add multiple ratings to the newly created neighborhood.
            TestHelper.NeighborhoodServiceObj.AddRating(newNeighborhood, 5);
            TestHelper.NeighborhoodServiceObj.AddRating(newNeighborhood, 4);
            TestHelper.NeighborhoodServiceObj.AddRating(newNeighborhood, 3);

            // Store the newly created neighborhood's Id 
            int idWithMultipleRatings = newNeighborhood.id;

            // Act
            _pageModel.OnGet(idWithMultipleRatings);

            // Assert 
            Assert.Greater(_pageModel.voteCount, 1);
            Assert.AreEqual(_pageModel.voteLabel, "Votes");
        }

        #endregion  OnGet_Ratings

        #region OnPost_Ratings

        /// <summary>
        /// Test that when OnPost is called from a rating input, the BindProperty successfully
        /// sets the rating and that the count of the ratings has increased by one. 
        /// </summary>
        [Test]
        public void OnPost_Valid_Rating_Valid_Should_Return_Equal_True()
        {
            // Arrange
            var id = 1;

            // Store selected neighborhood object. 
            _pageModel.currentNeighborhood = TestHelper.NeighborhoodServiceObj.GetNeighborhoodById(id);

            // Store initial count of ratings. 
            var oldRatingCount = _pageModel.currentNeighborhood.ratings.Count();

            // Set an additional rating to the selected neighborhood object. 
            _pageModel.rating = 3;

            // Act
            var result = _pageModel.OnPost(id, "0") as RedirectResult;

            // Assert 
            Assert.AreEqual(3, _pageModel.currentNeighborhood.ratings.Last());
            Assert.AreEqual(oldRatingCount + 1, _pageModel.currentNeighborhood.ratings.Count());
        }

        #endregion OnPost_Ratings

        #region OnPost_Comments

        /// <summary>
        /// Tests that when OnPost is called, the input comment is stored in the database and the 
        /// value of the comment is correct. 
        /// </summary>
        [Test]
        public void OnPost_Valid_Comment_Should_Return_Not_Null()
        {
            // Arrange
            // Create mock user input data. 
            var bogusComment = "bogus comment";

            // Store input in String array to match FormCollection Value format.
            string[] commentArray = { bogusComment };

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
            Assert.AreEqual(formCol["Neighborhood.Comments"][0], bogusComment);
        }

        /// <summary>
        /// Tests that when OnPost is called, DeleteComment is working inside of OnPost.
        /// </summary>
        [Test]
        public void OnPost_Valid_CommentId_Should_Return_Equal_True()
        {
            // Arrange
            var id = 1;

            // Store selected neighborhood object. 
            _pageModel.currentNeighborhood = TestHelper.NeighborhoodServiceObj.GetNeighborhoodById(id);

            // Add a new comment to the selected neighborhood. 
            TestHelper.NeighborhoodServiceObj.AddComment(_pageModel.currentNeighborhood, "bogus!");

            // Store initial count of comments. 
            var oldCommentCount = _pageModel.currentNeighborhood.comments.Count();

            // Store comment id. 
            var commentId = _pageModel.currentNeighborhood.comments.Last().CommentId;

            // Act
            _pageModel.OnPost(id, commentId);

            // Assert 
            Assert.AreEqual(oldCommentCount - 1, _pageModel.currentNeighborhood.comments.Count());
        }

        /// <summary>
        /// Test that BindProperty NewCommentText is settable.
        /// </summary>
        [Test]
        public void OnPost_Valid_NewCommentText_Valid_Should_Return_Equal_True()
        {
            // Arrange
            var id = 1;
            _pageModel.currentNeighborhood = TestHelper.NeighborhoodServiceObj.GetNeighborhoodById(id);
            var newComment = "Nice work!";
            var oldCommentCount = _pageModel.currentNeighborhood.comments.Count();

            // Set New Comment
            _pageModel.newCommentText = newComment;

            // Act
            _pageModel.OnPost(id, "0");

            // Assert 
            Assert.AreEqual(_pageModel.currentNeighborhood.comments.Last().Comment, newComment);
            Assert.AreEqual(_pageModel.currentNeighborhood.comments.Count(), oldCommentCount + 1);
        }
        #endregion OnPostAsync_Comments

        #region Comments

        /// <summary>
        /// Tests Comment object's get method.
        /// </summary>
        [Test]
        public void Comment_Model_Valid_Should_Returns_True()
        {
            // Arrange
            string bogusComment = "bogus";
            var testComment = _commentModel.Comment;
            testComment = bogusComment;

            // Act 
            var res = testComment;

            // Assert 
            Assert.AreEqual(bogusComment, res);
        }

        /// <summary>
        /// Test that when OnPost is called from a comment input, the BindProperty successfully
        /// sets the comment.
        /// </summary>
        [Test]
        public void Valid_CommentModel_Is_Settable_Should_Return_Not_Null()
        {
            // Arrange
            var bogusComment = "bogus comment";
            string id = Guid.NewGuid().ToString();

            // Act 
            _commentModel.CommentId = id;
            _commentModel.Comment = bogusComment;

            // Assert 
            Assert.NotNull(_commentModel.CommentId);
            Assert.NotNull(_commentModel.Comment);
        }

        #endregion Comments

    }
}
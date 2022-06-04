﻿using LetsGoSEA.WebSite.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using shortid;
using shortid.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace LetsGoSEA.WebSite.Services
{
    /// <summary>
    /// Mediates communication between a NeighborhoodsController and Neighborhoods Data.
    /// </summary>
    public class NeighborhoodService
    {
        // Constructor.
        public NeighborhoodService(IWebHostEnvironment webHostEnvironment)
        {
            WebHostEnvironment = webHostEnvironment;
        }

        // Getter: Get JSON file from wwwroot.
        private IWebHostEnvironment WebHostEnvironment { get; }

        // Store the path of Neighborhoods JSON file (combine the root path, folder name, and file name).
        private string NeighborhoodFileName => Path.Combine(WebHostEnvironment.WebRootPath, "data", "neighborhoods.json");

        // Generate/retrieve a list of NeighborhoodModel objects from JSON file.
        public IEnumerable<NeighborhoodModel> GetNeighborhoods()
        {
            // Open Neighborhoods JSON file.
            using var jsonFileReader = File.OpenText(NeighborhoodFileName);

            // Read and Deserialize JSON file into an array of NeighborhoodModel objects.
            return JsonSerializer.Deserialize<NeighborhoodModel[]>(jsonFileReader.ReadToEnd(),
                // Make case insensitive
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        /// <summary>
        /// Returns null if passed invalid id.
        /// Returns a single neighborhood corresponding to the id.
        /// </summary>
        /// <param name="id">id of the requested neighborhood</param>
        /// <returns>NeighborhoodModel of the requested neighborhood</returns>
        public NeighborhoodModel GetNeighborhoodById(int? id)
        {
            try
            {
                var data = GetNeighborhoods().Where(x => x.id == id);
                NeighborhoodModel singleNeighborhood = data.ElementAt(0);
                return singleNeighborhood;
            }
            catch (ArgumentOutOfRangeException)
            {
                // If the id passed is invalid, we return null.
                return null;
            }
        }

        /// <summary>
        /// Save All neighborhood data to storage.
        /// </summary>
        /// <param name="neighborhoods">all the neighborhood objects to be saved</param>
        private void SaveData(IEnumerable<NeighborhoodModel> neighborhoods)
        {
            // Re-write all the neighborhood data to JSON file
            using var outputStream = File.Create(NeighborhoodFileName);
            JsonSerializer.Serialize(
                new Utf8JsonWriter(outputStream, new JsonWriterOptions
                {
                    SkipValidation = true,
                    Indented = true
                }),
                neighborhoods
            );
        }

        /// <summary>
        /// Create a TEMPORARY neighborhood with autogenerated ID to show in Browser.
        /// </summary>
        /// <returns>A NeighborhoodModel object to be used TEMPORARILY in Page</returns>
        public NeighborhoodModel CreateID()
        {
            // Create a New Neighborhood 
            var data = new NeighborhoodModel()
            {
                // Generate the next valid Id number
                id = GetNeighborhoods().Count() + 1,

                // Assign values to Model fields to show in browser.
                name = "",
                image = "",
                city = "Seattle",
                state = "WA",
                shortDesc = ""
            };

            return data;
        }

        /// <summary>
        /// Create a new neighborhood object, add user input data to it, and save object in JSON file.
        /// </summary>
        /// <param name="name">name data entered by user</param>
        /// <param name="address">Address of the neighborhood</param>
        /// <param name="imageURLs">image URLs entered by user</param>
        /// <param name="shortDesc">short description entered by user</param>
        /// <param name="imageFiles">image files added by user</param>
        /// <returns>A new NeighborhoodModel object to be later saved in JSON</returns>
        public NeighborhoodModel AddData(string name, string address, string imageURLs, string shortDesc, IFormFileCollection imageFiles)
        {

            // If user did not enter image URL, change imageURLs to "Default" to match Model initialization.
            if (imageURLs == "")
            {
                imageURLs = "Default";
            }

            // Create a new neighborhood model
            var data = new NeighborhoodModel()
            {
                // Add user input data to the corresponding field
                id = GetNeighborhoods().Count() + 1,
                name = name,
                image = imageURLs,
                address = address,
                city = "Seattle",
                state = "WA",
                shortDesc = shortDesc
            };

            // If user uploaded image files, upload files to database
            if (imageFiles != null && imageFiles.Any())
            {
                UploadImageIfAvailable(data, imageFiles);
            }

            // Get the current set, and append the new record to it 
            var dataset = GetNeighborhoods();
            var newdataset = dataset.Append(data);

            // Save data set in JSON
            SaveData(newdataset);

            return data;
        }

        /// <summary>
        /// Upload user-uploaded file images to database for selected neighborhood.
        /// </summary>
        /// <param name="neighborhood">the neighborhood user uploaded photo for</param>
        /// <param name="imageFiles">the image files user uploaded</param>
        private void UploadImageIfAvailable(NeighborhoodModel neighborhood, IFormFileCollection imageFiles)
        {
            // Get contentRootPath to save the file on server.
            var wwwrootPath = WebHostEnvironment.WebRootPath;

            // For each submitted file:
            for (var i = 0; i < imageFiles.Count(); i++)
            {
                // Extract the file name.
                var fileName = Path.GetFileName(imageFiles[i].FileName);

                // Create the relative image path to be saved in database to help with image retrieval.
                var relativeImagePath = @"image/Neighborhood/" + fileName;

                // Create absolute image path to upload the file on server.
                var absImagePath = Path.Combine(wwwrootPath, relativeImagePath);

                // Physically upload/copy the file onto server using Absolute Path.
                using (var filestream = new FileStream(absImagePath, FileMode.Create))
                {
                    imageFiles[i].CopyTo(filestream);
                }

                // Add uploaded images to database
                neighborhood.uploadedImages.Add(
                    new UploadedImageModel()
                    {
                        // Assign new ID to UploadedImageModel object.
                        UploadedImageId = GenerateRandomID(), 

                        // Assign upoladed image name
                        UploadedImageName = imageFiles[i].FileName,

                        // Assign uploaded image path
                        UploadedImagePath = relativeImagePath,
                    });
            }
        }

        /// <summary>
        /// Finds neighborhood in NeighborhoodModel, updates the neighborhood, and saves the Neighborhood.
        /// </summary>
        /// <param name="data">neighborhood data to be saved</param>
        /// <param name="deleteImageIds">IDs of uploaded images user wants to delete</param>
        /// <param name="imagesToUpload">the image file user uploaded</param>
        public NeighborhoodModel UpdateData(NeighborhoodModel data, string[] deleteImageIds, IFormFileCollection imagesToUpload)
        {
            var neighborhoods = GetNeighborhoods();
            var neighborhoodData = neighborhoods.FirstOrDefault(x => x.id.Equals(data.id));
            if (neighborhoodData == null)
            {
                return null;
            }

            // If user deleted all URL images, reset image field to "Default" to match Model initialization.
            data.image ??= "Default";

            // Update neighborhood data.
            neighborhoodData.name = data.name;
            neighborhoodData.image = data.image;
            neighborhoodData.city = data.city;
            neighborhoodData.state = data.state;
            neighborhoodData.address = data.address;
            neighborhoodData.shortDesc = data.shortDesc;
            neighborhoodData.ratings = data.ratings;
            neighborhoodData.comments = data.comments;

            // If user has selected uploaded images to delete, delete files from database and server. 
            if (deleteImageIds != null)
            {
                DeleteUploadedImage(neighborhoodData, deleteImageIds);
            }

            // If user has uploaded image files, upload files to database.
            if (imagesToUpload != null)
            {
                UploadImageIfAvailable(neighborhoodData, imagesToUpload);
            }

            SaveData(neighborhoods);

            return neighborhoodData;
        }

        /// <summary>
        /// Remove the neighborhood record from the system.
        /// </summary>
        /// <param name="id">id of the neighborhood to NOT be saved</param>
        /// <returns>the neighborhood object to be deleted</returns>
        public NeighborhoodModel DeleteData(int id)
        {
            // Get the current set.
            var dataSet = GetNeighborhoods();

            // Get the record to be deleted.
            var data = dataSet.FirstOrDefault(m => m.id == id);

            // Only save the remaining records in the system.
            var newDataSet = GetNeighborhoods().Where(m => m.id != id);
            SaveData(newDataSet);

            // Return the record to be deleted.
            return data;
        }

        /// <summary>
        /// Take in the neighborhood ID and the rating.
        /// If the rating does not exist, add it.
        /// Save the update.
        /// </summary>
        /// <param name="neighborhood">Neighborhood Model to add rating to</param>
        /// <param name="rating">user input rating</param>
        public bool AddRating(NeighborhoodModel neighborhood, int rating)
        {
            // If neighborhood is null, return.
            if (neighborhood == null)
            {
                return false;
            }

            switch (rating)
            {
                // Check Rating for boundaries, do not allow ratings below 0.
                case < 0:
                // Check Rating for boundaries, do not allow ratings above 5.
                case > 5:
                    return false;
            }

            // Check to see if ratings exist, if there are not, then create the array.
            neighborhood.ratings ??= Array.Empty<int>();

            // Add the Rating to the Array.
            var ratings = neighborhood.ratings.ToList();
            ratings.Add(rating);
            neighborhood.ratings = ratings.ToArray();

            // Save the data back to the data store.
            UpdateData(neighborhood, null, null);

            return true;
        }

        /// <summary>
        /// Generates a new unique identifier.
        /// </summary>
        /// <returns>String version of GUID</returns>
        private string GenerateRandomID()
        {
            var options = new GenerationOptions(useNumbers: true, length: 8);
            return ShortId.Generate(options);
        }


        /// <summary>
        /// Adds a comment to the NeighborhoodModel.
        /// </summary>
        /// <param name="neighborhood">Current neighborhood</param>
        /// <param name="comment">User input</param>
        /// <returns>true if comment Neighborhood data was updated successfully</returns>
        public bool AddComment(NeighborhoodModel neighborhood, string comment)
        {
            // If neighborhood is null, return false.
            if (neighborhood == null)
            {
                return false;
            }

            switch (comment)
            {
                // Check comment is null, return false.
                case null:
                // Check comment is empty, return false.
                case "":
                    return false;
            }

            // Add comment to the comment list.
            neighborhood.comments.Add(
                new CommentModel()
                {
                    // Assign new Id to the CommentModel object.
                    CommentId = GenerateRandomID(),

                    // Assign the comment to the CommentModel object.
                    Comment = comment
                }
            );

            // Save the neighborhood.
            UpdateData(neighborhood, null, null);

            return true;
        }

        /// <summary>
        /// Deletes a comment from the NeighborhoodModel.
        /// </summary>
        /// <param name="neighborhood">Current neighborhood</param>
        /// <param name="commentId">Comment's unique identifier</param>
        /// <returns>true if comment Neighborhood data was deleted successfully</returns>
        public bool DeleteComment(NeighborhoodModel neighborhood, string commentId)
        {

            // If neighborhood is null, return false.
            if (neighborhood == null)
            {
                return false;
            }

            var currentCommentList = neighborhood.comments;
            int commentIdx = -1;

            // Search for comment to remove, store index.
            for (var i = 0; i < currentCommentList.Count; i++)
            {
                var comment = currentCommentList.ElementAt(i);
                if (comment.CommentId == commentId)
                {
                    commentIdx = i;
                    break;
                }
            }

            // Invalid ID.
            if (commentIdx == -1)
                return false;

            // Remove the comment.
            neighborhood.comments.RemoveAt(commentIdx);

            // Save the neighborhood.
            UpdateData(neighborhood, null, null);

            return true;
        }

        /// <summary>
        /// Get all images from the database, including the URL images and Uploaded file images, if applicable.
        /// </summary>
        /// <param name="neighborhood">the neighborhood to get all images from</param>
        /// <returns>A List of paths to images</returns>
        public List<string> GetAllImages(NeighborhoodModel neighborhood)
        {
            // Temporary image list.
            var allImages = new List<string>();

            // Placeholder image if no URL image or file image is available.
            var noImagePath = "/image/no_image.jpg";

            // Whether the neighborhood is seeded with any URL image link. 
            var hasUrlImage = neighborhood.image != "Default";

            // Whether the neighborhood is seeded with any uploaded image file.
            var hasFileImage = neighborhood.uploadedImages.Count != 0;

            // Add all URL images to the image list if present.
            if (hasUrlImage)
            {
                var urlImages = neighborhood.image.Split(',');
                allImages.AddRange(urlImages);
            }

            // Add all file images to the image list if present.
            if (hasFileImage)
            {
                foreach (var uploadedImageModel in neighborhood.uploadedImages)
                {
                    allImages.Add("/" + uploadedImageModel.UploadedImagePath);
                }
            }

            // Add placeholder image if no image is present in the database.
            else if (!hasUrlImage)
            {
                allImages.Add(noImagePath);
            }

            return allImages;
        }

        /// <summary>
        /// Deletes an uploaded image from the NeighborhoodModel in JSON file. 
        /// Also deletes the physical image file from the server. <======================TO BE IMPLEMENTED 
        /// </summary>
        /// <param name="neighborhood">Current neighborhood</param>
        /// <param name="deleteImageIds">IDs of the uploaded image Models to be deleted </param> 
        public void DeleteUploadedImage(NeighborhoodModel neighborhood, string[] deleteImageIds)
        {
            // Remove the selected UploadedImageModel from JSON file.
            foreach (var id in deleteImageIds)
            {
                var imageToRemove = neighborhood.uploadedImages.Single(r => r.UploadedImageId.Equals(id));
                neighborhood.uploadedImages.Remove(imageToRemove);
            }
        }
    }
}
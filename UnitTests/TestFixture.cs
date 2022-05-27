using NUnit.Framework;
using System.IO;

namespace UnitTests
{
    /// <summary>
    /// Test fixture set up class to compliment TestHelper.
    /// </summary>
    [SetUpFixture]
    public class TestFixture
    {
        // Holds the path to the Web Root.
        public static string DataWebRootPath = "./wwwroot";

        // Holds the path to the data folder for the content.
        public static string DataContentRootPath = "./data/";

        // Holds the path to image folder for the content.
        public static string ImageContentRootPath = "./image/Neighborhood";

        /// <summary>
        /// Pre-test setup function that makes copies of the current state 
        /// of the database for use by the TestHelper. 
        /// </summary>
        [OneTimeSetUp]
        public void RunBeforeAnyTests()
        {

            // Copy over the latest version of the database files.
            var DataWebPath = "../../../../src/wwwroot/data";
            var ImageWebPath = "../../../../src/wwwroot/image/Neighborhood";
            var DataUTDirectory = "wwwroot";
            var dataUtPath = DataUTDirectory + "/data";
            var imageUtPath = DataUTDirectory + "/image/Neighborhood";

            // Delete the destination folder.
            if (Directory.Exists(DataUTDirectory))
            {
                Directory.Delete(DataUTDirectory, true);
            }

            // Make the data directory.
            Directory.CreateDirectory(dataUtPath);

            // Copy over all data files.
            var filePaths = Directory.GetFiles(DataWebPath);
            foreach (var filename in filePaths)
            {
                var newFilePathName = filename.Replace(DataWebPath, dataUtPath);

                File.Copy(filename, newFilePathName);
            }

            // Make the image directory.
            Directory.CreateDirectory(imageUtPath);

            // Copy over all image files.
            var imagePaths = Directory.GetFiles(ImageWebPath);
            foreach (var imageFileName in imagePaths)
            {
                var newFilePathName = imageFileName.Replace(ImageWebPath, dataUtPath);

                File.Copy(imageFileName, newFilePathName);
            }

        }

        [OneTimeTearDown]
        public void RunAfterAnyTests()
        {
        }
    }
}
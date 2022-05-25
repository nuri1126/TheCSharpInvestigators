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
        // Path to the Web Root
        public static string DataWebRootPath = "./wwwroot";

        // Path to the data folder for the content
        public static string DataContentRootPath = "./data/";

        // Path to image folder for the content
        public static string ImageContentRootPath = "./image/Neighborhood";

        [OneTimeSetUp]
        public void RunBeforeAnyTests()
        {
            // Run this code once when the test harness starts up.

            // This will copy over the latest version of the database files

            var DataWebPath = "../../../../src/wwwroot/data";
            var ImageWebPath = "../../../../src/wwwroot/image/Neighborhood";
            // old
            // var DataWebPath = "../../../../src/bin/Debug/net5.0/wwwroot/data";
            var DataUTDirectory = "wwwroot";
            var dataUtPath = DataUTDirectory + "/data";
            var imageUtPath = DataUTDirectory + "/image/Neighborhood";

            // Delete the Destination folder
            if (Directory.Exists(DataUTDirectory))
            {
                Directory.Delete(DataUTDirectory, true);
            }

            // Make the data directory
            Directory.CreateDirectory(dataUtPath);

            // Copy over all data files
            var filePaths = Directory.GetFiles(DataWebPath);
            foreach (var filename in filePaths)
            {

                var newFilePathName = filename.Replace(DataWebPath, dataUtPath);

                File.Copy(filename, newFilePathName);
            }

            // Make the image directory
            Directory.CreateDirectory(imageUtPath);

            // Copy over all image files
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
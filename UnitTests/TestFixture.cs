using System.IO;

using NUnit.Framework;

namespace UnitTests
{
    [SetUpFixture]
    public class TestFixture
    {
        // Path to the Web Root
        public static string DataWebRootPath = "./wwwroot";

        // Path to the data folder for the content
        public static string DataContentRootPath = "./data/";

        [OneTimeSetUp]
        public void RunBeforeAnyTests()
        {
            // Run this code once when the test harness starts up.

            // This will copy over the latest version of the database files

            var DataWebPath = "../../../../src/wwwroot/data";
            // old
            // var DataWebPath = "../../../../src/bin/Debug/net5.0/wwwroot/data";
            var DataUTDirectory = "wwwroot";
            var dataUtPath = DataUTDirectory + "/data";

            // Delete the Destination folder
            if (Directory.Exists(DataUTDirectory))
            {
                Directory.Delete(DataUTDirectory, true);
            }

            // Make the directory
            Directory.CreateDirectory(dataUtPath);

            // Copy over all data files
            var filePaths = Directory.GetFiles(DataWebPath);
            foreach (var filename in filePaths)
            {
                
                var newFilePathName = filename.Replace(DataWebPath, dataUtPath);

                File.Copy(filename, newFilePathName);
            }
        }

        [OneTimeTearDown]
        public void RunAfterAnyTests()
        {
        }
    }
}
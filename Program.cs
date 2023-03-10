using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CountStringOccurrencesInFilename
{
    internal class Program
    {
        public static string filePath2 { get; private set; }

        static void Main()
        {
            // c:/anturafilefolder i only valid folder to choose file from
            string onlyValidDirectory = @"c:\anturafilefolder";

            // Welcome using our service!
            InfoUsingService(onlyValidDirectory);

            // Create folder: c:\anturafilefolder\ if its not exist
            CheckAnturaFolderIfNotExist();

            // If folder c:\anturafilefolder\ already exists - Move to that folder
            MoveToAnturaFileFolder();

            // Provide a file in the c:\anturafilefolder\
            Console.WriteLine("You must provide a file\n");
            
            while (true)
            {
                string filePath = Console.ReadLine();
                string[] input = new string[] { filePath };
                string filePath2 = input[0];
                
                string baseDirectory = Path.GetFullPath(filePath);
                Console.WriteLine($"\nBaseDirectory: {baseDirectory}\n");

                var isValidFolder = CheckFileToCountIsIntoAnturaFileFolder(onlyValidDirectory, baseDirectory);
                if (!isValidFolder)
                {
                    MoveToAnturaFileFolder();
                    Console.WriteLine("By security reasons we only accept files from the Antura-folder!");
                    Console.WriteLine("Closing down after 10 seconds...");
                    Thread.Sleep(10000);
                    break;
                }
                string getFullPath = Path.GetFullPath(filePath2);
                //Console.WriteLine($"getFullPath: {getFullPath}");
                string baseFilename = Path.GetFileNameWithoutExtension(filePath2);
                //Console.WriteLine($"baseFileName: {baseFilename}");

                // Check file exist in full path destination 
                var isValidFile = CheckFileExist(filePath2);

                if (!isValidFile)
                {
                    Console.WriteLine("We only accept valid files from the Antura-folder!");
                    Console.WriteLine("Closing down after 7 seconds...");
                    Thread.Sleep(7000);
                    break;
                }

                // Read all file contents
                string fileContents = ReadAllFileContants(filePath2);

                // Count number of times the basefilename (without extension) occurs in the filename
                // Only small characters counts
                int count = CountOccurrences(fileContents, baseFilename);

                // Result after counting in file
                WriteCountResult(baseFilename, count, filePath2);
                break;
            }
        }

        private static void InfoUsingService(string validFolder)
        {
            Console.WriteLine("_____________________________________________________");
            Console.WriteLine("Information:");
            Console.WriteLine("Welcome to using our file counting service");
            Console.WriteLine($"Only files in the folder: {validFolder} accepts");
            Console.WriteLine("_____________________________________________________");
        }

        private static bool CheckFileToCountIsIntoAnturaFileFolder(string onlyValidDirectory, string baseDirectory)
        {
            string directory = Path.GetDirectoryName(baseDirectory);
            Console.WriteLine($"directory:  {directory}");
            Console.WriteLine($"OnlyValid: {onlyValidDirectory}");
            if (onlyValidDirectory != directory)
            {
                Console.WriteLine("File must be located in the anturafilefolder directory.");
                return false;
            }
            else
            {
                return true;
            }
        }

        private static void CheckAnturaFolderIfNotExist()
        {
            string path = @"c:\anturafilefolder";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                Console.WriteLine("Folder c:/anturafilefolder created successfully.");
            }
            else
            {
                Console.WriteLine($"\nFolder {path} already exists.");
            }
        }
        private static void MoveToAnturaFileFolder()
        {
            string path = @"c:\anturafilefolder";
            if (Directory.Exists(path))
            {
                Directory.SetCurrentDirectory(path);
                Console.WriteLine($"Moved to folder {path} successfully.\n");
            }
            else
            {
                Console.WriteLine("\nFolder does not exist.");
            }

        }

        private static string ReadAllFileContants(string filePath2)
        {
            try
            {
                string fileContents = File.ReadAllText(filePath2);
                return fileContents;
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine("Access to the file was denied.");
                Console.ReadLine();
                return "";
            }
        }

        private static bool CheckFileExist(string filePath2)
        {
            if (!File.Exists(filePath2))
            {
                Console.WriteLine($"File not found! {filePath2}");
                return false;
            }
            else
            {
                return true;
            }
        }

        private static void WriteCountResult(string baseFilename, int count, string filePath2)
        {
            Console.WriteLine($"The string: {baseFilename} in the file: {filePath2} occurs: {count} times in the file.");
            Console.WriteLine("Closing down after 7 seconds...thank you for using our service");
            Thread.Sleep(7000);
    }

        public static int CountOccurrences(string fileContents, string stringToSearch)
        {
            var count = 0;
            var i = 0;

            while ((i = fileContents.IndexOf(stringToSearch, i, StringComparison.Ordinal)) != -1)
            {
                count++;
                i += stringToSearch.Length;
            }
            return count;
        }
    }
}

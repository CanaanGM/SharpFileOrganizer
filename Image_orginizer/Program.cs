// See using System.IO;

using System.Runtime.InteropServices;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // The source and destination directories
            string source_dir = $"C:\\users\\{Environment.UserName}\\desktop";
            string dest_dir = @"D:\references";

            // Get the path to the source directory
            string source = args.Length > 0 ? args[0] : ".";
            //OrganizeImages(source_dir, dest_dir);
            OpenImageAtRandom(dest_dir);
        }

        private static void OrganizeImages(string source_dir, string dest_dir)
        {
            // Create the destination directory if it doesn't exist
            if (!Directory.Exists(dest_dir))
                Directory.CreateDirectory(dest_dir);

            // Get a list of files in the source directory
            string[] files = Directory.GetFiles(source_dir);

            // Iterate over the files in the source directory
            foreach (string file in files)
            {
                /// can add all these in a ["jpg", "png", . . .] and check if the split(".") 
                /// is in here and depending on it go 

                if (file.EndsWith(".jpg") || file.EndsWith(".png") || file.EndsWith(".jpeg"))
                {
                    // Move the file to the destination directory
                    string file_name = Path.GetFileName(file);
                    string dest_path = Path.Combine(dest_dir, file_name);
                    File.Move(file, dest_path);
                }
                else if (file.EndsWith(".gif") || file.EndsWith(".avif") || file.EndsWith(".apneg"))
                {

                    string animated = Path.Combine(dest_dir, "animated");
                    if (!Directory.Exists(dest_dir))
                    {
                        Directory.CreateDirectory(dest_dir);
                    }
                    // Move the file to the destination directory
                    string file_name = Path.GetFileName(file);
                    string dest_path = Path.Combine(animated, file_name);
                    File.Move(file, dest_path);
                }
            }
        }

        public static bool OpenImageAtRandom(string pathToImage)
        {
            var imageFoldersPaths = Directory.GetDirectories(pathToImage);

            var supportedExtensions = new List<string> { "jpg", "png", "gif", "jpeg", "avif", "apneg" };
            var allFilesInDir = new List<string>(); // every single file in the given dir
            var allImages = new List<string>(); // holds all the IMAGE files from allFilesInDir

            foreach (string folder_path in imageFoldersPaths)
            {

                var fileAttr = File.GetAttributes(folder_path);
                if (fileAttr.HasFlag(FileAttributes.Directory))
                // its a sub directory loop thru it
                    foreach (var item in Directory.GetDirectories(folder_path))
                    {
                        allFilesInDir.AddRange(Directory.GetFiles(item));
                    }

                allFilesInDir.AddRange(
                Directory.GetFiles(folder_path)
                );
            }

            if (allFilesInDir.Count() > 0)
            {

                allImages.AddRange(allFilesInDir.Where(x=> supportedExtensions.Contains(x.Split('\\').Last().Split('.').Last())) );

                var randomImageIndex = new Random().Next(0, allImages.Count());
                var randomImagePath = allImages[randomImageIndex];

                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    System.Diagnostics.Process.Start("explorer.exe", randomImagePath);
                    return true;
                }

                if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    System.Diagnostics.Process.Start("xdg-open", randomImagePath);
                    return true;
                }

                if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    System.Diagnostics.Process.Start("open", randomImagePath);
                    return true;
                }

            }
            // there was no images to open . . .
            return false;
        }
    }
}
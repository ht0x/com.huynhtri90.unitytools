using System.IO;
using System.IO.Compression;

public partial class MyUtilities 
{  
    public static readonly string EXTENSION = ".zip";
   
    public static bool Compress(string source, string destination, bool replaceExisting = true)
    {
        if (string.IsNullOrEmpty(source))
        {
            CLog.LogErrorLoopEditor("Source path is null or empty.");
            return false;
        }

        if (IsPathDirectory(source) && !Directory.Exists(source))
        {
            CLog.LogErrorLoopEditor("Source path is an empty directory. There is nothing to perform compression!");
            return false;
        }

        if (IsPathFile(source) && !File.Exists(source))
        {
            CLog.LogErrorLoopEditor("Source path is a file path and it does not exist!");
            return false;
        }

        if (string.IsNullOrEmpty(destination))
        {
            CLog.LogErrorLoopEditor("Destination path is null or empty.");
            return false;
        }

        if (Path.GetExtension(destination) != EXTENSION)
        {
            CLog.LogErrorLoopEditor("Destination path is not valid. Please specify the name of a zip file instead.");
            return false;
        }

        if (replaceExisting && File.Exists(destination))        
            File.Delete(destination);        
        
        if (File.GetAttributes(source).HasFlag(FileAttributes.Directory))                 
            ZipFile.CreateFromDirectory(source, destination);        
        else
        {          
            var file = new FileInfo(source);
            using var originalFileStream = file.OpenRead();
            if (file.Attributes != FileAttributes.Hidden & file.Extension != EXTENSION)
            {
                using var compressedFileStream = File.Create(destination);
                using var compressionStream = new GZipStream(compressedFileStream, CompressionMode.Compress);
                originalFileStream.CopyTo(compressionStream);
            }
        }

        return true;
    }
   
    public static bool Compress(DirectoryInfo source, FileInfo destination, bool replaceExisting = true)
    {
        if (source == null)
        {
            CLog.LogErrorLoopEditor("Source directory is null.");
            return false;           
        }

        if (destination == null)
        {
            CLog.LogErrorLoopEditor("Destination file is null.");
            return false;
        }

        return Compress(source.FullName, destination.FullName, replaceExisting);
    }
   
    public static bool Compress(FileInfo source, FileInfo destination, bool replaceExisting)
    {
        if (source == null)
        {
            CLog.LogErrorLoopEditor("Source file is null.");
            return false;
        }

        if (destination == null)
        {
            CLog.LogErrorLoopEditor("Destination file is null.");
            return false;
        }

        return Compress(source.FullName, destination.FullName, replaceExisting);
    }
   
    public static bool Decompress(string source, string destination, bool overwrite = false)
    {
        if (string.IsNullOrEmpty(source))
        {
            CLog.LogErrorLoopEditor("Source file path is null or empty.");
            return false;          
        }

        if (Path.GetExtension(source) != EXTENSION)
        {
            CLog.LogErrorLoopEditor("Source path is not valid. Please specify the name of a zip file instead.");
            return false;          
        }
      
        if (File.GetAttributes(source).HasFlag(FileAttributes.Directory))
        {
            CLog.LogErrorLoopEditor("Source is a directory. Please specify the name of a zip file instead.");
            return false;          
        }

        if (!File.Exists(source))
        {
            CLog.LogErrorLoopEditor("Source file does not exist.");
            return false;
        }

        if (string.IsNullOrEmpty(destination))
        {
            CLog.LogErrorLoopEditor("Destination path is null or empty.");
            return false;
        }

        if (!IsPathDirectory(destination))
        {
            CLog.LogErrorLoopEditor("Destination is not a directory. Please specify the name of a directory instead.");
            return false;
        }

        if (!Directory.Exists(destination))        
            Directory.CreateDirectory(destination);                  

        if (overwrite)
        {
            using ZipArchive archive = ZipFile.OpenRead(source);
            foreach (ZipArchiveEntry entry in archive.Entries)
            {
                string targetFile = Path.Combine(destination, entry.FullName);
                Directory.CreateDirectory(Path.GetDirectoryName(targetFile) ?? string.Empty);
                entry.ExtractToFile(targetFile, true);
            }
        }
        else ZipFile.ExtractToDirectory(source, destination);

        return true;
    }
  
    public static bool Decompress(FileInfo source, DirectoryInfo destination, bool overwrite = false)
    {
        if (source == null)
        {
            CLog.LogErrorLoopEditor("Source file is null.");
            return false;
        }

        if (destination == null)
        {
            CLog.LogErrorLoopEditor("Destination directory is null.");
            return false;           
        }

        return Decompress(source.FullName, destination.FullName, overwrite);
    }  
}

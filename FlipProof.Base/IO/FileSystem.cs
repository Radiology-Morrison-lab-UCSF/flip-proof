namespace FlipProof.Base.IO;

/// <summary>
/// File/directory operations
/// </summary>
public static class FileSystem
{
   /// <param name="dir">The top directory to begin searching through. Child directories are also searched</param>
   /// <param name="includeDSStore">Whether to return .DS_Store files OSX leaves lying around</param>
   public static List<FilePath> GetAllFiles(FilePath dir, bool includeDSStore = false)
   {
      return GetAllFiles(dir, includeDSStore ? Array.Empty<string>() : new[] { ".DS_Store" });
   }


   /// <summary>
   /// Returns all files found in a directory and its children
   /// </summary>
   /// <param name="dir">The directory to start looking in</param>
   /// <param name="ignoreFilesEndingIn">Files that end with any string in this list are ignored. Case insensitive</param>
   /// <returns></returns>
   public static List<FilePath> GetAllFiles(FilePath dir, IReadOnlyCollection<string> ignoreFilesEndingIn)
   {
      List<FilePath> found = Directory.EnumerateFiles(dir.AbsolutePath)
         .Where(NotInExcludeList)
         .Select(a => new FilePath(a))
         .ToList();

      foreach (string subDir in Directory.EnumerateDirectories(dir.AbsolutePath))
      {
         found.AddRange(GetAllFiles(new FilePath(subDir), ignoreFilesEndingIn));
      }

      return found;


      bool NotInExcludeList(string loc) => !ignoreFilesEndingIn.Any(ext=>loc.EndsWith(ext, StringComparison.OrdinalIgnoreCase));
   }

   public static void CopyDirectoryContent(FilePath dirFrom, FilePath dirTo, bool overwrite=true)
   {
      Directory.GetFiles(dirFrom).Foreach(a => File.Move(a, dirTo + Path.GetFileName(a), overwrite));
   }

   /// <summary>
   /// Moves a file. If one path is gzipped and the other not, it will zip or unzip as required
   /// </summary>
   /// <param name="from"></param>
   /// <param name="to"></param>
   public static async Task MoveGzSafe(FilePath from, FilePath to)
   {
      if (from.FileExtension.Equals(to.FileExtension, StringComparison.CurrentCultureIgnoreCase))
      {
         File.Move(from, to);
      }
      else if (from.FileExtension.EndsWith(".gz", StringComparison.OrdinalIgnoreCase))
      {
         await GZip.UnzipFile(from, to, true);
      }
      else if (to.FileExtension.EndsWith(".gz", StringComparison.OrdinalIgnoreCase))
      {
         await GZip.ZipFile(from, to, true);
      }
      else
      {
         File.Move(from, to);
      }
   }
   /// <summary>
   /// Copies a file. If one path is gzipped and the other not, it will zip or unzip as required
   /// </summary>
   /// <param name="from"></param>
   /// <param name="to"></param>
   public static async Task CopyGzSafe(FilePath from, FilePath to)
   {
      if (from.FileExtension.Equals(to.FileExtension, StringComparison.CurrentCultureIgnoreCase))
      {
         File.Copy(from, to);
      }
      else if (from.FileExtension.EndsWith(".gz", StringComparison.OrdinalIgnoreCase))
      {
         await GZip.UnzipFile(from, to, false);
      }
      else if (to.FileExtension.EndsWith(".gz", StringComparison.OrdinalIgnoreCase))
      {
         await GZip.ZipFile(from, to, false);
      }
      else
      {
         File.Copy(from, to);
      }
   }

}
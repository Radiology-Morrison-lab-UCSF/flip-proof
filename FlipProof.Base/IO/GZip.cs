using System.IO.Compression;

namespace FlipProof.Base.IO;

/// <summary>
/// Provides gzip functionality
/// </summary>
public static class GZip
{
   /// <summary>
   /// Zips a file to disk
   /// </summary>
   /// <param name="input">To Zip</param>
   /// <param name="output">Where to save</param>
   /// <param name="removeOriginal">Delete the input</param>
   /// <returns>A task</returns>
   /// <exception cref="ArgumentException">Bad file suffix</exception>
   public static async Task ZipFile(FilePath input, FilePath output, bool removeOriginal)
   {
      if (!output.FileExtension.EndsWith(".gz", StringComparison.CurrentCultureIgnoreCase))
      {
         throw new ArgumentException("File path should end in .gz");
      }
      
      await CompressOrDecompress(input, output, removeOriginal, CompressionMode.Compress);
   }

   static async Task CompressOrDecompress(FilePath input, FilePath output, bool removeOriginal, CompressionMode mode)
   {
      try
      {
         await using FileStream inStream = new(input, FileMode.Open, FileAccess.Read, FileShare.Read);
         await using FileStream outStream = File.Create(output, 192000, FileOptions.Asynchronous);
         await using GZipStream gZipStream = new(mode == CompressionMode.Compress ? outStream : inStream, mode);
         if (mode == CompressionMode.Compress)
         {
            await inStream.CopyToAsync(gZipStream);
         }
         else
         {
            await gZipStream.CopyToAsync(outStream);   
         }
         
      }
      catch
      {
         // Don't leave a partially written file in weird crash cases
         output.Delete();
         throw;
      }


      if (removeOriginal)
      {
         input.Delete();
      }
   }

   /// <summary>
   /// Reads the file. If it is zipped it will be streamed unzipped
   /// </summary>
   /// <param name="path"></param>
   /// <param name="forciblyAssumeGzipped">If true it will unzip this stream. If false, it will return a normal file stream if the file does not end in .gz</param>
   /// <returns></returns>
   public static async Task<Stream> GetNonGzStream(FilePath path, bool forciblyAssumeGzipped)
   {
      if (forciblyAssumeGzipped || path.FileExtension.EndsWith(".gz", StringComparison.OrdinalIgnoreCase))
      {
         await using FileStream inStream = new(path, FileMode.Open, FileAccess.Read, FileShare.Read);
         await using GZipStream gZipStream = new(inStream, CompressionMode.Decompress);
         MemoryStream ms = new();
         await gZipStream.CopyToAsync(ms);
         return ms;
      }

      return new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
   }

   public static async Task UnzipFile(FilePath input, FilePath output, bool removeOriginal)
   {
      if (!input.FileExtension.EndsWith(".gz", StringComparison.CurrentCultureIgnoreCase))
      {
         throw new ArgumentException("File path should end in .gz");
      }
      
      await CompressOrDecompress(input, output, removeOriginal, CompressionMode.Decompress);
   }
}
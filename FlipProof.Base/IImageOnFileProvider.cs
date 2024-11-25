namespace FlipProof.Base;

/// <summary>
/// Returns a path to an image on file
/// </summary>
public interface IImageOnFileProvider
{
   /// <summary>
   /// Returns a path to an image on file. May create a file using the provided argument if the file is not found
   /// </summary>
   /// <param name="tfg">Used to create the file if it does not exist. If null and the file does not exist, and exception is thrown</param>
   /// <exception cref="FileNotFoundException">File not found and the <see cref="TemporaryFilenameGenerator"/> is null</exception>
   FilePath GetImagePath(TemporaryFilenameGenerator? tfg);

}
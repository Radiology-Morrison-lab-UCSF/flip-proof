namespace FlipProof.Base;

/// <summary>
/// The file we are creating already exists but we've not been told we can overwrite
/// </summary>
public class ResultFoundException : IOException
{
   public FilePath Path { get; }
   public ResultFoundException(FilePath path) : base($"The file {path} was found but overwriting results is not allowed")
   {
      Path = path;
   }
}
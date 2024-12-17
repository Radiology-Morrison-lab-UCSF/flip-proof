namespace FlipProof.Base.IO;


/// <summary>
/// Path to a file or directory
/// </summary>
public readonly struct FilePath : IImageOnFileProvider
{
   public string AbsolutePath { get; }

   /// <summary>
   /// Assumes this is a file and returns the file extension, by splitting from the first . in the filename.
   /// Result includes the fullstop
   /// </summary>
   public string FileExtension
   {
      get
      {
         string fn = Path.GetFileName(AbsolutePath);
         int index = IndexOfFileExtension(fn);
         return index < 0 ? fn : fn[index..];
      }
   }

   private static int IndexOfFileExtension(string fn)
   {
      return fn.IndexOf('.');
   }

   /// <summary>
   /// This without its <see cref="FileExtension"/>
   /// </summary>
   public string AbsolutePathWithoutFileExtension => AbsolutePath[..^FileExtension.Length];

   /// <summary>
   /// True if the file exists
   /// </summary>
   public bool Exists => Path.Exists(AbsolutePath);

   public FilePath(DirectoryInfo d) : this(d.FullName) { }

   /// <summary>
   /// 
   /// </summary>
   /// <param name="path">Relative or absolute</param>
   public FilePath(string path)
   {
      AbsolutePath = Path.GetFullPath(path);
   }


   /// <summary>
   /// Nothing done if the directory already exists
   /// </summary>
   public void CreateAsDirectory() => Directory.CreateDirectory(AbsolutePath);

   /// <summary>
   /// Nothing done if the directory already exists
   /// </summary>
   public void CreateParentAsDirectory() => Directory.CreateDirectory(GetParentDirectory());

   public FilePath GetParentDirectory() => new(Directory.GetParent(AbsolutePath)!);

   public bool IsWithin(FilePath parent) => ToString().Contains(parent.ToString());

   /// <summary>
   /// Inserts text before the first dot in the filename and returns the result
   /// </summary>
   /// <param name="toInsert"></param>
   /// <returns></returns>
   public FilePath InsertBeforeExtension(string toInsert) => AbsolutePathWithoutFileExtension.ToString() + toInsert + FileExtension;


   /// <summary>
   /// Throws a file not found exception if this is not found
   /// </summary>
   /// <exception cref="FileNotFoundException">Thrown if the path is not found</exception>
   public void ThrowIfNotFound()
   {
      if (!Exists)
      {
         throw new FileNotFoundException(AbsolutePath);
      }
   }

   /// <summary>
   /// Throws a <see cref="ResultFoundException"/> if this file or directory exists
   /// </summary>
   /// <exception cref="ResultFoundException">This file or directory exists</exception>
   public void ThrowIfFound()
   {
      if (Exists)
      {
         throw new ResultFoundException(this);
      }
   }

   /// <summary>
   /// Deletes the file or directory if it exists
   /// </summary>
   public void Delete()
   {
      if (Exists)
      {
         if (Directory.Exists(AbsolutePath))
            Directory.Delete(AbsolutePath, true);
         else
            File.Delete(AbsolutePath);
      }
   }
   FilePath IImageOnFileProvider.GetImagePath(TemporaryFilenameGenerator? tfg) => this;

   public override string ToString() => AbsolutePath;

   public static implicit operator string(FilePath p) => p.AbsolutePath;
   public static implicit operator FilePath(string p) => new(p);

   public static FilePath operator +(FilePath l, string r) => new(Path.Combine(l.AbsolutePath, r));

}
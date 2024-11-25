using System.Text;

namespace FlipProof.Base;

/// <summary>
/// Creates temporary filenames. By default, upon disposal those temporary files/directories are deleted
/// </summary>
public class TemporaryFilenameGenerator : IDisposable
{
  readonly FilePath dir;
  readonly List<FilePath> _allGeneratedFilenames = new();
  public IEnumerable<FilePath> AllGeneratedFilenames => _allGeneratedFilenames;

  /// <summary>
  /// Default suffix applied to the filenames
  /// </summary>
  readonly string suffix;

  /// <summary>
  /// When true (the default), dispose and finalize will delete all temporary files created
  /// </summary>
  public bool DeleteFilesWhenDestroyed { get; init; } = true;

  /// <summary>
  /// The last filename generated
  /// </summary>
  /// <exception cref="InvalidOperationException"></exception>
  public FilePath Last => _allGeneratedFilenames.Count == 0
    ? throw new InvalidOperationException("No last to return")
    : _allGeneratedFilenames[^1];


  public TemporaryFilenameGenerator():this(Path.GetTempPath(), null)
  {
    
  }
  /// <param name="dir">The directory to create filenames for</param>
  /// <exception cref="FileNotFoundException">If the directory does not exist</exception>
  public TemporaryFilenameGenerator(FilePath dir, string? suffix)
  {
    this.dir = dir;
    dir.ThrowIfNotFound();
    this.suffix = suffix == null ? string.Empty : DotSuffix(suffix);
  }

  public FilePath NextDir() => Next("") + Path.DirectorySeparatorChar;

  /// <summary>
  /// Gets the next temporary file name
  /// </summary>
  /// <param name="suffixOverride">Overrides the default suffix</param>
  /// <returns></returns>
  public FilePath Next(string? suffixOverride = null)
  {
    string suff = suffixOverride == null ? suffix : DotSuffix(suffixOverride);

    var last = dir + (Guid.NewGuid() + suff);
    _allGeneratedFilenames.Add(last);
    return last;
  }


  static string DotSuffix(string suffixOverride)
  {
    if (suffixOverride.Length != 0 && !suffixOverride.StartsWith('.'))
    {
      suffixOverride = "." + suffixOverride;
    }

    return suffixOverride;
  }

  /// <summary>
  /// Deletes all existing files whose name was generated by this. Non-existing filenames are ignored
  /// </summary>
  public void DeleteAllFiles() => _allGeneratedFilenames.Foreach(a => a.Delete());

  #region IDisposable

  void ReleaseUnmanagedResources()
  {
    if (DeleteFilesWhenDestroyed)
    {
      DeleteAllFiles();
    }
  }

  protected virtual void Dispose(bool disposing)
  {
    ReleaseUnmanagedResources();
    if (disposing)
    {
      // release managed resources here
      _allGeneratedFilenames.Clear(); // avoid trying to double-delete in case of dispose called twice
    }
  }

  /// <summary>
  /// Deletes all temporary files created
  /// </summary>
  public void Dispose()
  {
    Dispose(true);
    GC.SuppressFinalize(this);
  }

  ~TemporaryFilenameGenerator()
  {
    Dispose(false);
  }
  #endregion
}
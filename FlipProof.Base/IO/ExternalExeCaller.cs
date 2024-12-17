using System.Diagnostics;
using System.Text;

namespace FlipProof.Base.IO;

/// <summary>
/// Calls external programs
/// </summary>
public static class ExternalExeCaller
{
   public enum ShellExecuteStyle
   {
      NoShell,
      HiddenShell,
      DisplayedShell
   }
   public static async Task Call(FilePath exeFilename,
       IEnumerable<object?> arguments,
       bool printCall = false,
       string? workingDir = null,
       ShellExecuteStyle shellExecute = ShellExecuteStyle.NoShell,
       Dictionary<string, string?>? environmentalVariables = null)
   {
      string argString = ConcatAndDelimit(arguments);

      if (printCall)
      {
         Console.WriteLine(GetCall());
      }

      ProcessStartInfo psi = new(exeFilename, argString)
      {
         UseShellExecute = shellExecute != ShellExecuteStyle.NoShell,
         WindowStyle = shellExecute == ShellExecuteStyle.DisplayedShell ? ProcessWindowStyle.Normal : ProcessWindowStyle.Hidden,
      };

      environmentalVariables?.Foreach(kvp => psi.Environment.Add(kvp.Key, kvp.Value));

      if (workingDir is not null)
      {
         psi.WorkingDirectory = workingDir;
      }
      Process p = Process.Start(psi) ?? throw new Exception($"Could not execute {exeFilename} {argString}");

      await p.WaitForExitAsync();

      if (p.ExitCode != 0)
      {
         throw new Exception("Command failed: " + GetCall());
      }

      string GetCall() => exeFilename.ToString() + " " + argString;
   }

   static string ConcatAndDelimit(IEnumerable<object?> content, char delimiter = ' ')
   {
      StringBuilder sb = new();
      foreach (object? var in content)
      {
         if (var is not null)
         {
            sb.Append(var);
            sb.Append(delimiter);
         }
      }

      // remove extra delimiter
      if (sb.Length != 0)
      {
         sb.Remove(sb.Length - 1, 1);
      }

      return sb.ToString();
   }
}
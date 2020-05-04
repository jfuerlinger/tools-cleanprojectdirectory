using System;
using System.IO;
using System.Linq;
using static System.Console;

namespace CleanProjectDirectory
{
  public class Program
  {
    private static void Main(string[] args)
    {
      WriteLine("Projektverzeichnisse (C#, Java, Android, Angular) verkleinern");
      DeleteFolders("bin");
      DeleteFolders("obj");
      //DeleteFolders("dist");
      DeleteFolders("packages");
      DeleteFolders("TestResults");
      DeleteFolders("build");
      DeleteFolders(".gradle");
      DeleteFolders(".pioenvs");
      DeleteFolders(".vs");
      DeleteFolders("node_modules");
      DeleteFiles("*.dll");
      DeleteFiles("*.zip");
      DeleteFolders(".pioenvs");
      DeleteFolders(".piolibdeps");
      DeleteFiles("c_cpp_properties.json");
      DeleteFiles("launch.json");
      Write("Beenden mit Eingabetaste ...");
      ReadLine();
    }


    /// <summary>
    /// Löscht alle Verzeichnisse unterhalb des aktuellen Verzeichnisses
    /// 
    /// </summary>
    /// <param name="folderName">Verzeichnisname</param>
    private static void DeleteFolders(string folderName)
    {
      int deletedFolders = 0;
      int exceptionFolders = 0;
      long deletedBytes = 0;
      DirectoryInfo directoryInfo = new DirectoryInfo(folderName);

      var directories =
          Directory.GetDirectories(Environment.CurrentDirectory, folderName, SearchOption.AllDirectories);
      while (directories.Length > 0)
      {
        var dir = new DirectoryInfo(directories[0]);
        deletedBytes += dir.GetFiles("*.*", SearchOption.AllDirectories).Sum(file => file.Length);
        try
        {
          Directory.Delete(directories[0], true);
          deletedFolders++;
        }
        catch (Exception ex)
        {
          WriteLine($"    !!! Error deleting folder {directories[0]}: {ex.Message}");
          exceptionFolders++;
        }
        directories = Directory.GetDirectories(Environment.CurrentDirectory, folderName, SearchOption.AllDirectories);
      }
      if (exceptionFolders > 0)
      {
        WriteLine($"{deletedFolders,3} Verzeichnisse: {folderName} gelöscht, {exceptionFolders} Exceptions, {deletedBytes / 1024:N0} MB");
      }
      else
      {
        WriteLine($"{deletedFolders,3} Verzeichnisse: {folderName} gelöscht, {deletedBytes / 1024:N0} kB");
      }
    }

    /// <summary>
    /// Löscht alle Dateien mit dem entsprechenden Pattern
    /// 
    /// </summary>
    /// <param name="pattern">Verzeichnisname</param>
    private static void DeleteFiles(string pattern)
    {
      int deletedFiles = 0;
      int exceptionFiles = 0;
      long deletedBytes = 0;
      DirectoryInfo directoryInfo = new DirectoryInfo(Environment.CurrentDirectory);

      var fileInfos = directoryInfo.GetFiles(pattern, SearchOption.AllDirectories);
      foreach (var fileInfo in fileInfos)
      {
        try
        {
          fileInfo.Delete();
          deletedFiles++;
          deletedBytes += fileInfo.Length;
        }
        catch (Exception e)
        {
          WriteLine($"    !!! Error deleting file {fileInfo.FullName}: {e.Message}");
          exceptionFiles++;
        }
      }
      if (exceptionFiles > 0)
      {
        WriteLine($"{deletedFiles,3} Pattern: {pattern} gelöscht, {exceptionFiles} Exceptions, {deletedBytes / 1024:N0} MB");
      }
      else
      {
        WriteLine($"{deletedFiles,3} Pattern: {pattern} gelöscht, {deletedBytes / 1024:N0} kB");
      }
    }

  }
}

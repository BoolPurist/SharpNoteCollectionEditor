using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace NoteCollectionEditor;

/// <summary>
/// Implementation comes from stack overflow
/// <see href="https://stackoverflow.com/questions/816566/how-do-you-get-the-current-project-directory-from-c-sharp-code-when-creating-a-c">
/// stack overflow
/// </see>
/// </summary>
public class ProjectSourcePath
{
  private const string NameOfThisFile = $"{nameof(ProjectSourcePath)}.cs";

  public static string GetPathToProject()
  {
    string pathToCsFile = GetPathToSourceFile();
    Debug.Assert( pathToCsFile.EndsWith( NameOfThisFile, StringComparison.Ordinal ) );
    int lengthOfProjectPath = pathToCsFile.Length - NameOfThisFile.Length;
    return pathToCsFile.Substring( 0,  lengthOfProjectPath);
  }

  private static string GetPathToSourceFile([CallerFilePath] string? path = null)
  {
    Debug.Assert(path != null);
    return path;
  }
}

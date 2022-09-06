using System;
using System.IO;
using NoteCollectionEditor.ConfigMapping;
using NoteCollectionEditor.Extensions;

namespace NoteCollectionEditor.Services;

public class PathStoreForImportExportService : IPathStoreImportExport
{

  public string LastSavedName { get; private set; }

  public string ImportDirectoryPath { get; private set; }
  public string ExportDirectoryPath { get; private set; }

  public void SetExportFromFullPath(string fullPath)
  {
    LastSavedName = Path.GetFileName(fullPath);
    ExportDirectoryPath = Path.GetDirectoryName(fullPath) ?? String.Empty;
  }

  public void SetImportFromFullPath(string fullPath)
  {
    ImportDirectoryPath = Path.GetDirectoryName(fullPath) ?? String.Empty;
  }



  public PathStoreForImportExportService(IAppConfigs config)
  {
    var defaultImportExportPath = config.GetDefaultPathExportImport();
    LastSavedName = Path.GetFileName(config.PathToNoteSource);
    ExportDirectoryPath = defaultImportExportPath;
    ImportDirectoryPath = defaultImportExportPath;
  }


}

namespace NoteCollectionEditor.Services;

public interface IPathStoreImportExport
{

  public string LastSavedName { get; }
  string ExportDirectoryPath { get;  }
  string ImportDirectoryPath { get;  }

  public void SetExportFromFullPath(string fullPath);

  public void SetImportFromFullPath(string fullPath);
}

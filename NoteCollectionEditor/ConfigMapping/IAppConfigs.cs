namespace NoteCollectionEditor.ConfigMapping;

public interface IAppConfigs
{
  public AppDevelopmentConfig DevelopmentConfiguration { get; }
  public string PathToNoteSource { get; }

  public string AppVersion { get; }
  public string AppLink { get; }

  public string GetDefaultPathExportImport();

}

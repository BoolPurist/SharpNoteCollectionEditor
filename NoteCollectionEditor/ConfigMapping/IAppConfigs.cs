namespace NoteCollectionEditor.ConfigMapping;

public interface IAppConfigs
{
  public AppDevelopmentConfig DevelopmentConfiguration { get; }
  public string PathToNoteSource { get; }
  
}

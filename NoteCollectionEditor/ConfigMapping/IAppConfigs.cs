namespace NoteCollectionEditor.ConfigMapping;

public interface IAppConfigs
{
  public NoteDataSourceConfig DataSource { get; }
  public string PathToNoteSource { get; }
  
}

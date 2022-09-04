namespace NoteCollectionEditor.ConfigMapping;

public class NoteDataSourceConfig
{
  public bool LoadCrashes { get; set; }
  public int LoadDelay { get; set; }
  public int SaveDelay { get; set; }

  public bool WithFile { get; set; }
}

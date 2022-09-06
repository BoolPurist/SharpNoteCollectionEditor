namespace NoteCollectionEditor.ConfigMapping;

public class AppDevelopmentConfig
{
  public bool LoadCrashes { get; set; }
  public int LoadDelay { get; set; }
  public int SaveDelay { get; set; }

  public bool WithFile { get; set; }

  public string PathToDataDump { get; set; }
}

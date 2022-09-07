using JetBrains.Annotations;

namespace NoteCollectionEditor.ConfigMapping;

public class AppDevelopmentConfig
{
  public bool LoadCrashes { get; [UsedImplicitly] set; }
  public int LoadDelay { get; [UsedImplicitly] set; }
  public int SaveDelay { get; [UsedImplicitly] set; }

  public bool WithFile { get; [UsedImplicitly] set; }

  public string? PathToDataDump { get; [UsedImplicitly] set; }
}

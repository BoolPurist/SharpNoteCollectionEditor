namespace NoteCollectionEditor.Extensions;

public static class EnvironmentUtils
{
  public static bool IsInDevelopment()
  {
#if DEBUG
    return true;
#else
    return false;
#endif
  }
}

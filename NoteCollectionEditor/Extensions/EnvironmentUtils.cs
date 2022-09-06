using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

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

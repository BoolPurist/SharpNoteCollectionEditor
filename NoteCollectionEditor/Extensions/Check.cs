using System;

namespace NoteCollectionEditor.Extensions;

public static class Check
{
  public static void ThrowIfNoCastPossible<T>(object toCheck, string parameterName)
  {
    if (toCheck is not T)
    {
      string targetType = typeof(T).Name;
      string actualType = toCheck.GetType().Name;
      throw new ArgumentException($"{toCheck} with type {actualType} can not be casted into type {targetType}", parameterName);
    }
  }
}

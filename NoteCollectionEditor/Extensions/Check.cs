using System;

namespace NoteCollectionEditor.Extensions;

public static class Check
{
  public static T ThrowIfNoCastPossible<T>(object toCheck, string parameterName)
  {
    if (toCheck is T toReturn)
    {
      return toReturn;
    }
    else
    {
      string targetType = typeof(T).Name;
      string actualType = toCheck.GetType().Name;
      throw new ArgumentException($"{toCheck} with type {actualType} can not be casted into type {targetType}", parameterName);
    }



  }
}

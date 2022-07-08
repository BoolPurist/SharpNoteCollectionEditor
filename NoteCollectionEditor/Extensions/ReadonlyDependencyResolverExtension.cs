using System;
using Splat;

namespace NoteCollectionEditor.Extensions;

public static class ReadonlyDependencyResolverExtension
{
  public static T GetRequiredService<T>(this IReadonlyDependencyResolver resolver)
  {
    T fetchedService = resolver.GetService<T>();
    if (fetchedService == null)
    {
      throw new InvalidOperationException($"Service of type {typeof(T).Name} could not be retrieved.");
    }
    return fetchedService;
  }
}
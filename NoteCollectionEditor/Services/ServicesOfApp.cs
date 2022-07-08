using NoteCollectionEditor.Views;
using Splat;

namespace NoteCollectionEditor.Services;

public static class ServicesOfApp
{
  public static IReadonlyDependencyResolver Resolver { get; private set; }

  public static void Register(IMutableDependencyResolver services, IReadonlyDependencyResolver resolver)
  {
    services.Register(CreateMainWindow, typeof(MainWindow));
    Resolver = resolver;
  }

  private static MainWindow CreateMainWindow()
  {
    return new MainWindow();
  }
}
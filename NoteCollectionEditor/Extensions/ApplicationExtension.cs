using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;

namespace NoteCollectionEditor.Extensions;

public static class ApplicationExtension
{
  public static Window? GetMainWindow(this Application app)
  {
    if (app.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
    {
      return desktop.MainWindow;
    }

    return null;
  }

  public static Window? GetCurrentMainWindow()
  {
    if (Application.Current != null)
    {
      return Application.Current.GetMainWindow();
    }

    return null;
  }
}

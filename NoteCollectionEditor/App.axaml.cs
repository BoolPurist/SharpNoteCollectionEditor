using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using NoteCollectionEditor.Services;
using NoteCollectionEditor.Views;

namespace NoteCollectionEditor
{
  public class App : Application
  {
    public override void Initialize()
    {
      AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
      ServicesOfApp.RegisterAppServices();

      if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
      {
        desktop.MainWindow = new MainWindow();
      }

      base.OnFrameworkInitializationCompleted();
    }
  }
}

using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using NoteCollectionEditor.Extensions;
using NoteCollectionEditor.Services;
using NoteCollectionEditor.ViewModels;
using NoteCollectionEditor.Views;
using Splat;

namespace NoteCollectionEditor
{
  public partial class App : Application
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
        desktop.MainWindow = new MainWindow
        {
          DataContext = ServicesOfApp.Resolver.GetRequiredService<MainWindow>(),
        };
      }

      base.OnFrameworkInitializationCompleted();
    }

    
  }
}
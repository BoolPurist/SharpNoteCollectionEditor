using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using NoteCollectionEditor.ConfigMapping;
using NoteCollectionEditor.Extensions;
using NoteCollectionEditor.Services;

namespace NoteCollectionEditor.Views;

public partial class AboutView : Window
{
  public static readonly DirectProperty<AboutView, string> AppVersionProperty =
    AvaloniaProperty.RegisterDirect<AboutView, string>(
      nameof(AppVersion),
      about => about.AppVersion,
      (about, value) => about.AppVersion = value
    );

  private string _appVersion = String.Empty;

  public string AppVersion
  {
    get => _appVersion;
    set => SetAndRaise(AppVersionProperty, ref _appVersion, value);
  }

  public static readonly DirectProperty<AboutView, string> AppLinkProperty =
    AvaloniaProperty.RegisterDirect<AboutView, string>(
      nameof(AppLink),
      about => about.AppLink,
      (about, value) => about.AppLink = value
    );

  private string _appLink = String.Empty;

  public string AppLink
  {
    get => _appLink;
    set => SetAndRaise(AppLinkProperty, ref _appLink, value);
  }

  public AboutView()
  {
    InitializeComponent();

    var config =  ServicesOfApp.Resolver.GetRequiredService<IAppConfigs>();
    AppVersion = config.AppVersion;
    AppLink = config.AppLink;

#if DEBUG
    this.AttachDevTools();
#endif
  }

  private void InitializeComponent()
  {
    AvaloniaXamlLoader.Load(this);
  }
}


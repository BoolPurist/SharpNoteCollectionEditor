using System;
using System.Diagnostics;
using Avalonia.Controls.Shapes;
using Microsoft.Extensions.Configuration;
using Path = System.IO.Path;

namespace NoteCollectionEditor.ConfigMapping;



public class AppConfigs : IAppConfigs
{
  private const string SectionNameForPathToNoteListSource = "NoteListDataName";

  public NoteDataSourceConfig DataSource { get; set; } = new();
  public string PathToNoteSource { get; private set; } = String.Empty;

  private AppConfigs()
  {

  }

  public static AppConfigs CreateWithoutConfigFile()
  {
    return new AppConfigs();
  }

  public static AppConfigs Create()
  {
    var appConfig = new AppConfigs();

    var config = new ConfigurationBuilder()
      .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
      .AddJsonFile("appsettings.json")
#if DEBUG
      .AddJsonFile("appsettings.develop.json")
#endif
      .Build();

    appConfig.DataSource = GetSectionAsBindingByType<NoteDataSourceConfig>(config);
    appConfig.PathToNoteSource = GetSectionAsBinding<string>(config, SectionNameForPathToNoteListSource);
    appConfig.PathToNoteSource = Path.Join(Environment.CurrentDirectory, appConfig.PathToNoteSource);
    return appConfig;
  }

  private static T GetSectionAsBindingByType<T>(IConfigurationRoot root)
  {
    var boundSection = root.GetSection(typeof(T).Name).Get<T>();
    Debug.Assert(boundSection != null);
    return boundSection;
  }

  private static T GetSectionAsBinding<T>(IConfigurationRoot root, string nameSection)
  {
    var boundSection = root.GetSection(nameSection).Get<T>();
    Debug.Assert(boundSection != null);
    return boundSection;
  }



}

using System;
using System.Diagnostics;
using Microsoft.Extensions.Configuration;

namespace NoteCollectionEditor.ConfigMapping;



public class AppConfigs : IAppConfigs
{
  public NoteDataSourceConfig DataSource { get; set; } = new();

  private AppConfigs()
  {

  }

  public static AppConfigs CreateNotesFromFile()
  {
    return new AppConfigs();
  }

  public static AppConfigs Create()
  {
    var appConfig = new AppConfigs();

    var config = new ConfigurationBuilder()
      .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
#if DEBUG
      .AddJsonFile("appsettings.develop.json")
#endif
      .Build();

    appConfig.DataSource = GetSectionAsBinding<NoteDataSourceConfig>(config);
    return appConfig;
  }

  private static T GetSectionAsBinding<T>(IConfigurationRoot root)
  {
    var boundSection = root.GetSection(typeof(T).Name).Get<T>();
    Debug.Assert(boundSection != null);
    return boundSection;
  }



}

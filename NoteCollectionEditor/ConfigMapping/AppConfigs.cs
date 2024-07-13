using System;
using System.Diagnostics;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using NoteCollectionEditor.Extensions;
using Path = System.IO.Path;

namespace NoteCollectionEditor.ConfigMapping;

public class AppConfigs : IAppConfigs
{
  private const string SectionNameDataFileName = "NoteListDataName";
  private const string SectionNameRelativeDataPath = "PathToDataDump";

  public AppDevelopmentConfig DevelopmentConfiguration { get; private set; } = new();
  public string PathToNoteSource { get; private set; } = String.Empty;
  public string NoteDataFileName { get; private set; } = String.Empty;

  public string AppVersion { get; private set; } = String.Empty;

  public string AppLink { get; private set; } = String.Empty;



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

    if (EnvironmentUtils.IsInDevelopment())
    {
      appConfig.DevelopmentConfiguration = GetSectionAsBindingByType<AppDevelopmentConfig>(config);
    }

    appConfig.NoteDataFileName = GetSectionAsBinding<string>(config, SectionNameDataFileName);


    ApplyDevelopmentSettings(appConfig, config);

    appConfig.AppVersion = GetSectionAsBinding<string>(config, nameof(AppVersion));
    appConfig.AppLink = GetSectionAsBinding<string>(config, nameof(AppLink));

    return appConfig;
  }

  private static void ApplyDevelopmentSettings(AppConfigs appConfig, IConfigurationRoot config)
  {
#if DEBUG
    var appDevConfig = GetSectionAsBindingByType<AppDevelopmentConfig>(config);
    appConfig.PathToNoteSource = Path.Join(GetProjectPath(), appDevConfig.PathToDataDump, appConfig.NoteDataFileName);
#else
    appConfig.PathToNoteSource = Path.Join(GetCurrentExe(), appConfig.NoteDataFileName);
#endif
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

  public string GetDefaultPathExportImport()
  {
    if (EnvironmentUtils.IsInDevelopment())
    {
      var projectPath = ProjectSourcePath.GetPathToProject();
      var relativeExportPath = DevelopmentConfiguration.PathToDataDump;
      var exportPath = Path.Join(projectPath, relativeExportPath);
      return exportPath;
    }

    return Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
  }

  private static string GetCurrentExe() => AppContext.BaseDirectory;

  private static string GetProjectPath()
  {

    // Taken from stackoverflow.com:
    // https://stackoverflow.com/questions/816566/how-do-you-get-the-current-project-directory-from-c-sharp-code-when-creating-a-c
    string workingDirectory = AppDomain.CurrentDomain.BaseDirectory;
    // Getting from /bin/debug/netx.x to project root.
    // It assumed an application is always placed under 3 folders deep.
    // With assumption no null referencing will happen.
    string projectDirectory = System.IO.Directory.GetParent(workingDirectory)!.Parent!.Parent!.Parent!.FullName;

    return projectDirectory;
  }


}

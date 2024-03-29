using System;
using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using NoteCollectionEditor.Extensions;
using Path = System.IO.Path;

namespace NoteCollectionEditor.ConfigMapping;

public class AppConfigs : IAppConfigs
{
    private const string SectionNameForPathToNoteListSource = "NoteListDataName";

    public AppDevelopmentConfig DevelopmentConfiguration { get; private set; } = new();
    public string PathToNoteSource { get; private set; } = String.Empty;

    public string AppVersion { get; set; } = String.Empty;

    public string AppLink { get; set; } = String.Empty;



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

        var nameJsonDataFile = GetSectionAsBinding<string>(config, SectionNameForPathToNoteListSource);
        appConfig.PathToNoteSource = Environment.CurrentDirectory;

        ApplyDevelopmentSettings(appConfig, config);

        appConfig.PathToNoteSource = Path.Join(appConfig.PathToNoteSource, nameJsonDataFile);

        Console.WriteLine($"appConfig.PathToNoteSource: {appConfig.PathToNoteSource}");
        appConfig.AppVersion = GetSectionAsBinding<string>(config, nameof(AppVersion));
        appConfig.AppLink = GetSectionAsBinding<string>(config, nameof(AppLink));

        return appConfig;
    }

    private static void ApplyDevelopmentSettings(AppConfigs appConfig, IConfigurationRoot config)
    {
#if DEBUG
        var appDevConfig = GetSectionAsBindingByType<AppDevelopmentConfig>(config);
        appConfig.PathToNoteSource = Path.Join(appConfig.PathToNoteSource, appDevConfig.PathToDataDump);
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



}

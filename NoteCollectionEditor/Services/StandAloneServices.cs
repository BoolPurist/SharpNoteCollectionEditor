using System.Collections.Generic;
using NoteCollectionEditor.ConfigMapping;
using NoteCollectionEditor.Extensions;
using NoteCollectionEditor.Models;
using NoteCollectionEditor.ViewModels;
using Splat;

namespace NoteCollectionEditor.Services;

public static partial class ServicesOfApp
{
  private static void RegisterLogger(IMutableDependencyResolver container)
  {
    container.RegisterConstant<ILogger>(new ConsoleLogger() {Level = LogLevel.Debug});
  }

  private static void RegisterAppConfig(IMutableDependencyResolver container)
  {
    container.RegisterConstant<IAppConfigs>(AppConfigs.Create());
  }

  private static void RegisterAddNoteViewModel(IMutableDependencyResolver container)
  {
    container.Register(() => new AlterNoteViewModel());
  }

  private static void RegisterINoteListRepository(IMutableDependencyResolver container)
  {
    if (!EnvironmentUtils.IsInDevelopment())
    {
      var jsonDataSource = CreateJsonFileDataSource();
      container.RegisterConstant<INoteListRepository>(jsonDataSource);
      jsonDataSource.EnsureNeededFiles();
    }
    else
    {
      var configs = Resolver.GetRequiredService<IAppConfigs>();
      bool workWithFile = configs.DevelopmentConfiguration.WithFile;

      if (workWithFile)
      {
        var jsonDataSource = CreateDevJsonFileDataSource();
        container.RegisterConstant<INoteListRepository>(jsonDataSource);
        jsonDataSource.SaveAllSync(FakeData());
      }
      else
      {
        container.Register<INoteListRepository>(CreateFakeSource);
      }
    }


    NoteListFakeInMemorySource CreateFakeSource() => new(
      FakeData(),
      Resolver.GetRequiredService<IAppConfigs>()
    );

    NoteListJsonFileSource CreateJsonFileDataSource() =>
      new (
        Resolver.GetRequiredService<IAppConfigs>(),
        Resolver.GetRequiredService<ILogger>()
        );

    DevelopmentNoteListJsonFileSource CreateDevJsonFileDataSource() =>
      new (
        Resolver.GetRequiredService<IAppConfigs>(),
        Resolver.GetRequiredService<ILogger>()
      );

#pragma warning disable CS8321
    IEnumerable<NoteModel> FakeData()
#pragma warning restore CS8321
    {
      return new[]
      {
        new NoteModel {Title = "First", Content = "First Content"},
        new NoteModel {Title = "Second", Content = "Second Content"},
        new NoteModel {Title = "Third", Content = new string('y', 10)},
        new NoteModel {Title = "Fourth", Content = new string('z', 10)}
      };
    }
  }
}

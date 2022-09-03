using System.Collections.Generic;
using System.Linq;
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
    if (_unitTests)
    {
      container.Register<ILogger>(() => new InMemoryLogger());
    }
    else
    {
      container.RegisterConstant<ILogger>( new ConsoleLogger() { Level = LogLevel.Debug });
    }
  }

  private static void RegisterAppConfig(IMutableDependencyResolver container)
  {
    container.RegisterConstant<IAppConfigs>(_unitTests ? AppConfigs.CreateNotFromFile() : AppConfigs.Create());
  }
  
  private static void RegisterAddNoteViewModel(IMutableDependencyResolver container)
  {
    container.Register(() => new AddNoteViewModel());
  }

  private static void RegisterINoteListRepository(IMutableDependencyResolver container)
  {
    container.Register<INoteListRepository>(CreateFakeSource);
    NoteListFakeInMemorySource CreateFakeSource() => new (
      Enumerable.Empty<NoteModel>(), 
      Resolver.GetRequiredService<IAppConfigs>()
    );

#pragma warning disable CS8321
    IEnumerable<NoteModel> FakeData()
#pragma warning restore CS8321
    {
      return new[]
      {
        new NoteModel {Title = "First", Content = "First Content"},
        new NoteModel {Title = "Second", Content = "Second Content"},
        new NoteModel {Title = "Second", Content = new string('x', 200)},
        new NoteModel {Title = "Second", Content = new string('x', 400)}
      };
    }
  }

}
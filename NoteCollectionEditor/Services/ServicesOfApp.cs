using NoteCollectionEditor.Extensions;
using NoteCollectionEditor.Models;
using NoteCollectionEditor.ViewModels;
using NoteCollectionEditor.Views;
using Splat;

namespace NoteCollectionEditor.Services;

public static class ServicesOfApp
{
  public static IReadonlyDependencyResolver Resolver { get; private set; } = null!;

  public static void Register(IMutableDependencyResolver services, IReadonlyDependencyResolver resolver)
  {
    Resolver = resolver;
    RegisterStandAloneServices(services);
    
    services.Register(CreateNoteListViewModel, typeof(NoteListViewModel));
  }

  private static void RegisterStandAloneServices(IMutableDependencyResolver services)
  {
    services.Register(CreateMainWindow, typeof(MainWindow));
    services.Register(CreateINoteListRepository, typeof(INoteListRepository));
  }

  private static MainWindow CreateMainWindow()
  {
    return new MainWindow();
  }

  private static NoteListViewModel CreateNoteListViewModel()
  {
    return new NoteListViewModel(Resolver.GetRequiredService<INoteListRepository>());
  }

  private static INoteListRepository CreateINoteListRepository()
  {
    return new NoteListInMemorySource(new []
    {
      new NoteModel() {Title = "First", Content = "First Content"},
      new NoteModel() {Title = "Second", Content = "Second Content"}
    });
  }
}
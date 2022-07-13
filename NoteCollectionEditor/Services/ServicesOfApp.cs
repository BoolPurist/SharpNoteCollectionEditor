using System;
using NoteCollectionEditor.ConfigMapping;
using NoteCollectionEditor.Extensions;
using NoteCollectionEditor.Models;
using NoteCollectionEditor.ViewModels;
using NoteCollectionEditor.Views;
using Splat;

namespace NoteCollectionEditor.Services;

public static class ServicesOfApp
{
  private const string DefaultErrorHandler = "DefaultErrorHandler";
  private static bool _unitTests;

  private static readonly Object Locker = new ();
  
  public static IReadonlyDependencyResolver Resolver { get; private set; } = null!;
  
  public static void RegisterAppServices()
  {
    _unitTests = false;
    Register();
  }
  
  public static void RegisterForUnitTest()
  {
    _unitTests = true;
    RegisterAppServices();
  }

  private static void Register()
  {
    lock (Locker)
    {
      IMutableDependencyResolver services = Locator.CurrentMutable;
      IReadonlyDependencyResolver resolver = Locator.Current;
      
      Resolver = resolver;
      RegisterStandAloneServices(services);
    
      services.Register(CreateNoteListViewModel, typeof(NoteListViewModel));
    }
  }
  
  



  public static IErrorHandler GetDefaultErrorHandler()
  {
    return Resolver.GetService<IErrorHandler>(DefaultErrorHandler);
  }

  private static void RegisterStandAloneServices(IMutableDependencyResolver services)
  {
    services.RegisterConstant<ILogger>(new ConsoleLogger() { Level = LogLevel.Info });
    services.RegisterConstant<IAppConfigs>(_unitTests ? AppConfigs.CreateNotFromFile() : AppConfigs.Create());
    services.Register(CreateMainWindow, typeof(MainWindow));
    services.Register(CreateAddNoteViewModel, typeof(AddNoteViewModel));
    services.Register(CreateINoteListRepository, typeof(INoteListRepository));
    
    services.Register(CreateDefaultErrorHandler, DefaultErrorHandler);
  }

  public static IErrorHandler CreateDefaultErrorHandler()
  {
    return new LogException(Resolver.GetRequiredService<ILogger>());
  }

  private static MainWindow CreateMainWindow()
  {
    return new MainWindow();
  }

  private static NoteListViewModel CreateNoteListViewModel()
  {
    return new NoteListViewModel(
      Resolver.GetRequiredService<INoteListRepository>(), 
      Resolver.GetRequiredService<ILogger>()
      );
  }

  private static AddNoteViewModel CreateAddNoteViewModel()
  {
    return new AddNoteViewModel();
  }

  private static INoteListRepository CreateINoteListRepository()
  {
    return new NoteListFakeInMemorySource(new []
    {
      new NoteModel() {Title = "First", Content = "First Content"},
      new NoteModel() {Title = "Second", Content = "Second Content"},
      new NoteModel() {Title = "Second", Content = new string('x', 200)},
      new NoteModel() {Title = "Second", Content = new string('x', 400)}
    }, 
    Resolver.GetRequiredService<IAppConfigs>()
    );
  }
}
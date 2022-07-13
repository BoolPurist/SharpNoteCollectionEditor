using System;
using NoteCollectionEditor.ConfigMapping;
using NoteCollectionEditor.Extensions;
using NoteCollectionEditor.Models;
using NoteCollectionEditor.ViewModels;
using NoteCollectionEditor.Views;
using Splat;

namespace NoteCollectionEditor.Services;

public static partial  class ServicesOfApp
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
      RegisterNoteListViewModel(services);
    }
  }
  public static IErrorHandler GetDefaultErrorHandler()
  {
    return Resolver.GetService<IErrorHandler>(DefaultErrorHandler);
  }
  
  
  private static void RegisterStandAloneServices(IMutableDependencyResolver services)
  {
    RegisterLogger(services);
    RegisterAppConfig(services);
    RegisterINoteListRepository(services);
    RegisterAddNoteViewModel(services);
  }

  private static void RegisterNoteListViewModel(IMutableDependencyResolver container)
  {
    container.Register(() => new NoteListViewModel(
      Resolver.GetRequiredService<INoteListRepository>(), 
      Resolver.GetRequiredService<ILogger>()
    ));
  }

}
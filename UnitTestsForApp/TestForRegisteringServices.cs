

using System.Diagnostics;
using NoteCollectionEditor.Extensions;
using NoteCollectionEditor.Services;
using NoteCollectionEditor.ViewModels;
using NoteCollectionEditor.Views;
using Splat;

namespace UnitTestsForApp;

public class TestForRegisteringServices
{
  [Fact]
  public void ShouldRetrieveNoteListViewModel()
  {
    AssertForOneService<NoteListViewModel>();
  }
  
  [Fact]
  public void ShouldRetrieveINoteListRepository()
  {
    AssertForOneService<INoteListRepository>();
  }
  
  [Fact]
  public void ShouldRetrieveAddNoteViewModel()
  {
    AssertForOneService<AddNoteViewModel>();
  }

  [Fact]
  public void ShouldRetrieveILogger()
  {
    AssertForOneService<ILogger>();
  }
  
  [Fact]
  public void ShouldRetrieveDefaultErrorHandler()
  {
    ServicesOfApp.RegisterAppServices();
    Debug.Assert(ServicesOfApp.Resolver != null, "ServicesOfApp.Resolver != null");
    var service = ServicesOfApp.GetDefaultErrorHandler();
    Assert.NotNull(service);
  }


  private static void AssertForOneService<T>()
  {
    ServicesOfApp.RegisterForUnitTest();
    Debug.Assert(ServicesOfApp.Resolver != null, "ServicesOfApp.Resolver != null");
    var service = ServicesOfApp.Resolver.GetRequiredService<T>();
    Assert.NotNull(service);
  }
}
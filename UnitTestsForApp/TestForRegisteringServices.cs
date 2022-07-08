

using System.Diagnostics;
using NoteCollectionEditor.Extensions;
using NoteCollectionEditor.Services;
using NoteCollectionEditor.ViewModels;
using NoteCollectionEditor.Views;

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

  private static void AssertForOneService<T>()
  {
    App.RegisterAppServices();
    Debug.Assert(ServicesOfApp.Resolver != null, "ServicesOfApp.Resolver != null");
    var service = ServicesOfApp.Resolver.GetRequiredService<T>();
    Assert.NotNull(service);
  }
}
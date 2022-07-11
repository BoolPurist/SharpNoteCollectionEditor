using System.Reactive.Linq;
using NoteCollectionEditor.Models;
using NoteCollectionEditor.Services;
using NoteCollectionEditor.ViewModels;
using Splat;

namespace UnitTestsForApp;

public class TestNoteListViewModel
{
  private NoteListFakeInMemorySource CreateFakeProvider() => 
    new NoteListFakeInMemorySource(new[]
  {
    new NoteModel { Title = "First", Content = "1. Content"},
    new NoteModel { Title = "Second", Content = "2. Content"},
    new NoteModel { Title = "Third", Content = "3. Content"}
  });
  
  [Fact]
  public async Task ShouldContainCollectionAfterLoading()
  {
    var data = CreateFakeProvider();
    var expected = await data.LoadAll();
    var viewModel = new NoteListViewModel(data, new InMemoryLogger(LogLevel.Debug));

    await viewModel.LoadNotesIn.Execute().GetAwaiter();
    
    Assert.Equal(viewModel.Notes, expected);
  }
  

  [Fact]
  public async Task ShouldIndicateLoadingError()
  {
    ServicesOfApp.RegisterAppServices();
    var data = CreateFakeProvider();
    data.ThrowErrorInLoading = true;
    var fakeLogger = new InMemoryLogger(LogLevel.Debug);
    var viewModel = new NoteListViewModel(data, fakeLogger);
    bool errorStateBeforeLoading = viewModel.ErrorInLoading;
    await viewModel.LoadNotesIn.Execute().GetAwaiter();
    Assert.False(errorStateBeforeLoading);
    Assert.True(viewModel.ErrorInLoading);
  }
  
  
  
}
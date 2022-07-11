using System.Reactive.Linq;
using Avalonia.Shared.PlatformSupport;
using NoteCollectionEditor.Models;
using NoteCollectionEditor.Services;
using NoteCollectionEditor.ViewModels;
using Splat;

namespace UnitTestsForApp;

public class TestNoteListViewModel
{
  private NoteListFakeInMemorySource CreateFakeProvider() => 
    new (new[]
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
    var data = CreateFakeProvider();
    var fakeLogger = new InMemoryLogger(LogLevel.Debug);
    var viewModel = new NoteListViewModel(data, fakeLogger);
    data.ThrowErrorInLoading = true;
    bool errorStateBeforeLoading = viewModel.ErrorInLoading;
    await viewModel.LoadNotesIn.Execute().GetAwaiter();
    Assert.False(errorStateBeforeLoading);
    Assert.True(viewModel.ErrorInLoading);
  }

  [Fact]
  public void ShouldAddNote()
  {
    var data = CreateFakeProvider();
    var fakeLogger = new InMemoryLogger(LogLevel.Debug);
    var viewModel = new NoteListViewModel(data, fakeLogger);
    var expectedAdded = new NoteModel { Title = "Added", Content = "Content"};
    viewModel.AddNoteCommand.Execute(expectedAdded);
    Assert.False(viewModel.ErrorInLoading, "No loading error should happened");
    Assert.Single(viewModel.Notes);
    Assert.Equal(expectedAdded, viewModel.Notes.First());
  }
  
  [Fact]
  public async Task ShouldAddNoteToLoadedOnes()
  {
    // Data
    var toAdd = new NoteModel { Title = "Added", Content = "Content"};
    var loadedDate = new List<NoteModel>
    {
      new NoteModel {Title = "First", Content = "1. Content"},
      new NoteModel {Title = "Second", Content = "2. Content"},
      new NoteModel {Title = "Third", Content = "3. Content"}
    };
    var expectedEndResult = loadedDate.Concat(new [] { toAdd });
    
    // Set up
    var data = new NoteListFakeInMemorySource(loadedDate);
    var fakeLogger = new InMemoryLogger(LogLevel.Debug);
    var viewModel = new NoteListViewModel(data, fakeLogger);
    
    // Act
    await viewModel.LoadNotesIn.Execute().GetAwaiter();
    viewModel.AddNoteCommand.Execute(toAdd);
    
    // Assert
    Assert.False(viewModel.ErrorInLoading);
    Assert.Equal(expectedEndResult, viewModel.Notes);
  }

}
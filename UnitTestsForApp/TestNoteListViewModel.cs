using System.Reactive.Linq;
using Avalonia.Shared.PlatformSupport;
using NoteCollectionEditor.Models;
using NoteCollectionEditor.Services;
using NoteCollectionEditor.ViewModels;
using Splat;

namespace UnitTestsForApp;

public class TestNoteListViewModel
{
  private record EnvironmentForNoteListViewModel(
    InMemoryLogger Logger,
    NoteListFakeInMemorySource FakeSource,
    NoteListViewModel ViewModel
    );

  private static List<NoteModel> ExampleNotesForLoading => new List<NoteModel>
  {
    new NoteModel {Title = "First", Content = "1. Content"},
    new NoteModel {Title = "Second", Content = "2. Content"},
    new NoteModel {Title = "Third", Content = "3. Content"}
  };

  private static EnvironmentForNoteListViewModel CreateEnvironment() 
    => CreateEnvironment(ExampleNotesForLoading);

  private static EnvironmentForNoteListViewModel CreateEnvironment(IEnumerable<NoteModel> toLoad)
  {
    var data = new NoteListFakeInMemorySource(toLoad);
    var fakeLogger = new InMemoryLogger();
    var viewModel = new NoteListViewModel(data, fakeLogger);
    return new EnvironmentForNoteListViewModel(
      fakeLogger,
      data,
      viewModel
    );
  }

  [Fact]
  public async Task ShouldContainCollectionAfterLoading()
  {
    var env = CreateEnvironment();
    var viewModel = env.ViewModel;
    var expected = await env.FakeSource.LoadAll();
    await viewModel.LoadNotesIn.Execute().GetAwaiter();
    Assert.Equal(viewModel.Notes, expected);
  }

  [Fact]
  public async Task ShouldIndicateLoadingError()
  {
    var env = CreateEnvironment();
    var data = env.FakeSource;
    var viewModel = env.ViewModel;
    data.ThrowErrorInLoading = true;
    bool errorStateBeforeLoading = viewModel.ErrorInLoading;
    await viewModel.LoadNotesIn.Execute().GetAwaiter();
    Assert.False(errorStateBeforeLoading);
    Assert.True(viewModel.ErrorInLoading);
  }

  [Fact]
  public void ShouldAddNote()
  {
    var viewModel = CreateEnvironment().ViewModel;
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
    var loadedDate = ExampleNotesForLoading;
    var expectedEndResult = loadedDate.Concat(new [] { toAdd });
    
    // Set up
    var env = CreateEnvironment(loadedDate);
    var viewModel = env.ViewModel;
    
    // Act
    await viewModel.LoadNotesIn.Execute().GetAwaiter();
    viewModel.AddNoteCommand.Execute(toAdd);
    
    // Assert
    Assert.False(viewModel.ErrorInLoading);
    Assert.Equal(expectedEndResult, viewModel.Notes);
  }

}
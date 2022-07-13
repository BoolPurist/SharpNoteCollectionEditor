using System.Reactive.Linq;
using Avalonia.Shared.PlatformSupport;
using NoteCollectionEditor.ConfigMapping;
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
    var data = new NoteListFakeInMemorySource(toLoad, AppConfigs.CreateNotFromFile());
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
    Assert.True(viewModel.NoNotesFoundInNormalCase);
    await viewModel.LoadNotesIn.Execute().GetAwaiter();
    Assert.Equal(viewModel.Notes, expected);
    Assert.False(viewModel.NoNotesFoundInNormalCase);
  }

  [Fact]
  public async Task ShouldIndicateLoadingError()
  {
    var env = CreateEnvironment();
    var data = env.FakeSource;
    var viewModel = env.ViewModel;
    data.ThrowErrorInLoading = true;
    
    await viewModel.LoadNotesIn.Execute().GetAwaiter();
    Assert.False(viewModel.IsLoading, "Should not indicate loading if error happened.");
    Assert.True(viewModel.ErrorInLoading, "Should indicate error.");
    Assert.False(viewModel.NoNotesFoundInNormalCase);
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
    Assert.False(viewModel.NoNotesFoundInNormalCase);
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
    Assert.False(viewModel.ErrorInLoading, "No error in loading should happened.");
    Assert.False(viewModel.NoNotesFoundInNormalCase);
    Assert.Equal(expectedEndResult, viewModel.Notes);
  }

  [Fact]
  public async Task ShouldIndicateLoading()
  {
    // Set up
    var testEnvironment = CreateEnvironment();
    const int waitingTime = 1000;
    testEnvironment.FakeSource.LoadDelay = waitingTime;
    var notes = testEnvironment.ViewModel; 
    
    // Assert before act.
    Assert.False(notes.IsLoading, "Should indicate loading if loading has not started yet !");
    Assert.True(notes.NoNotesFoundInNormalCase);
    // Act
    var loadingState = notes.LoadNotesIn.Execute().GetAwaiter();
    await Task.Delay(waitingTime / 2);
    // Assert during act
    Assert.True(notes.IsLoading, "Should indicate loading while still loading.");
    Assert.False(notes.NoNotesFoundInNormalCase, "Notes are being loaded");
    await loadingState;
    
    // Assert after act
    Assert.False(notes.IsLoading, "Should not indicate loading after loading has finished.");
    Assert.False(notes.NoNotesFoundInNormalCase, "Notes were loaded");
  }

}
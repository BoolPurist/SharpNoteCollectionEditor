using System.Reactive.Linq;
using NoteCollectionEditor.ConfigMapping;
using NoteCollectionEditor.Models;
using NoteCollectionEditor.Services;
using NoteCollectionEditor.ViewModels;

namespace UnitTestsForApp;

public class TestNoteListViewModel
{
  private record EnvironmentForNoteListViewModel(
    // ReSharper disable once NotAccessedPositionalProperty.Local
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

  private static EnvironmentForNoteListViewModel CreateEnvironmentForTests()
    => CreateEnvironmentForTests(ExampleNotesForLoading);

  private static EnvironmentForNoteListViewModel CreateEnvironmentForTests(IEnumerable<NoteModel> toLoad)
  {
    var data = new NoteListFakeInMemorySource(toLoad, AppConfigs.CreateWithoutConfigFile());
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
    var env = CreateEnvironmentForTests();
    var viewModel = env.ViewModel;
    var expected = await env.FakeSource.LoadAll();

    await viewModel.LoadNotesIn.Execute().GetAwaiter();
    Assert.Equal(expected, viewModel.Notes);
    Assert.False(viewModel.NoNotesFoundInNormalCase);
    AssertIfIdAscending(viewModel.Notes);

  }

  private void AssertIfIdAscending(IEnumerable<NoteModel> toAssert)
  {
    int currentExpectedId = 0;
    foreach (var singleNote in toAssert)
    {
      Assert.Equal(currentExpectedId, singleNote.Id);
      currentExpectedId++;
    }
  }

  [Fact]
  public void ShouldIndicateNotLoadedYet()
  {
    var env = CreateEnvironmentForTests();
    var viewModel = env.ViewModel;

    Assert.True(viewModel.NoNotesFoundInNormalCase);
  }

  [Fact]
  public async Task ShouldIndicateLoadingError()
  {
    var env = CreateEnvironmentForTests();
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
    var viewModel = CreateEnvironmentForTests().ViewModel;
    var expectedAdded = new NoteModel { Title = "Added", Content = "Content"};
    var actualNotes = viewModel.Notes;
    viewModel.AddNoteCommand.Execute(expectedAdded);
    Assert.False(viewModel.ErrorInLoading, "No loading error should happened");
    Assert.Single(actualNotes);
    Assert.Equal(expectedAdded, actualNotes.First());
    Assert.False(viewModel.NoNotesFoundInNormalCase);
    AssertIfIdAscending(actualNotes);
  }

  [Fact]
  public async Task ShouldAddNoteToLoadedOnes()
  {
    // Data
    var toAdd = new NoteModel { Title = "Added", Content = "Content"};
    var loadedDate = ExampleNotesForLoading;
    var expectedEndResult = loadedDate.Concat(new [] { toAdd });

    // Set up
    var env = CreateEnvironmentForTests(loadedDate);
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
    var testEnvironment = CreateEnvironmentForTests();
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

  [Fact]
  public async Task ShouldEditOnNote()
  {
    const int editId = 1;

    var testEnvironment = CreateEnvironmentForTests();
    var viewModel = testEnvironment.ViewModel;
    await viewModel.LoadNotesIn.Execute().GetAwaiter();
    var replacement = CreateEdited();


    viewModel.EditNoteCommand.Execute(replacement);

    var actualNote = viewModel.Notes[editId];
    Assert.Equal(editId, replacement.Id);
    Assert.Equal(actualNote.Title, replacement.Title);
    Assert.Equal(actualNote.Content, replacement.Content);

    static NoteModel CreateEdited() => new NoteModel {Title = "New Title", Content = "New Content", Id = editId};
  }



}

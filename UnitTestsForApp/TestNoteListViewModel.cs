using NoteCollectionEditor.Models;
using NoteCollectionEditor.Services;
using NoteCollectionEditor.ViewModels;

namespace UnitTestsForApp;

public class TestNoteListViewModel
{
  [Fact]
  public async Task ShouldContainCollectionAfterLoading()
  {
    var data = new NoteListInMemorySource(new[]
    {
      new NoteModel { Title = "First", Content = "1. Content"},
      new NoteModel { Title = "Second", Content = "2. Content"},
      new NoteModel { Title = "Third", Content = "3. Content"}
    });
    var expected = await data.LoadAll();
    var viewModel = new NoteListViewModel(data);
    await viewModel.LoadNotes();
    
    Assert.Equal(viewModel.Notes, expected);
  }
}
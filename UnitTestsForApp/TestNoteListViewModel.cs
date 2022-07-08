using NoteCollectionEditor.Models;
using NoteCollectionEditor.Services;
using NoteCollectionEditor.ViewModels;

namespace UnitTestsForApp;

public class TestNoteListViewModel
{
  [Fact]
  public void ShouldContainCollectionAfterLoading()
  {
    var data = new NoteListInMemorySource(new[]
    {
      new NoteModel { Title = "First", Content = "1. Content"},
      new NoteModel { Title = "Second", Content = "2. Content"},
      new NoteModel { Title = "Third", Content = "3. Content"}
    });
    var expected = data.LoadAll();
    var viewModel = new NoteListViewModel(data);
    
    Assert.Equal(viewModel.Notes, expected);
  }
}
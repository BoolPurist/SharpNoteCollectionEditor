using System.Reactive.Linq;
using NoteCollectionEditor.Extensions;
using NoteCollectionEditor.Models;
using NoteCollectionEditor.Services;
using NoteCollectionEditor.ViewModels;

namespace UnitTestsForApp;

public class TestAlterNoteViewModel
{
  [Fact]
  public void ShouldCanExecuteIfBothFieldsAreFilled()
  {
    var viewModel = new AlterNoteViewModel();
    var command = viewModel.SubmitNewNote;

    Assert.False(
      command.CanExecute(null),
      "Should not be executable with empty fields");
    viewModel.NewContent = "b";
    Assert.False(
      command.CanExecute(null),
      "Should not be executable with only filled title"
      );
    viewModel.NewTitle = "a";

    Assert.True(
      command.CanExecute(null),
      "Should be executable with title and content filled."
      );
  }

  [Fact]
  public void ShouldFireIfSubmitCommandIsTrigger()
  {
    var viewModel = new AlterNoteViewModel();
    bool wasFired = false;
    var expectedModel = new NoteModel
    {
      Title = "Expected title",
      Content = "Expected content"
    };
    viewModel.NewContent = expectedModel.Content;
    viewModel.NewTitle = expectedModel.Title;
    viewModel.Submit +=
      (sender, actualModel) =>
      {
        Assert.Equal(
          expectedModel.Title, actualModel.Title);
        Assert.Equal(expectedModel.Content, actualModel.Content);
        wasFired = true;
      };
    viewModel.SubmitNewNote.Execute(null);
    Assert.True(wasFired, "Event Submit was not fired.");
  }

}

using NoteCollectionEditor.Models;
using NoteCollectionEditor.ViewModels;

namespace UnitTestsForApp;

public class TestAlterNoteViewModel
{
    [Fact]
    public void ShouldCanExecuteIfBothFieldsAreFilled()
    {
        var viewModel = new AlterNoteViewModel();
        viewModel.SubmitCommand();

        Assert.False(
          viewModel.CanSubmitCommand(),
          "Should not be executable with empty fields");
        viewModel.NewContent = "b";
        Assert.False(
          viewModel.CanSubmitCommand(),
          "Should not be executable with only filled title"
          );
        viewModel.NewTitle = "a";

        Assert.True(
          viewModel.CanSubmitCommand(),
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
          () =>
          {
              wasFired = true;
          };
        viewModel.SubmitCommand();
        Assert.True(wasFired, "Event Submit was not fired.");
    }

}

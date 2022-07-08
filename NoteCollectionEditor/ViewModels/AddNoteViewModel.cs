using ReactiveUI;

namespace NoteCollectionEditor.ViewModels;

public class AddNoteViewModel : ReactiveObject
{
  private string _newTitle;
  private string _newContent;

  public string NewTitle
  {
    get => _newTitle;
    set
    {
      _newTitle = value; 
      this.RaiseAndSetIfChanged(ref _newTitle, value);
    }
  }

  public string NewContent
  {
    get => _newContent;
    set
    {
      _newContent = value;
      this.RaiseAndSetIfChanged(ref _newContent, value);
    }
  }

  public AddNoteViewModel()
  {
    _newTitle = "Jonny";
    _newContent = "bla bla";
  }

  public override string ToString()
  {
    return $"NewTitle: {NewTitle}, NewContent {NewContent}";
  }
}
using System;
using System.Windows.Input;
using NoteCollectionEditor.Models;
using ReactiveUI;


namespace NoteCollectionEditor.ViewModels;

public class AlterNoteViewModel : ViewModelBase
{


  public event EventHandler<NoteModel>? Submit;

  public ICommand SubmitNewNote { get; }

  public string AcceptButtonText
  {
    get => _acceptButtonText;
    set => this.RaiseAndSetIfChanged(ref _acceptButtonText, value);
  }

  public string CancelButtonText
  {
    get => _cancelButtonText;
    set => this.RaiseAndSetIfChanged(ref _cancelButtonText, value);
  }

  public string NewTitle
  {
    get => _newTitle;
    set => this.RaiseAndSetIfChanged(ref _newTitle, value);
  }


  public bool InsertOnTop
  {
    get => _insertOnTop;
    set
    {
      Console.WriteLine($"Setter: {nameof(InsertOnTop)}");
      this.RaiseAndSetIfChanged(ref _insertOnTop, value);
    }
  }

  public string NewContent
  {
    get => _newContent;
    set
    {
      Console.WriteLine(value);
      this.RaiseAndSetIfChanged(ref _newContent, value);

    }
  }

  private string _newTitle;
  private string _newContent;
  private string _acceptButtonText = "Accept";
  private string _cancelButtonText = "Cancel";
  private bool _insertOnTop = true;

  public AlterNoteViewModel() : this(String.Empty, String.Empty)
  {
  }

  public AlterNoteViewModel(string title, string textBody)
  {
    _newTitle = title;
    _newContent = textBody;

    var canBeSubmitted = this.WhenAnyValue(
      data => data.NewTitle,
      data => data.NewContent,
      (newTitle, content) =>
        !string.IsNullOrWhiteSpace(newTitle)
        && !string.IsNullOrWhiteSpace(content)
    );

    SubmitNewNote = ReactiveCommand.Create(
      () => Submit?.Invoke(null, new NoteModel
      {
        Title = NewTitle,
        Content = NewContent
      }),
      canBeSubmitted
    );
  }

  public override string ToString()
  {
    return $"NewTitle: {NewTitle}, NewContent {NewContent}";
  }
}

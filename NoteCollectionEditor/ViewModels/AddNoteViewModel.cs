using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows.Input;
using DynamicData.Binding;
using NoteCollectionEditor.Models;
using ReactiveUI;
using Tmds.DBus;

namespace NoteCollectionEditor.ViewModels;

public class AddNoteViewModel : ViewModelBase
{
  private string _newTitle;
  private string _newContent;

  public event EventHandler<NoteModel>? Submit;

  public ICommand SubmitNewNote { get; private set; }

  public string NewTitle
  {
    get => _newTitle;
    set => this.RaiseAndSetIfChanged(ref _newTitle, value);
  }

  public string NewContent
  {
    get => _newContent;
    set => this.RaiseAndSetIfChanged(ref _newContent, value);
  }

  public AddNoteViewModel()
  {
    _newTitle = "";
    _newContent = "";

    var canBeSubmitted = this.WhenAnyValue(
      data => data.NewTitle,
      data => data.NewContent,
      (title, content) =>
        !string.IsNullOrWhiteSpace(title)
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
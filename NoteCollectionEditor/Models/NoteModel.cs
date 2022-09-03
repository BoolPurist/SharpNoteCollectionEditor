using System;
using ReactiveUI;

namespace NoteCollectionEditor.Models;

public partial class NoteModel : ReactiveObject
{
  private string? _title;
  private string? _content;
  private int _id = -1;

  public string? Title
  {
    get => _title;
    set => this.RaiseAndSetIfChanged(ref _title, value);
  }

  public string? Content
  {
    get => _content;
    set => this.RaiseAndSetIfChanged(ref _content, value);
  }

  public int Id
  {
    get => _id;
    set => this.RaiseAndSetIfChanged(ref _id, value);
  }
}

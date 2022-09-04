using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using ReactiveUI;

namespace NoteCollectionEditor.Models;


public partial class NoteModel : INotifyPropertyChanged
{
  private string? _title;
  private string? _content;
  private int _id = -1;

  public string? Title
  {
    get => _title;
    set => SetField(ref _title, value);
  }

  public string? Content
  {
    get => _content;
    set => SetField(ref _content, value);
  }

  public int Id
  {
    get => _id;
    set => SetField(ref _id, value);
  }

  public event PropertyChangedEventHandler? PropertyChanged;

  protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
  {
    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
  }

  protected void SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
  {
    field = value;
    OnPropertyChanged(propertyName);
  }
}

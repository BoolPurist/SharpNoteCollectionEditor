using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace NoteCollectionEditor.Models;


public partial class NoteModel : INotifyPropertyChanged, IEquatable<NoteModel>
{
  public event PropertyChangedEventHandler? PropertyChanged;

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



  [JsonIgnore]
  public int Id
  {
    get => _id;
    set => SetField(ref _id, value);
  }

  private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
  {
    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
  }

  // ReSharper disable once RedundantAssignment
  private void SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
  {
    field = value;
    OnPropertyChanged(propertyName);
  }

  public bool Equals(NoteModel? other)
  {
    if (ReferenceEquals(null, other)) return false;
    if (ReferenceEquals(this, other)) return true;
    return _title == other._title && _content == other._content && _id == other._id;
  }

  public override bool Equals(object? obj)
  {
    if (ReferenceEquals(null, obj)) return false;
    if (ReferenceEquals(this, obj)) return true;
    if (obj.GetType() != this.GetType()) return false;
    return Equals((NoteModel) obj);
  }

  public override int GetHashCode()
  {
    return HashCode.Combine(Title, Content, Id);
  }
}

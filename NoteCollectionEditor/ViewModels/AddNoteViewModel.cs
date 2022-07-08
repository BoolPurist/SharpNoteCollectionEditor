using System;
using ReactiveUI;
using Tmds.DBus;

namespace NoteCollectionEditor.ViewModels;

public class AddNoteViewModel : ViewModelBase
{
  private string _newTitle;
  private string _newContent;
  private bool _isSumittable = false;

  public event EventHandler<bool>? AbilityToSubmitHasChanged;

  public string NewTitle
  {
    get => _newTitle;
    set
    {
      _newTitle = value;
      CheckAbilityForSubmitHasChanged();
      this.RaiseAndSetIfChanged(ref _newTitle, value);
    }
  }

  public string NewContent
  {
    get => _newContent;
    set
    {
      _newContent = value;
      CheckAbilityForSubmitHasChanged();
      this.RaiseAndSetIfChanged(ref _newContent, value);
    }
  }

  public AddNoteViewModel()
  {
    _newTitle = "";
    _newContent = "";
  }

  public bool HasValidTitle => !string.IsNullOrWhiteSpace(NewTitle);
  public bool HasValidContent => !string.IsNullOrWhiteSpace(NewContent);

  public bool HasValidDataForNote => HasValidContent && HasValidTitle;

  private void CheckAbilityForSubmitHasChanged()
  {
    bool currentStatus = HasValidDataForNote;
    if (_isSumittable != currentStatus)
    {
      AbilityToSubmitHasChanged?.Invoke(this, currentStatus);
    }
    _isSumittable = currentStatus;
  }

  public override string ToString()
  {
    return $"NewTitle: {NewTitle}, NewContent {NewContent}";
  }
}
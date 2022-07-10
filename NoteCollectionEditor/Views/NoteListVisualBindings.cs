using ReactiveUI;

namespace NoteCollectionEditor.Views;

public class NoteListVisualBindings : ReactiveObject
{
  private double _viewHeight = 640;

  public double ViewHeight
  {
    get => _viewHeight;
    set => this.RaiseAndSetIfChanged(ref _viewHeight, value);
  }
}
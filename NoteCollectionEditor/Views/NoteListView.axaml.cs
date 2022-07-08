using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace NoteCollectionEditor.Views;

public partial class NoteListView : UserControl
{
  public NoteListView()
  {
    InitializeComponent();
  }

  private void InitializeComponent()
  {
    AvaloniaXamlLoader.Load(this);
  }
}
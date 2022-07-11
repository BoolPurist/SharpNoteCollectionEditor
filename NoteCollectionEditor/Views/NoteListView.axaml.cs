using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using NoteCollectionEditor.Extensions;
using NoteCollectionEditor.Services;
using NoteCollectionEditor.ViewModels;

namespace NoteCollectionEditor.Views;

public partial class NoteListView : UserControl
{

  public NoteListViewModel Data { get; private set; }

  public NoteListVisualBindings VisualData { get; private set; }
  public NoteListView()
  {
    Data = ServicesOfApp.Resolver.GetRequiredService<NoteListViewModel>();
    VisualData = new NoteListVisualBindings();
    
    InitializeComponent();
  }
  
  private void InitializeComponent()
  {
    AvaloniaXamlLoader.Load(this);
  }
  
}
using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using NoteCollectionEditor.Extensions;
using NoteCollectionEditor.Models;
using NoteCollectionEditor.Services;
using NoteCollectionEditor.ViewModels;


namespace NoteCollectionEditor.Views;

public partial class AlterNoteWindow : Window
{


  public AlterNoteViewModel Data { get; set; }



  public AlterNoteWindow() : this(String.Empty, String.Empty)
  {
  }

  public AlterNoteWindow(string startTitle, string startContent)
  {
    InitializeComponent();

    Data = ServicesOfApp.Resolver.GetRequiredService<AlterNoteViewModel>();
    DataContext = this;
    Data.Submit += OnSubmit;
    Data.NewContent = startContent;
    Data.NewTitle = startTitle;

#if DEBUG
    this.AttachDevTools();
#endif
  }

  public void SetAcceptButtonText(string newText) => Data.AcceptButtonText = newText;

  private void OnSubmit(object? sender, NoteModel toSubmit)
  {
    Close(new NoteModel() { Content = toSubmit.Content, Title = toSubmit.Title} );
  }

  // ReSharper disable once UnusedParameter.Local
  protected void OnClickCancel(object? sender, RoutedEventArgs e)
  {
    // Prevents freezing if cancel is pressed in ide preview design mode.
    if (Design.IsDesignMode) return;
    Close(null);
  }

  private void InitializeComponent()
  {
    AvaloniaXamlLoader.Load(this);
  }

  private void OnClosed(EventArgs e)
  {
    base.OnClosed(e);
    Data.Submit -= OnSubmit;
  }
}

using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using NoteCollectionEditor.Models;
using NoteCollectionEditor.ViewModels;

namespace NoteCollectionEditor.Views;

public partial class AddNoteView : Window
{
  public AddNoteViewModel Data { get; set; }

  public AddNoteView()
  {
    InitializeComponent();
    Data = new AddNoteViewModel();
    DataContext = Data;
#if DEBUG
    this.AttachDevTools();
#endif
  }

  private void InitializeComponent()
  {
    AvaloniaXamlLoader.Load(this);
  }

  private void OnClickCreateNewNote(object? sender, RoutedEventArgs e)
  {
    var newNote = new NoteModel()
    {
      Title = Data.NewTitle,
      Content = Data.NewContent
    };
    
    Close(newNote);
  }

  private void OnClickCancel(object? sender, RoutedEventArgs e)
  {
    Close(null);
  }
}
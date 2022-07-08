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
    DataContext = this;
#if DEBUG
    this.AttachDevTools();
#endif
  }

  private void InitializeComponent()
  {
    AvaloniaXamlLoader.Load(this);
  }

  public void CreateNewNote()
  {
    var newNote = new NoteModel()
    {
      Title = Data.NewTitle,
      Content = Data.NewContent
    };
    
    Close(newNote);
  }

  public bool CanCreateNewNote(object parameter)
  {
    return true;
  }

  private void OnClickCancel(object? sender, RoutedEventArgs e)
  {
    Close(null);
  }
}
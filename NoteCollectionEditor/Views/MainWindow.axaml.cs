
using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using NoteCollectionEditor.Models;

namespace NoteCollectionEditor.Views
{
  public partial class MainWindow : Window
  {
    // TODO: Find a way to calculate this depending on the layout.
    private const double OffsetHeightScrollViewerNotes = 150;
    
    public MainWindow()
    {
      InitializeComponent();
      DataContext = this;
    }


    private async void OnClickSpawnAddNoteWindow(object? sender, RoutedEventArgs e)
    {
      var windowAddingNote = new AddNoteView();
      var newNote = await windowAddingNote.ShowDialog<NoteModel>(this);
      if (newNote != null)
      {
        // Add new note to view model.
        ListOfNotes.Data.AddNoteCommand.Execute(newNote);
      }
      
    }


    private void OnWindowSizeChanged(object? sender, EventArgs e)
    {
      double newViewHeightOfNotes = Math.Max(0, Height - OffsetHeightScrollViewerNotes);
      ListOfNotes.VisualData.ViewHeight = newViewHeightOfNotes;
    }
  }
}
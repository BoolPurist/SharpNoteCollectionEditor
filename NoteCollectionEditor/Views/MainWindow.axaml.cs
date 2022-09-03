using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Interactivity;
using NoteCollectionEditor.Models;

namespace NoteCollectionEditor.Views
{
  public partial class MainWindow : Window
  {
    // TODO: Find a way to calculate this depending on the layout.
    private const double OffsetHeightScrollViewerNotes = 150;
    private const string AddNoteButtonText = "Add Note.";

    public MainWindow()
    {
      InitializeComponent();
      DataContext = this;
    }


    private async void OnClickSpawnAddNoteWindow(object? sender, RoutedEventArgs e)
    {
      var windowAddingNote = new AlterNoteWindow();
      windowAddingNote.SetAcceptButtonText(AddNoteButtonText);
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

    private void TopLevel_OnOpened(object? sender, EventArgs e)
    {
      if (!Design.IsDesignMode)
      {
        ListOfNotes.Data.LoadNotesIn.Execute();
      }
    }
  }
}

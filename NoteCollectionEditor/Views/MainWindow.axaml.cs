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
    private const double OffsetHeightScrollViewerNotes = 200;
    private const string AddNoteButtonText = "Add Note.";

    public MainWindow()
    {
      InitializeComponent();
    }


    public async Task SpawnDialogForAddNote()
    {
      var windowAddingNote = new AlterNoteWindow();
      windowAddingNote.SetAcceptButtonText(AddNoteButtonText);
      var newNote = await windowAddingNote.ShowDialog<NoteModel>(this);
      if (newNote != null)
      {
        // Add new note to view model.
        ListOfNotes.Data.CommandAddNote(newNote);
      }
    }


    private void OnWindowSizeChanged(object? sender, EventArgs e)
    {
      double newViewHeightOfNotes = Math.Max(0, Height - OffsetHeightScrollViewerNotes);
      ListOfNotes.VisualData.ViewHeight = newViewHeightOfNotes;
    }

    private async void TopLevel_OnOpened(object? sender, EventArgs e)
    {
      if (!Design.IsDesignMode)
      {
        await ListOfNotes.Data.CommandLoadNotes();
      }
    }
  }
}

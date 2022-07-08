
using Avalonia.Controls;
using Avalonia.Interactivity;
using NoteCollectionEditor.Models;

namespace NoteCollectionEditor.Views
{
  public partial class MainWindow : Window
  {
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
        ListOfNotes.Data.Notes.Add(newNote);
      }
      

    }

    
  }
}
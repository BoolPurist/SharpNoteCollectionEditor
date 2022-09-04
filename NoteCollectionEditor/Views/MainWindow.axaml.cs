using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Interactivity;
using NoteCollectionEditor.Extensions;
using NoteCollectionEditor.Models;
using NoteCollectionEditor.Services;
using NoteCollectionEditor.Views.Dialogs;
using Splat;

namespace NoteCollectionEditor.Views
{
  public partial class MainWindow : Window
  {
    // TODO: Find a way to calculate this depending on the layout.
    private const double OffsetHeightScrollViewerNotes = 200;
    private const string AddNoteButtonText = "Add Note.";
    private const string InitialNameForExportedNoteJsonFile = "exported_notes.json";


    private readonly ILogger _logger;

    public MainWindow()
    {
      _logger = ServicesOfApp.Resolver.GetRequiredService<ILogger>();
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

    public async Task CommandSpawnImportDialogForNotes()
    {
      var fileDialog = new OpenFileDialog
      {
        AllowMultiple = false
      };
      fileDialog.Filters.Add(new FileDialogFilter {Name = "json file", Extensions = {"json"}});
      await fileDialog.ShowAsync(this);
    }

    public async Task CommandSpawnExportDialogForNotes()
    {
      var saveDialog = CreateDialogForSaving();
      var pathToExport = await saveDialog.ShowAsync(this);

      // User has canceled export dialog.
      if (pathToExport == null) return;

      bool exportIt = true;
      if (File.Exists(pathToExport))
      {
        var popUp = CreateWarningDialog();
        // Ask user if he/she really wants to override the file.
        exportIt  = await popUp.ShowDialog<bool>(this);
      }

      if(!exportIt) return;

      var exportContent = ListOfNotes.Data.CreateExportJson();
      await File.WriteAllTextAsync(pathToExport, exportContent);
      _logger.LogInfo($"Exported note list to path {pathToExport}");

      SaveFileDialog CreateDialogForSaving()
        => new() {InitialFileName = InitialNameForExportedNoteJsonFile};

      AskPopUpDialog CreateWarningDialog()
        => new()
        {
          WarningText = $"File at {pathToExport} already exits !\nDo you want to override it ?",
          ConfirmDespiteWarningText = "Override file"
        };
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

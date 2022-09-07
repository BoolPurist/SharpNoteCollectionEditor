using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using NoteCollectionEditor.Extensions;
using NoteCollectionEditor.Models;
using NoteCollectionEditor.Services;
using NoteCollectionEditor.ViewModels;
using NoteCollectionEditor.Views.Dialogs;
using Splat;

namespace NoteCollectionEditor.Views
{
  public partial class MainWindow : Window
  {
    // TODO: Find a way to calculate this depending on the layout.
    private const double OffsetHeightScrollViewerNotes = 200;
    private const string AddNoteButtonText = "Add Note.";
    private readonly NoteListViewModel _viewModel;

    private readonly ILogger _logger;

    private readonly IPathStoreImportExport _importExportPathHistory;

    public MainWindow()
    {
      _logger = ServicesOfApp.Resolver.GetRequiredService<ILogger>();
      _importExportPathHistory = ServicesOfApp.Resolver.GetRequiredService<IPathStoreImportExport>();

      InitializeComponent();

      _viewModel = ListOfNotes.Data;
    }


    public async Task SpawnDialogForAddNote()
    {
      var windowAddingNote = new AlterNoteWindow();
      windowAddingNote.SetAcceptButtonText(AddNoteButtonText);
      var newNote = await windowAddingNote.ShowDialog<NoteModel>(this);
      if (newNote != null)
      {
        // Add new note to view model.
        _viewModel.CommandAddNote(newNote);
      }
    }

    public async Task CommandSpawnImportDialogForNotes()
    {
      var loadDialog = CreateLoadDialog();
      var pathToLoad = await loadDialog.ShowAsync(this);

      // User has canceled import.
      if (pathToLoad == null) return;

      string importPath = pathToLoad.First();
      var contentForImport = await File.ReadAllTextAsync(importPath);

      var hasImported = _viewModel.ImportNoteListFromJson(contentForImport);

      if (!hasImported)
      {
        await CreateErrorPopUp().ShowDialog(this);
      }

      _importExportPathHistory.SetImportFromFullPath(importPath);

      OpenFileDialog CreateLoadDialog()
      {
        var fileDialog = new OpenFileDialog
        {
          AllowMultiple = false,
          Title = "Import a note list",
          Directory = _importExportPathHistory.ImportDirectoryPath
        };
        Debug.Assert(fileDialog.Filters != null);
        fileDialog.Filters.Add(new FileDialogFilter {Name = "json file", Extensions = {"json"}});
        return fileDialog;
      }

      ErrorMessagePopUpDialog CreateErrorPopUp()
      {
        var errorPopup = new ErrorMessagePopUpDialog
        {
          ErrorText = "File for import is not valid"
        };
        return errorPopup;
      }
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
        exportIt = await popUp.ShowDialog<bool>(this);
      }

      if (!exportIt) return;

      var exportContent = _viewModel.CreateExportJson();
      await File.WriteAllTextAsync(pathToExport, exportContent);
      _importExportPathHistory.SetExportFromFullPath(pathToExport);
      _logger.LogInfo($"Exported note list to path {pathToExport}");

      SaveFileDialog CreateDialogForSaving()
        => new()
        {
          InitialFileName = _importExportPathHistory.LastSavedName,
          Title = "Export current note list",
          Directory = _importExportPathHistory.ExportDirectoryPath,

        };

      AskPopUpDialog CreateWarningDialog()
        => new()
        {
          WarningText = $"File at {pathToExport} already exits !\nDo you want to override it ?",
          ConfirmDespiteWarningText = "Override file"
        };
    }



    private void OnWindowSizeChanged(
      // ReSharper disable once UnusedParameter.Local
      object? sender,
      // ReSharper disable once UnusedParameter.Local
      EventArgs e)
    {
      double newViewHeightOfNotes = Math.Max(0, Height - OffsetHeightScrollViewerNotes);
      ListOfNotes.VisualData.ViewHeight = newViewHeightOfNotes;
    }


    private async void TopLevel_OnOpened(
      // ReSharper disable once UnusedParameter.Local
      object? sender,
      // ReSharper disable once UnusedParameter.Local
      EventArgs e
      )
    {
      if (!Design.IsDesignMode)
      {
        await _viewModel.CommandLoadNotes();
      }
    }
  }
}

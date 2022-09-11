using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.Json;
using System.Threading.Tasks;
using Avalonia.Metadata;
using DynamicData;
using NoteCollectionEditor.Extensions;
using NoteCollectionEditor.Models;
using NoteCollectionEditor.Services;
using ReactiveUI;
using Splat;

namespace NoteCollectionEditor.ViewModels;

public class NoteListViewModel : ReactiveObject
{
  public string ErrorLoadingMessage => "Unable to load notes";

  /// <summary>
  /// True if no notes are found in a normal case.
  /// Normal case means no notes are loaded/saved and no error occured during loading/saving.
  /// </summary>
  public bool NoNotesFoundInNormalCase => !ErrorInLoading && !IsLoading && Notes.Count == 0;

  private ReadOnlyObservableCollection<NoteModel>? _notesProxyOutside;

  public ReadOnlyObservableCollection<NoteModel> Notes
  {
    get
    {
      _notesProxyOutside ??= new ReadOnlyObservableCollection<NoteModel>(_notes);
      return _notesProxyOutside;
    }
  }

  public bool IsLoading
  {
    get => _isLoading;
    set => this.RaiseAndSetIfChanged(ref _isLoading, value);
  }

  public bool ErrorInLoading
  {
    get => _errorInLoading;
    set => this.RaiseAndSetIfChanged(ref _errorInLoading, value);
  }

  public bool IsSaving
  {
    get => _isSaving;
    private set => this.RaiseAndSetIfChanged(ref _isSaving, value);
  }

  public void SetNoteCollection(IEnumerable<NoteModel> newCollection)
  {
    _notes.Clear();
    _notes.AddRange(newCollection);
    AdjustIdToPosition(_notes);
    this.RaisePropertyChanged(nameof(Notes));
  }

  private readonly INoteListRepository _dataSource;
  private readonly ILogger _logger;
  private bool _errorInLoading;
  private bool _isLoading;
  private readonly ObservableCollection<NoteModel> _notes = new();
  private bool _isSaving;

  public NoteListViewModel(INoteListRepository repository, ILogger logger)
  {
    _dataSource = repository;
    _logger = logger;
  }

  [DependsOn(nameof(IsLoading)), DependsOn(nameof(IsSaving))]
  public bool CanCommandLoadNotes(object parameter) => !IsLoading && !IsSaving;

  public async Task CommandLoadNotes()
  {
    if (!CanCommandLoadNotes(null!))
    {
      _logger.LogWarning(
        $"Command {nameof(CommandLoadNotes)} was triggered despite of {nameof(CanCommandLoadNotes)} being false.");
      return;
    }

    try
    {
      OnStartLoading();
      var notes = await _dataSource.LoadAll();
      SetNoteCollection(notes);
      OnLoadingFinished();
    }
    catch (Exception exception)
    {
      HandleLoadingError(exception);
    }
  }

  [DependsOn(nameof(IsSaving)), DependsOn(nameof(IsLoading))]
  public bool CanCommandSaveNotes(object parameter) => !IsSaving && !IsLoading;

  public async Task CommandSaveNotes()
  {
    if (!CanCommandSaveNotes(null!))
    {
      _logger.LogWarning(
        $"Command {nameof(CommandSaveNotes)} was triggered despite of {nameof(CanCommandSaveNotes)} being false.");
      return;
    }

    _logger.LogDebug("Starting saving notes");
    IsSaving = true;
    await _dataSource.SaveAll(_notes);

    IsSaving = false;
  }

  public void CommandAddNoteOnBottom(NoteModel toAdd)
  {
    toAdd.Id = _notes.Count;
    _notes.Add(toAdd);
    _logger.LogDebug($"Note has been added on the bottom at index {toAdd.Id}.");
    OnNotesChanged();
  }

  public void CommandAddNoteOnTop(NoteModel toAdd)
  {
    toAdd.Id = _notes.Count;
    _notes.Insert(0, toAdd);
    _logger.LogDebug($"Note has been added on the top at index {toAdd.Id}.");
    OnNotesChanged();
  }

  public void CommandEditNote(NoteModel toEdit)
  {
    _notes[toEdit.Id] = toEdit;
    _logger.LogDebug($"Changed note at {toEdit.Id} to \n{toEdit}");
    OnNotesChanged();
  }

  public void CommandDeleteNote(int deleteId)
  {
    _notes.RemoveAt(deleteId);
    AdjustIdToPosition(_notes);
    _logger.LogDebug($"Removed note at {deleteId}");
    OnNotesChanged();
  }

  public bool CanCommandMoveDownNote(int index)
  {
    var downIndex = index + 1;
    int max = _notes.Count - 1;
    return downIndex <= max;
  }

  public bool CanCommandMoveUpNote(int index)
  {
    var max = _notes.Count - 1;
    return index > 0;
  }

  /// <summary>
  /// Moves note at index up  the list.
  /// Swaps the chosen element with its left neighbor element.
  /// </summary>
  /// <example>
  /// Example: index = 1: element goes to index 0 and the previous element at index 0 goes to index 1.
  /// </example>
  public void CommandMoveUpNote(int index)
  {
    int indexToUp = index - 1;
    (_notes[index], _notes[indexToUp]) = (_notes[indexToUp], _notes[index]);
    AdjustIdToPosition(_notes);
  }

  /// <summary>
  /// Moves note at index down the list.
  /// Swaps the chosen element with its right neighbor element.
  /// </summary>
  /// <example>
  /// Example: index = 2: element goes to index 2 and the previous element at index 2 goes to index 1.
  /// </example>
  public void CommandMoveDownNote(int index)
  {
    int indexToUp = index + 1;
    (_notes[index], _notes[indexToUp]) = (_notes[indexToUp], _notes[index]);
    AdjustIdToPosition(_notes);
  }

  public string CreateExportJson() => JsonSerializer.Serialize(_notes);

  public bool CommandImportNoteListFromJson(string json)
  {
    try
    {
      var importedNoteList = JsonSerializer.Deserialize<List<NoteModel>>(json);
      if (importedNoteList == null)
      {
        _logger.LogError($"Imported note list has valid format but is still null !");
        return false;
      }
      SetNoteCollection(importedNoteList);
    }
    catch (JsonException exception)
    {
      _logger.LogInfo(
        $"File as json could not be converted to a List of type {nameof(NoteModel)}.\n{exception.Message}"
      );
      return false;
    }

    return true;
  }

  private static void AdjustIdToPosition(IEnumerable<NoteModel> toAdjust)
  {
    int newNextId = 0;
    foreach (var note in toAdjust)
    {
      note.Id = newNextId++;
    }
  }

  private void NotifyIfNotesWereFound()
    => this.RaisePropertyChanged(nameof(NoNotesFoundInNormalCase));

  private void HandleLoadingError(Exception thrown)
  {
    OnLoadingError();
    _logger.LogExceptionAsError(thrown, "Error in loading");
  }

  private void OnNotesChanged()
  {
    ErrorInLoading = false;
    IsLoading = false;
    NotifyIfNotesWereFound();
  }

  private void OnStartLoading()
  {
    _logger.LogInfo("Loading notes.");
    IsLoading = true;
    ErrorInLoading = false;
    NotifyIfNotesWereFound();
  }

  private void OnLoadingFinished()
  {
    _logger.LogInfo("Loading of notes completed successively.");
    IsLoading = false;
    ErrorInLoading = false;
    NotifyIfNotesWereFound();
  }

  private void OnLoadingError()
  {
    IsLoading = false;
    ErrorInLoading = true;
    NotifyIfNotesWereFound();
  }
}

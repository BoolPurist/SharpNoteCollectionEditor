using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Metadata;
using DynamicData.Binding;
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

  public ObservableCollection<NoteModel> Notes
  {
    get => _notes;
    set
    {
      AdjustIdToPosition(value);
      _notes = value;
      this.RaisePropertyChanged();
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

  private readonly INoteListRepository _dataSource;
  private readonly ILogger _logger;
  private bool _errorInLoading;
  private bool _isLoading;
  private ObservableCollection<NoteModel> _notes = new ObservableCollection<NoteModel>();
  private bool _isSaving;

  public NoteListViewModel(INoteListRepository repository, ILogger logger)
  {
    _dataSource = repository;
    _logger = logger;
  }

  public async Task CommandLoadNotes()
  {
    try
    {
      OnStartLoading();
      var notes = await _dataSource.LoadAll();
      Notes = new ObservableCollectionExtended<NoteModel>(notes);
      OnLoadingFinished();
    }
    catch (Exception exception)
    {
      HandleLoadingError(exception);
    }
  }

  [DependsOn(nameof(IsSaving))]
  public bool CanCommandSaveNotes(object parameter) => !IsSaving;

  public async Task CommandSaveNotes()
  {
    if (!CanCommandSaveNotes(null!))
    {
      _logger.LogWarning($"Command was triggered despite of {nameof(CanCommandSaveNotes)} being false.");
      return;
    }

    _logger.LogDebug("Starting saving notes");
    IsSaving = true;
    await _dataSource.SaveAll(_notes.ToList());
    IsSaving = false;
  }

  public void CommandAddNote(NoteModel toAdd)
  {
    toAdd.Id = _notes.Count;
    Notes.Add(toAdd);
    _logger.LogDebug("Note has been added.");
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

  private void AdjustIdToPosition(IEnumerable<NoteModel> toAdjust)
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

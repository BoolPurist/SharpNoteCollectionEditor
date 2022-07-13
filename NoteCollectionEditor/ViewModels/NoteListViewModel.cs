using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using DynamicData;
using DynamicData.Binding;
using NoteCollectionEditor.Extensions;
using NoteCollectionEditor.Models;
using NoteCollectionEditor.Services;
using ReactiveUI;
using Splat;

namespace NoteCollectionEditor.ViewModels;



public class NoteListViewModel : ReactiveObject
{
  
  public IObservableCollection<NoteModel> Notes { get; set; } = new ObservableCollectionExtended<NoteModel>();

  public string ErrorLoadingMessage { get; } = "Unable to load notes";

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

  /// <summary>
  /// True if no notes are found in a normal case.
  /// Normal case means no notes are loaded/saved and no error occured during loading/saving.
  /// </summary>
  public bool NoNotesFoundInNormalCase => !ErrorInLoading && !IsLoading && Notes.Count == 0;

  private void NotifyIfNotesWereFound() 
    => this.RaisePropertyChanged(nameof(NoNotesFoundInNormalCase));



  private readonly INoteListRepository _dataSource;
  private readonly ILogger _logger;
  private bool _errorInLoading;
  private bool _isLoading;

  public NoteListViewModel(INoteListRepository repository, ILogger logger)
  {
    _dataSource = repository;
    _logger = logger;
    AddNoteCommand = ReactiveCommand.Create<NoteModel>(AddNote);
    LoadNotesIn = ReactiveCommand.CreateFromTask(LoadNotes);
  }
  
  /// <summary>
  /// Takes a NoteModel as parameter and
  /// adds it as new entity to the collection of notes.
  /// </summary>
  public ICommand AddNoteCommand { get; private set; }
  
  public ReactiveCommand<Unit, Unit> LoadNotesIn { get; private set; }

  private void HandleLoadingError(Exception thrown)
  {
    OnLoadingError();
    _logger.LogExceptionAsError(thrown, "Error in loading");
  }
  
  private async Task LoadNotes()
  {
    try
    {
      OnStartLoading();
      
      var notes = await _dataSource.LoadAll();
      Notes.Clear();
      Notes.AddRange(notes);
      OnLoadingFinished();
    }
    catch (Exception exception)
    {
      HandleLoadingError(exception);
    }
  }

  private void AddNote(NoteModel toAdd)
  {
    Notes.Add(toAdd);
    _logger.LogDebug("Note has been added.");
    OnNotesChanged();
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
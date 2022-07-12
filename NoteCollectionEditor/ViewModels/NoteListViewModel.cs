using System;
using System.Collections.Generic;
using System.Reactive;
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

  public bool ErrorInLoading
  {
    get => _errorInLoading;
    set => this.RaiseAndSetIfChanged(ref _errorInLoading, value);
  }

  private readonly INoteListRepository _dataSource;
  private readonly ILogger _logger;
  private bool _errorInLoading;

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
    ErrorInLoading = true;
    _logger.LogExceptionAsError(thrown, "Error in loading");
  }
  
  private async Task LoadNotes()
  {
    try
    {
      var notes = await _dataSource.LoadAll();
      Notes.Clear();
      Notes.AddRange(notes);
    }
    catch (Exception exception)
    {
      HandleLoadingError(exception);
    }
  }

  private void AddNote(NoteModel toAdd)
  {
    ErrorInLoading = false;
    Notes.Add(toAdd);
  }
}
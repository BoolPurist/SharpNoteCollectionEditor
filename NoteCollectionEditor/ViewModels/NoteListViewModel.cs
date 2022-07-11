using System;
using System.Collections.ObjectModel;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using DynamicData.Binding;
using NoteCollectionEditor.Models;
using NoteCollectionEditor.Services;
using ReactiveUI;

namespace NoteCollectionEditor.ViewModels;

public class NoteListViewModel : ReactiveObject
{
  private readonly INoteListRepository _dataSource;
  
  public NoteListViewModel(INoteListRepository repository)
  {
    _dataSource = repository;
    Notes = new ObservableCollectionExtended<NoteModel>();
    InitializeCommands();

    RxApp.MainThreadScheduler.Schedule(LoadAtStartUp);

    void InitializeCommands()
    {
      AddNoteCommand = ReactiveCommand.Create<NoteModel>(AddNote);
    }
  }

  private async void LoadAtStartUp()
  {
    await LoadNotes();
  }

  public async Task LoadNotes()
  {
    var notes = await _dataSource.LoadAll();
    Notes = new ObservableCollectionExtended<NoteModel>(notes);
  }

  private void AddNote(NoteModel toAdd) => Notes.Add(toAdd);

  /// <summary>
  /// Takes a NoteModel as parameter and
  /// adds it as new entity to the collection of notes.
  /// </summary>
  public ICommand AddNoteCommand { get; private set; }

  public IObservableCollection<NoteModel> Notes { get; private set; }
}
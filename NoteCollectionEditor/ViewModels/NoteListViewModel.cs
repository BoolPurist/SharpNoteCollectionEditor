using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using DynamicData;
using DynamicData.Binding;
using NoteCollectionEditor.Models;
using NoteCollectionEditor.Services;
using ReactiveUI;

namespace NoteCollectionEditor.ViewModels;

public class NoteListViewModel : ReactiveObject
{
  public IObservableCollection<NoteModel> Notes { get; }
  
  private readonly INoteListRepository _dataSource;

  public NoteListViewModel(INoteListRepository repository)
  {
    _dataSource = repository;
    
    Notes = new ObservableCollectionExtended<NoteModel>();
    
    AddNoteCommand = ReactiveCommand.Create<NoteModel>(AddNote);
    LoadNotesIn = ReactiveCommand.CreateFromTask(LoadNotes);
  }

  public ReactiveCommand<Unit, Unit> LoadNotesIn { get; private set; }

  private async Task LoadNotes()
  {
    var notes = await _dataSource.LoadAll();
    Notes.Clear();
    Notes.AddRange(notes);
  }
  
  private void AddNote(NoteModel toAdd) => Notes.Add(toAdd);

  /// <summary>
  /// Takes a NoteModel as parameter and
  /// adds it as new entity to the collection of notes.
  /// </summary>
  public ICommand AddNoteCommand { get; private set; }


}
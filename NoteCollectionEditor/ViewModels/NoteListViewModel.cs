using System.Collections.ObjectModel;
using System.Windows.Input;
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
    Notes = new ObservableCollection<NoteModel>(_dataSource.LoadAll());
    InitializeCommands();

    void InitializeCommands()
    {
      AddNoteCommand = ReactiveCommand.Create<NoteModel>(AddNote);
    }
  }

  private void AddNote(NoteModel toAdd) => Notes.Add(toAdd);

  /// <summary>
  /// Takes a NoteModel as parameter and
  /// adds it as new entity to the collection of notes.
  /// </summary>
  public ICommand AddNoteCommand { get; private set; }

  public ObservableCollection<NoteModel> Notes { get; private set; }
}
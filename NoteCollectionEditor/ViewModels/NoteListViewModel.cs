using System.Collections.ObjectModel;
using NoteCollectionEditor.Models;
using NoteCollectionEditor.Services;
using ReactiveUI;

namespace NoteCollectionEditor.ViewModels;

public class NoteListViewModel : ReactiveObject
{
  private INoteListRepository _dataSource;
  
  public NoteListViewModel(INoteListRepository repository)
  {
    _dataSource = repository;
    Notes = new ObservableCollection<NoteModel>(_dataSource.LoadAll());
  }
  
  public ObservableCollection<NoteModel> Notes { get; set; }
}
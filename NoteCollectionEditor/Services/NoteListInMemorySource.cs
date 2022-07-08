using System.Collections.Generic;
using NoteCollectionEditor.Models;
using NoteCollectionEditor.ViewModels;

namespace NoteCollectionEditor.Services;

public class NoteListInMemorySource : INoteListRepository
{
  private IEnumerable<NoteModel> _data;

  public NoteListInMemorySource(IEnumerable<NoteModel> data)
  {
    _data = data;
  }

  public IEnumerable<NoteModel> LoadAll()
  {
    return _data;
  }
}
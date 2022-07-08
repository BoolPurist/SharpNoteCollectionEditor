using System.Collections.Generic;
using NoteCollectionEditor.Models;

namespace NoteCollectionEditor.Services;

public class NoteListInMemorySource : INoteListRepository
{
  private readonly IEnumerable<NoteModel> _data;

  public NoteListInMemorySource(IEnumerable<NoteModel> data)
  {
    _data = data;
  }

  public IEnumerable<NoteModel> LoadAll()
  {
    return _data;
  }
}
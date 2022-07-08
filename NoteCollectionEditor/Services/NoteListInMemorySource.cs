using System.Collections.Generic;
using NoteCollectionEditor.Models;

namespace NoteCollectionEditor.Services;

public class NoteListInMemorySource : INoteListRepository
{
  public IEnumerable<NoteModel> LoadAll()
  {
    return new List<NoteModel>()
    {
      new NoteModel()
      {
        Title = "The very 1. Note",
        Content = "More to come."
      },
      new NoteModel()
      {
        Title = "The very 2. Note",
        Content = "Some more content."
      }
    };
  }
}
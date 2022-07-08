using System.Collections.Generic;
using NoteCollectionEditor.Models;

namespace NoteCollectionEditor.Services;


public interface INoteListRepository
{
  IEnumerable<NoteModel> LoadAll();
}
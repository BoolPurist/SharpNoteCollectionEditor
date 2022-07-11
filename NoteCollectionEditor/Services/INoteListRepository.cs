using System.Collections.Generic;
using System.Threading.Tasks;
using NoteCollectionEditor.Models;

namespace NoteCollectionEditor.Services;


public interface INoteListRepository
{
  Task<IEnumerable<NoteModel>> LoadAll();
}
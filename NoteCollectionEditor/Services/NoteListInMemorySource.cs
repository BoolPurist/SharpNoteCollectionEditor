using System.Collections.Generic;
using System.Threading.Tasks;
using NoteCollectionEditor.Models;

namespace NoteCollectionEditor.Services;

public class NoteListInMemorySource : INoteListRepository
{
  private readonly IEnumerable<NoteModel> _data;

  public NoteListInMemorySource(IEnumerable<NoteModel> data)
  {
    _data = data;
  }

  public async Task<IEnumerable<NoteModel>> LoadAll()
  {
    return await Task.FromResult(_data);
  }
}
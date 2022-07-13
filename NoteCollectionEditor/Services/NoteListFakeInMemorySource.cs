using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NoteCollectionEditor.ConfigMapping;
using NoteCollectionEditor.Models;

namespace NoteCollectionEditor.Services;

public class NoteListFakeInMemorySource : INoteListRepository
{
  private readonly IEnumerable<NoteModel> _data;
  public bool ThrowErrorInLoading { get; set; }

  public NoteListFakeInMemorySource(
    IEnumerable<NoteModel> data, 
    IAppConfigs configs)
  {
    _data = data;
    ThrowErrorInLoading = configs.DataSource.LoadCrashes;
  }

  public async Task<IEnumerable<NoteModel>> LoadAll()
  {
    if (ThrowErrorInLoading)
    {
      throw new InvalidOperationException("Error in loading");
    }

    return await Task.FromResult(_data);
  }
}
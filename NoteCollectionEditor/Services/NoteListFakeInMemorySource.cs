using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NoteCollectionEditor.ConfigMapping;
using NoteCollectionEditor.Models;

namespace NoteCollectionEditor.Services;

public class NoteListFakeInMemorySource : INoteListRepository
{
  private IEnumerable<NoteModel> _data;
  public bool ThrowErrorInLoading { get; set; }
  public int LoadDelay { get; set; }
  private bool HasLoadDelay => LoadDelay != 0;

  public List<NoteModel> Data => new (_data);

  public NoteListFakeInMemorySource(
    IEnumerable<NoteModel> data,
    IAppConfigs configs)
  {
    _data = data;
    ThrowErrorInLoading = configs.DataSource.LoadCrashes;
    LoadDelay = configs.DataSource.LoadDelay;
  }

  public async Task<IEnumerable<NoteModel>> LoadAll()
  {
    if (ThrowErrorInLoading)
    {
      throw new InvalidOperationException("Error in loading");
    }

    if (HasLoadDelay)
    {
      await Task.Delay(LoadDelay);
    }

    return await Task.FromResult(_data);
  }

  public Task SaveAll(IEnumerable<NoteModel> toSave)
  {
    _data = toSave;
    return Task.CompletedTask;
  }
}

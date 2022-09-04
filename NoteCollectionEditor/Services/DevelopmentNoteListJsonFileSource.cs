using System.Collections.Generic;
using System.Threading.Tasks;
using NoteCollectionEditor.ConfigMapping;
using NoteCollectionEditor.Models;
using Splat;

namespace NoteCollectionEditor.Services;

public class DevelopmentNoteListJsonFileSource : NoteListJsonFileSource
{
  private readonly int _loadingDelay;
  private readonly int _savingDelay;

  public DevelopmentNoteListJsonFileSource(IAppConfigs configs, ILogger logger) : base(configs, logger)
  {
    _loadingDelay = Configs.DevelopmentConfiguration.LoadDelay;
    _savingDelay = Configs.DevelopmentConfiguration.SaveDelay;
  }

  public override async Task SaveAll(IEnumerable<NoteModel> toSave)
  {
    await Task.Delay(_savingDelay);
    await base.SaveAll(toSave);
  }

  public override async Task<IEnumerable<NoteModel>> LoadAll()
  {
    await Task.Delay(_loadingDelay);
    return await base.LoadAll();
  }
}

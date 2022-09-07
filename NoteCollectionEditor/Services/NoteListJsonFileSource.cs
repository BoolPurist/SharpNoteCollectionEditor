using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using NoteCollectionEditor.ConfigMapping;
using NoteCollectionEditor.Extensions;
using NoteCollectionEditor.Models;
using Splat;

namespace NoteCollectionEditor.Services;

public class NoteListJsonFileSource : INoteListRepository
{
  protected readonly IAppConfigs Configs;
  private readonly ILogger _logger;

  public NoteListJsonFileSource(IAppConfigs configs, ILogger logger)
  {
    Configs = configs;
    _logger = logger;
  }

  public virtual async Task<IEnumerable<NoteModel>> LoadAll()
  {
    string content = await File.ReadAllTextAsync(Configs.PathToNoteSource);
    return JsonSerializer.Deserialize<List<NoteModel>>(content) ?? Enumerable.Empty<NoteModel>();
  }

  public virtual async Task SaveAll(IEnumerable<NoteModel> toSave)
  {
    string toWrite = GetSerialized(toSave);
    await File.WriteAllTextAsync(Configs.PathToNoteSource, toWrite);
    _logger.LogInfo($"Save notes at {Configs.PathToNoteSource}");
  }

  public void EnsureNeededFiles()
  {
    if (!File.Exists(Configs.PathToNoteSource))
    {
      SaveAllSync(Enumerable.Empty<NoteModel>());
    }
  }

  public void SaveAllSync(IEnumerable<NoteModel> toSave)
  {
    string toWrite = GetSerialized(toSave);
    File.WriteAllText(Configs.PathToNoteSource, toWrite);
  }

  private readonly JsonSerializerOptions _prettyJsonOption = new()
  {
    WriteIndented = true
  };

  private readonly JsonSerializerOptions _normalJsonOptions = new();

  private string GetSerialized(IEnumerable<NoteModel> toSave)
  {
    var options = EnvironmentUtils.IsInDevelopment() ? _prettyJsonOption : _normalJsonOptions;
    return JsonSerializer.Serialize(toSave, options);
  }


}

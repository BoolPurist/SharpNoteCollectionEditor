using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml;
using NoteCollectionEditor.ConfigMapping;
using NoteCollectionEditor.Extensions;
using NoteCollectionEditor.Models;
using Splat;

namespace NoteCollectionEditor.Services;

public class NoteListJsonFileSource : INoteListRepository
{
  private readonly IAppConfigs _configs;
  private readonly ILogger _logger;

  public NoteListJsonFileSource(IAppConfigs configs, ILogger logger)
  {
    _configs = configs;
    _logger = logger;
  }


  public async Task<IEnumerable<NoteModel>> LoadAll()
  {
    string content = await File.ReadAllTextAsync(_configs.PathToNoteSource);
    return JsonSerializer.Deserialize<List<NoteModel>>(content) ?? Enumerable.Empty<NoteModel>();
  }

  public void EnsureNeededFiles()
  {
    if (!File.Exists(_configs.PathToNoteSource))
    {
      SaveAllSync(Enumerable.Empty<NoteModel>());
    }
  }

  public async Task SaveAll(IEnumerable<NoteModel> toSave)
  {
    string toWrite = GetSerialized(toSave);
    await File.WriteAllTextAsync(_configs.PathToNoteSource, toWrite);
    _logger.LogInfo($"Save notes at {_configs.PathToNoteSource}");
  }

  public void SaveAllSync(IEnumerable<NoteModel> toSave)
  {
    string toWrite = GetSerialized(toSave);
    File.WriteAllText(_configs.PathToNoteSource, toWrite);
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

namespace NoteCollectionEditor.Models;

public partial class NoteModel
{
  public string? Title { get; set; }
  public string? Content { get; set; }

  public int Id { get; set; } = -1;
}

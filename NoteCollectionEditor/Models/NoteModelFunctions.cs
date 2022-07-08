namespace NoteCollectionEditor.Models;

public partial class NoteModel
{
  public override string ToString()
  {
    return $"Title: {Title}, Content: {Content}";
  }
}
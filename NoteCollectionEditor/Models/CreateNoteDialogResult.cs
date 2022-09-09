namespace NoteCollectionEditor.Models;

public record CreateNoteDialogResult(string NewTitle, string NewContent, bool InsertOnTop);

public static class CreateNoteDialogResultExtension
{
  public static NoteModel ToModel(this CreateNoteDialogResult toConvert)
  {
    return new NoteModel
    {
      Title = toConvert.NewTitle,
      Content = toConvert.NewContent
    };
  }
}

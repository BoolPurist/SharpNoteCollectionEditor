namespace NoteCollectionEditor.Models;

/// <summary>
/// Contains representation of a note. This note is either to be created or changed via the 
/// these provided information. The caller is responsible to know if the note is to be 
/// created or changed. 
/// </summary>
public record ChangeNoteDialogResult(string NewTitle, string NewContent, bool InsertOnTop);

public static class CreateNoteDialogResultExtension
{
    public static NoteModel ToModel(this ChangeNoteDialogResult toConvert)
    {
        return new NoteModel
        {
            Title = toConvert.NewTitle,
            Content = toConvert.NewContent
        };
    }
}

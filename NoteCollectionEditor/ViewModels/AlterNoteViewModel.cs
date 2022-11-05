using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Metadata;
using NoteCollectionEditor.Models;
using ReactiveUI;


namespace NoteCollectionEditor.ViewModels;

public class AlterNoteViewModel : ViewModelBase
{
    public event Action? Submit;

    [DependsOn(nameof(FieldsAreValid))]
    public bool CanSubmitCommand()
    {
        return FieldsAreValid;
    }

    public void SubmitCommand()
    {
        Submit?.Invoke();
    }

    public string AcceptButtonText
    {
        get => _acceptButtonText;
        set => this.RaiseAndSetIfChanged(ref _acceptButtonText, value);
    }

    public string CancelButtonText
    {
        get => _cancelButtonText;
        set => this.RaiseAndSetIfChanged(ref _cancelButtonText, value);
    }

    public string NewTitle
    {
        get => _newTitle;
        set
        {
            this.RaiseAndSetIfChanged(ref _newTitle, value);
            this.RaisePropertyChanged(nameof(FieldsAreValid));
        }
    }

    public bool FieldsAreValid => !string.IsNullOrWhiteSpace(_newTitle) &&
                                  !string.IsNullOrWhiteSpace(_newContent);


    public bool InsertOnTop
    {
        get => _insertOnTop;
        set
        {
            Console.WriteLine($"Setter: {nameof(InsertOnTop)}");
            this.RaiseAndSetIfChanged(ref _insertOnTop, value);
        }
    }

    public string NewContent
    {
        get => _newContent;
        set
        {
            this.RaiseAndSetIfChanged(ref _newContent, value);
            this.RaisePropertyChanged(nameof(FieldsAreValid));
        }
    }

    private string _newTitle;
    private string _newContent;
    private string _acceptButtonText = "Accept";
    private string _cancelButtonText = "Cancel";
    private bool _insertOnTop = true;

    public AlterNoteViewModel() : this(String.Empty, String.Empty)
    {
    }

    public AlterNoteViewModel(string title, string textBody)
    {
        _newTitle = title;
        _newContent = textBody;
    }

    public override string ToString()
    {
        return $"NewTitle: {NewTitle}, NewContent {NewContent}";
    }
}

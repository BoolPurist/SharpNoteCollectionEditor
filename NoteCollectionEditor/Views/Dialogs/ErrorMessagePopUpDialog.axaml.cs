using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace NoteCollectionEditor.Views.Dialogs;

public partial class ErrorMessagePopUpDialog : Window
{
  public static readonly StyledProperty<string> ErrorTextProperty =
    AvaloniaProperty.Register<AskPopUpDialog, string>(nameof(ErrorText), defaultValue: "Some error has occured !");

  public string ErrorText
  {
    get => GetValue(ErrorTextProperty);
    set => SetValue(ErrorTextProperty, value);
  }

  public ErrorMessagePopUpDialog()
  {
    InitializeComponent();
#if DEBUG
    this.AttachDevTools();
#endif
  }

  private void InitializeComponent()
  {
    AvaloniaXamlLoader.Load(this);
  }

  public void CommandClose()
  {
    Close(null);
  }
}


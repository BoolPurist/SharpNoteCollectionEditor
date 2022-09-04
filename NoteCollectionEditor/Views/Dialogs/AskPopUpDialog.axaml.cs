using System.ComponentModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace NoteCollectionEditor.Views.Dialogs;

/// <summary>
/// Little dialog box to warn the user and ask him/her if she/he wants to continue anyway.
/// Will return a boolean value. True if user clicked the accept button otherwise false
/// </summary>
public partial class AskPopUpDialog : Window
{
  public static readonly StyledProperty<string> WarningTextProperty =
    AvaloniaProperty.Register<AskPopUpDialog, string>(nameof(WarningText), defaultValue: "Attention ! Proceed with caution. Do you want to continue  ?");

  public static readonly StyledProperty<string> ConfirmDespiteWarningTextProperty =
    AvaloniaProperty.Register<AskPopUpDialog, string>(nameof(ConfirmDespiteWarningText), defaultValue: "Continue despite this warning");

  public string WarningText
  {
    get => GetValue(WarningTextProperty);
    set => SetValue(WarningTextProperty, value);
  }

  public string ConfirmDespiteWarningText
  {
    get => GetValue(ConfirmDespiteWarningTextProperty);
    set => SetValue(ConfirmDespiteWarningTextProperty, value);
  }

  public AskPopUpDialog()
  {
    InitializeComponent();
#if DEBUG
    this.AttachDevTools();
#endif
  }

  public void CommandCloseWithConfirmation()
  {
    Close(true);
  }

  public void CommandCloseWithCancel()
  {
    Close(false);
  }

  private void InitializeComponent()
  {
    AvaloniaXamlLoader.Load(this);
  }
}

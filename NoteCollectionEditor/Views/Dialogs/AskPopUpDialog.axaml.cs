using System;
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
  public static readonly DirectProperty<AskPopUpDialog, string> WarningTextProperty =
    AvaloniaProperty.RegisterDirect<AskPopUpDialog, string>(
      nameof(WarningText),
      text => text.WarningText,
      (text, value) => text.WarningText = value
      );

  public static readonly DirectProperty<AskPopUpDialog, string> ConfirmDespiteWarningTextProperty =
    AvaloniaProperty.RegisterDirect<AskPopUpDialog, string>(
      nameof(WarningText),
      text => text.WarningText,
      (text, value) => text.WarningText = value
    );

  private string _warningText = "Attention ! Proceed with caution. Do you want to continue  ?";

  public string WarningText
  {
    get => _warningText;
    set => SetAndRaise(WarningTextProperty, ref _warningText, value);
  }

  private string _confirmDespiteWarningText = "Okay";

  public string ConfirmDespiteWarningText
  {
    get => _confirmDespiteWarningText;
    set => SetAndRaise(ConfirmDespiteWarningTextProperty,ref _confirmDespiteWarningText, value);
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

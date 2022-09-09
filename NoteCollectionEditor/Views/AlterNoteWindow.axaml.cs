using System;
using System.Diagnostics;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using NoteCollectionEditor.Extensions;
using NoteCollectionEditor.Models;
using NoteCollectionEditor.Services;
using NoteCollectionEditor.ViewModels;

namespace NoteCollectionEditor.Views;

public partial class AlterNoteWindow : Window
{
  public AlterNoteViewModel Data { get; set; }

  public static readonly DirectProperty<AlterNoteWindow, bool> SpawnWithInsertTopOptionProperty =
    AvaloniaProperty.RegisterDirect<AlterNoteWindow, bool>(
      nameof(SpawnWithInsertTopOption),
      win => win.SpawnWithInsertTopOption,
      (win, value) => win.SpawnWithInsertTopOption = value
    );

  private bool _spawnWithInsertTopOption;

  public bool SpawnWithInsertTopOption
  {
    get => _spawnWithInsertTopOption;
    set => SetAndRaise(SpawnWithInsertTopOptionProperty,ref _spawnWithInsertTopOption, value);
  }

  public static AlterNoteWindow CreateForEdit(NoteModel modelToEdit)
  {
    var editorDialog = new AlterNoteWindow(
      modelToEdit.Title ?? String.Empty,
      modelToEdit.Content ?? String.Empty
    );
    editorDialog.SetAcceptButtonText("Confirm Edit");
    return editorDialog;
  }

  public AlterNoteWindow() : this(String.Empty, String.Empty)
  {
  }

  private AlterNoteWindow(string startTitle, string startContent)
  {
    InitializeComponent();

    Data = ServicesOfApp.Resolver.GetRequiredService<AlterNoteViewModel>();
    DataContext = this;
    Data.Submit += CommandSubmitAndClose;
    Data.NewContent = startContent;
    Data.NewTitle = startTitle;

    InitializeForDesign();

#if DEBUG
    this.AttachDevTools();
#endif
  }

  private void InitializeForDesign()
  {
    if (!Design.IsDesignMode) return;

    Data.NewTitle = "New Title";
    Data.NewContent = "...";
    SpawnWithInsertTopOption = true;
  }

  public void SetAcceptButtonText(string newText) => Data.AcceptButtonText = newText;

  // ReSharper disable once UnusedParameter.Local
  protected void OnClickCancel(object? sender, RoutedEventArgs e)
  {
    // Prevents freezing if cancel is pressed in ide preview design mode.
    if (Design.IsDesignMode) return;
    Close(null);
  }

  private void CommandSubmitAndClose()
  {
    Close(new CreateNoteDialogResult(Data.NewTitle, Data.NewContent, Data.InsertOnTop));
  }

  private void InitializeComponent()
  {
    AvaloniaXamlLoader.Load(this);
  }

  protected override void OnClosed(EventArgs e)
  {
    base.OnClosed(e);
    Data.Submit -= CommandSubmitAndClose;
  }
}

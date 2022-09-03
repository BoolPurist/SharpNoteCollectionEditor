using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using DynamicData.Binding;

using NoteCollectionEditor.Extensions;
using NoteCollectionEditor.Models;
using NoteCollectionEditor.Services;
using NoteCollectionEditor.ViewModels;
using Splat;

namespace NoteCollectionEditor.Views;


public partial class NoteListView : UserControl
{
  public NoteListViewModel Data { get; }

  private readonly ILogger _logger;

  public NoteListVisualBindings VisualData { get; private set; }

  public NoteListView()
  {
    _logger = ServicesOfApp.Resolver.GetRequiredService<ILogger>();
    Data = ServicesOfApp.Resolver.GetRequiredService<NoteListViewModel>();
    VisualData = new NoteListVisualBindings();
    InitializeForDesign();
    InitializeComponent();
  }

  private void InitializeForDesign()
  {
    if (!Design.IsDesignMode) return;

    Data.ErrorInLoading = true;
    Data.IsLoading = false;
    Data.Notes = new ObservableCollectionExtended<NoteModel>(
      new[]
      {
        new NoteModel {Title = "XXX", Content = new string('y', 200)},
        new NoteModel {Title = "xxxx", Content = "asdfsdf"}
      }
    );
  }



  private void InitializeComponent()
  {
    AvaloniaXamlLoader.Load(this);
  }

  private async void OnClick_EditNode(object? sender, RoutedEventArgs e)
  {
    if (sender is Button {Tag: int} button )
    {
      var mainWindow = ApplicationExtension.GetCurrentMainWindow();
      if (mainWindow == null)
      {
        _logger.LogError($"{nameof(OnClick_EditNode)}: Could not retrieve window of a note list user control.");
        return;
      }

      int editId = (int) button.Tag;
      var toEdit = Data.Notes[editId];

      var edited = await AlterNoteWindow.CreateForEdit(toEdit)
        .ShowDialog<NoteModel>(mainWindow);
      edited.Id = editId;

      Data.EditNoteCommand.Execute(edited);
    }
    else
    {
      _logger.LogError($"{nameof(OnClick_EditNode)}: sender is not of type button with tag property of type int.");
    }
  }


}

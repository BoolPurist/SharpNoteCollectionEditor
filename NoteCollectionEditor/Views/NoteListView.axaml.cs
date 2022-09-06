using System.Threading.Tasks;
using Avalonia.Controls;
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
    Data.SetNoteCollection(new[]
    {
      new NoteModel {Title = "XXX", Content = new string('y', 200)},
      new NoteModel {Title = "Xth Title", Content = "Some Content"}
    });
  }


  private void InitializeComponent()
  {
    AvaloniaXamlLoader.Load(this);
  }


  // ReSharper disable once UnusedMember.Local
  private async Task CommandSpawnDialogEditNode(int idForDelete)
  {
    var mainWindow = ApplicationExtension.GetCurrentMainWindow();

    if (mainWindow == null)
    {
      _logger.LogError($"{nameof(CommandSpawnDialogEditNode)}: Could not retrieve window of a note list user control.");
      return;
    }

    var toEdit = Data.Notes[idForDelete];


    var edited = await AlterNoteWindow.CreateForEdit(toEdit)
      .ShowDialog<NoteModel>(mainWindow);

    if (edited == null) return;

    edited.Id = idForDelete;
    Data.CommandEditNote(edited);
  }

  // ReSharper disable once UnusedMember.Local
  private void CommandSpawnDialogDeleteNode(int idForDelete)
  {
    Data.CommandDeleteNote(idForDelete);
  }

}


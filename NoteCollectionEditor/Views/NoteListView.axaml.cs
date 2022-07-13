using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using DynamicData.Binding;
using NoteCollectionEditor.Extensions;
using NoteCollectionEditor.Models;
using NoteCollectionEditor.Services;
using NoteCollectionEditor.ViewModels;
using ReactiveUI;

namespace NoteCollectionEditor.Views;



public partial class NoteListView : UserControl
{
  
  public NoteListViewModel Data { get; }

  public NoteListVisualBindings VisualData { get; private set; }
  public NoteListView()
  {
    Data = ServicesOfApp.Resolver.GetRequiredService<NoteListViewModel>();
    VisualData = new NoteListVisualBindings();
    InitializeForDesign();
    InitializeComponent();
  }

  private void InitializeForDesign()
  {
    if (Design.IsDesignMode)
    {
      Data.ErrorInLoading = true;
      Data.IsLoading = false;
      Data.Notes = new ObservableCollectionExtended<NoteModel>(
        new[]
        {
          new NoteModel {Title = "XXX", Content = new string('y', 200)},
          new NoteModel() {Title = "xxxx", Content = "asdfsdf"}
        }
      );
    }
  }

  private void InitializeComponent()
  {
    AvaloniaXamlLoader.Load(this);
  }
  
}
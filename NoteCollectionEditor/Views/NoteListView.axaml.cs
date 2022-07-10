using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reactive.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using NoteCollectionEditor.Extensions;
using NoteCollectionEditor.Models;
using NoteCollectionEditor.Services;
using NoteCollectionEditor.ViewModels;

namespace NoteCollectionEditor.Views;

public partial class NoteListView : UserControl 
{
  
  public NoteListViewModel Data { get; private set; }

  public NoteListVisualBindings VisualData { get; private set; }
  public NoteListView()
  {
    InitializeComponent();
    Data = ServicesOfApp.Resolver.GetRequiredService<NoteListViewModel>();
    VisualData = new NoteListVisualBindings();
    DataContext = this;
  }
  
  private void InitializeComponent()
  {
    AvaloniaXamlLoader.Load(this);
  }
  
}
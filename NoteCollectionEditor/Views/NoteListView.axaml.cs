using System;
using System.Collections.ObjectModel;
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
  public NoteListViewModel Data { get; set; }

  public NoteListView()
  {
    InitializeComponent();
    Data = ServicesOfApp.Resolver.GetRequiredService<NoteListViewModel>();
    DataContext = Data;
  }
  
  private void InitializeComponent()
  {
    AvaloniaXamlLoader.Load(this);
  }

  private void Button_OnClick(object? sender, RoutedEventArgs e)
  {
    Console.WriteLine($"Clicked");
  }
}
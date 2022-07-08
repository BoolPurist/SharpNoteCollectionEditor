using System;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Metadata;
using NoteCollectionEditor.Extensions;
using NoteCollectionEditor.Models;
using NoteCollectionEditor.Services;
using NoteCollectionEditor.ViewModels;
using ReactiveUI;

namespace NoteCollectionEditor.Views;

public partial class AddNoteView : Window
{
  public AddNoteViewModel Data { get; set; }

  public AddNoteView()
  {
    InitializeComponent();
    
    Data = ServicesOfApp.Resolver.GetRequiredService<AddNoteViewModel>();
    DataContext = this;
    Data.Submit += OnSubmit;

#if DEBUG
    this.AttachDevTools();
#endif
    
  }

  private void OnSubmit(object? sender, NoteModel toSubmit)
  {
    Close(toSubmit);
  }

  private void OnClickCancel(object? sender, RoutedEventArgs e)
  {
    Close(null);
  }

  private void InitializeComponent()
  {
    AvaloniaXamlLoader.Load(this);
  }

}
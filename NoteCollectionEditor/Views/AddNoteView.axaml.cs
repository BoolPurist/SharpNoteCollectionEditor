using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using NoteCollectionEditor.Extensions;
using NoteCollectionEditor.Models;
using NoteCollectionEditor.Services;
using NoteCollectionEditor.ViewModels;

namespace NoteCollectionEditor.Views;

public partial class AddNoteView : Window
{
  private AddNoteViewModel Data { get; set; }

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

  // ReSharper disable once UnusedParameter.Local
  private void OnClickCancel(object? sender, RoutedEventArgs e)
  {
    Close(null);
  }

  private void InitializeComponent()
  {
    AvaloniaXamlLoader.Load(this);
  }

  protected override void OnClosed(EventArgs e)
  {
    base.OnClosed(e);
    Data.Submit -= OnSubmit;
  }

}

using System;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Metadata;
using NoteCollectionEditor.Models;
using NoteCollectionEditor.ViewModels;
using ReactiveUI;

namespace NoteCollectionEditor.Views;

public partial class AddNoteView : Window
{
  public AddNoteViewModel Data { get; set; }

  public AddNoteView()
  {
    InitializeComponent();
    
    SetupContextAndEvents();

#if DEBUG
    this.AttachDevTools();
#endif

    void SetupContextAndEvents()
    {
      Data = new AddNoteViewModel();
      DataContext = this;
      ApplyNewNoteButton = this.FindControl<Button>(nameof(ApplyNewNoteButton));
      ToggleApplyButton(null, false);
      Data.AbilityToSubmitHasChanged += ToggleApplyButton;
    }
  }
  
  

  private void InitializeComponent()
  {
    AvaloniaXamlLoader.Load(this);
  }

  private void ToggleApplyButton(object? sender, bool newToggleValue)
  {
    if (ApplyNewNoteButton == null)
    {
      Console.WriteLine("asdfasdf");
    }
    else
    {
      ApplyNewNoteButton.IsEnabled = newToggleValue;
      Console.WriteLine($"AddNewButton.IsEnabled: {ApplyNewNoteButton.IsEnabled}");
    }


  }

  public void OnClickCreateNewNote(object? parameter, RoutedEventArgs routedEventArgs)
  {
    var newNote = new NoteModel()
    {
      Title = Data.NewTitle,
      Content = Data.NewContent
    };
    
    Close(newNote);
  }
  private void OnClickCancel(object? sender, RoutedEventArgs e)
  {
    Close(null);
  }
}
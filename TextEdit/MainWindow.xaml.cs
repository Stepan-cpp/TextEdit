using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;

namespace TextEdit
{
    public partial class MainWindow : Window
    {
        int indent = 4;

        private string filePath = string.Empty;
        public string FilePath {
            get
            {
                return filePath;
            }

            set {
                filePath = value;
                fileNameHeader.Text = value;
            }
        }
        private bool isSaved = false;
        public bool IsSaved
        {
            get
            {
                return isSaved;
            }
            set
            {
                isSaved = value;
                fileNameHeader.Text = (FilePath + (value?"":"*"));
            }
        }

        public MainWindow()
        {
            ApplicationCommands.Redo.InputGestures.Clear();
            ApplicationCommands.Redo.InputGestures.Add(new KeyGesture(Key.Z, ModifierKeys.Control | ModifierKeys.Shift));
            ThemeChange("DarkTheme");
            InitializeComponent();
            StateChanged += MainWindowStateChangeRaised;
            Closing += MainWindow_Closing;
        }

        private void MainWindowStateChangeRaised(object sender, EventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                RestoreButton.Visibility = Visibility.Visible;
                MaximizeButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                RestoreButton.Visibility = Visibility.Collapsed;
                MaximizeButton.Visibility = Visibility.Visible;
            }
        }

        private void ThemeChange(string theme)
        {
            var uri = new Uri(theme + ".xaml", UriKind.Relative);
            ResourceDictionary resourceDict = Application.LoadComponent(uri) as ResourceDictionary;
            App.Current.Resources.MergedDictionaries.Add(resourceDict);
        }

        public void NewFile(object sender, ExecutedRoutedEventArgs args)
        {
            FilePath = String.Empty;
            textBlock.Document = new FlowDocument();
        }

        public void OpenFile(object sender, ExecutedRoutedEventArgs args)
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "Text file|*.txt|All files|*.*";
            dialog.Multiselect = false;
            var result = dialog.ShowDialog();
            if (result.HasValue && result.Value)
            {
                FilePath = dialog.FileName;
                textBlock.Document = new FlowDocument(new Paragraph(new Run(File.ReadAllText(FilePath))));
            }
        }

        public void SaveFile(object sender, ExecutedRoutedEventArgs args)
        {
            if (FilePath.Length == 0)
                SaveFileAs(sender, args);
            else
            {
                File.WriteAllText(FilePath, new TextRange(textBlock.Document.ContentStart, textBlock.Document.ContentEnd).Text);
                IsSaved = true;
            }
        }

        public void SaveFileAs(object sender, ExecutedRoutedEventArgs args)
        {
            var dialog = new SaveFileDialog();
            dialog.Filter = "Text file|*.txt|All files|*.*";
            var result = dialog.ShowDialog();
            if (result.HasValue && result.Value)
            {
                FilePath = dialog.FileName;
                SaveFile(sender, args);
            }
        }

        public void Exit(object sender, ExecutedRoutedEventArgs args)
        {
            if (ShowSaveDialog())
                Close();
        }

        public void Duplicate(object sender, ExecutedRoutedEventArgs args)
        {
            if (textBlock.Selection.Text.Length == 0)
            {
                var start = textBlock.CaretPosition.GetLineStartPosition(0).GetPositionAtOffset(1);
                var text = start.GetTextInRun(LogicalDirection.Forward);
                start.InsertTextInRun(text);
                start.InsertLineBreak();
            }
            else
            {
                var start = textBlock.Selection.Start;
                var text = textBlock.Selection.Text;
                start.InsertTextInRun(text);
            }
        }

        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            if (!ShowSaveDialog())
                e.Cancel = true;
        }

        public bool ShowSaveDialog()
        {
            if (!IsSaved)
            {
                SaveWindow window = new SaveWindow();
                
                window.Owner = this;
                var success = window.ShowDialog();
                if (success.HasValue && success.Value)
                {
                    if (window.DoSave)
                    {
                        SaveFile(default, default);
                    }
                    return true;
                }
                else
                    return false;
            }
            return true;
        }

        private void TitleBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void DataGrid_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control)
            {
                e.Handled = true;
                textBlock.FontSize = Math.Clamp(textBlock.FontSize + e.Delta / 15, 7, 50);
            }
        }

        private void TextChanged(object sender, TextChangedEventArgs e)
        {
            IsSaved = false;
        }

        private void TextPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string text = textBlock.CaretPosition.GetTextInRun(LogicalDirection.Backward);

                int count = 0;
                while (count < text.Length && text[count] == ' ')
                    count++;

                textBlock.CaretPosition = textBlock.CaretPosition.InsertLineBreak();
                InsertAndMove(new string(' ', count));
                textBlock.Focus();
                e.Handled = true;
            }
            if (e.Key == Key.Tab)
            {
                InsertAndMove(new string(' ', indent));
                textBlock.Focus();
                e.Handled = true;
            }
            if (e.Key == Key.Back)
            {
                string text = textBlock.CaretPosition.GetTextInRun(LogicalDirection.Backward);
                if (1 < text.Length && text.All(c => c == ' '))
                {
                    textBlock.CaretPosition = textBlock.CaretPosition.GetPositionAtOffset(-indent) ?? textBlock.Document.ContentStart;
                    textBlock.CaretPosition.DeleteTextInRun(indent);
                    textBlock.Focus();
                    e.Handled = true;
                }
            }
        }

        private TextPointer InsertAndMove(string text, TextPointer ptr)
        {
            int start = textBlock.Document.ContentStart.GetOffsetToPosition(textBlock.CaretPosition);
            ptr.InsertTextInRun(text);
            return textBlock.Document.ContentStart.GetPositionAtOffset(start + text.Length);
        }

        private void InsertAndMove(string text)
        {
            textBlock.CaretPosition = InsertAndMove(text, textBlock.CaretPosition);
        }
    }
}

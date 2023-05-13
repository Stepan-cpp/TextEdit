using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
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
        double opacity = 0.6;

        public static string FileToOpen = "";

        private const string HelpFilePath = "manual.txt";

        private SearchAndReplace sar;

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
            SizeChanged += MainWindow_SizeChanged;
            LocationChanged += MainWindow_LocationChanged;
            Loaded += MainWindow_Loaded;
            if (FileToOpen.Length > 0)
            {
                FilePath = FileToOpen;
                Reload(default, default);
            }
            sar = new SearchAndReplace();
            UpdatePosition();
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            sar.Owner = this;
        }

        void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdatePosition();
        }

        private void UpdatePosition()
        {
            sar.Left = Left + Width - 170;
            sar.Top = Top + 30;
        }

        void MainWindow_LocationChanged(object sender, EventArgs e)
        {
            UpdatePosition();
        }

        private void Expander_Expanded(object sender, RoutedEventArgs e)
        {
            sar.Show();
        }

        private void SarExp_Collapsed(object sender, RoutedEventArgs e)
        {
            sar.Hide();
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
            IsSaved = false;
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
                Reload(sender, args);
            }
        }

        public void Reload(object sender, ExecutedRoutedEventArgs args)
        {
            if (filePath.Length>0)
            {
                textBlock.Document = new FlowDocument(new Paragraph(new Run(File.ReadAllText(FilePath))));
                IsSaved = true;
            }
        }

        public void Find(object sender, ExecutedRoutedEventArgs args)
        {
            SarExp.IsExpanded = true;
        }

        public void Replace(object sender, ExecutedRoutedEventArgs args)
        {
            SarExp.IsExpanded = true;
        }

        private bool simplified = false;

        public void Simplify(object sender, ExecutedRoutedEventArgs args)
        {
            simplified = true;
            Opacity = opacity;
            AppBarRow.Height = new GridLength(0);
            AppBar.Visibility = Visibility.Collapsed;
            textBlock.Margin = new Thickness(0);
        }

        public void Unsimplify(object sender, ExecutedRoutedEventArgs args)
        {
            simplified = false;
            Opacity = 1;
            AppBarRow.Height = new GridLength(30);
            AppBar.Visibility = Visibility.Visible;
            textBlock.Margin = new Thickness(10);
        }

        public void OpUp(object sender, ExecutedRoutedEventArgs args)
        {
            opacity = Math.Min(opacity + 0.1, 1);
            if (simplified)
                Opacity = opacity;
        }

        public void OpDown(object sender, ExecutedRoutedEventArgs args)
        {
            opacity = Math.Max(opacity - 0.1, 0.1);
            if (simplified)
                Opacity = opacity;
        }

        public void PinWindow(object sender, ExecutedRoutedEventArgs args)
        {
            Topmost = true;
            PinIndicator.Visibility = Visibility.Visible;
        }

        public void UnpinWindow(object sender, ExecutedRoutedEventArgs args)
        {
            Topmost = false;
            PinIndicator.Visibility = Visibility.Collapsed;
        }

        public void Find(string regex, bool caseSensitive)
        {
            Regex reg = new Regex(regex, caseSensitive?RegexOptions.None:RegexOptions.IgnoreCase);
            var doc = new TextRange(textBlock.CaretPosition, textBlock.Document.ContentEnd);
            var match = reg.Match(doc.Text);
            textBlock.CaretPosition = textBlock.CaretPosition.GetPositionAtOffset(match.Index + match.Length);
            textBlock.Focus();
        }

        public void ReplaceNext(string regex, string with, bool caseSensitive)
        {
            Regex reg = new Regex(regex, caseSensitive?RegexOptions.None:RegexOptions.IgnoreCase);
            var doc = new TextRange(textBlock.CaretPosition, textBlock.Document.ContentEnd);
            var match = reg.Match(doc.Text);
            textBlock.CaretPosition = textBlock.CaretPosition.GetPositionAtOffset(match.Index);
            textBlock.CaretPosition.DeleteTextInRun(match.Length);
            textBlock.CaretPosition.InsertTextInRun(with);
            textBlock.Focus();
        }

        public void ReplaceAll(string regex, string with, bool caseSensitive)
        {
            Regex reg = new Regex(regex, caseSensitive ? RegexOptions.None : RegexOptions.IgnoreCase);
            Match match;
            var doc = new TextRange(textBlock.CaretPosition, textBlock.Document.ContentEnd);
            while ((match = reg.Match(doc.Text)).Success)
            {
                textBlock.CaretPosition = textBlock.CaretPosition.GetPositionAtOffset(match.Index);
                textBlock.CaretPosition.DeleteTextInRun(match.Length);
                textBlock.CaretPosition.InsertTextInRun(with);
                doc = new TextRange(textBlock.CaretPosition, textBlock.Document.ContentEnd);
            }
            textBlock.Focus();
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
                textBlock.FontSize = Math.Clamp(textBlock.FontSize + 4*Math.Sign(e.Delta), 6, 78);
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

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void RestoreButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Normal;
        }

        private void MaximizeButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Maximized;
        }

        private void MinimizeWindow(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Help(object sender, ExecutedRoutedEventArgs e)
        {
            textBlock.Document = new FlowDocument(new Paragraph(new Run(File.ReadAllText(HelpFilePath))));
            isSaved = true;
        }

        private void MoveRight(object sender, ExecutedRoutedEventArgs e)
        {
            Left = Math.Min(Left + (SystemParameters.WorkArea.Width - Width) / 20, SystemParameters.WorkArea.Right - Width);
        }
        private void MoveDown(object sender, ExecutedRoutedEventArgs e)
        {
            Top = Math.Min(Top + (SystemParameters.WorkArea.Height - Height) / 20, SystemParameters.WorkArea.Bottom - Height);
        }
        private void MoveLeft(object sender, ExecutedRoutedEventArgs e)
        {
            Left = Math.Max(Left - (SystemParameters.WorkArea.Width - Width) / 20, 0);
        }
        private void MoveUp(object sender, ExecutedRoutedEventArgs e)
        {
            Top = Math.Max(Top - (SystemParameters.WorkArea.Height - Height) / 20, 0);
        }

        private void PushLeft(object sender, ExecutedRoutedEventArgs e)
        {
            Left = 0;
        }
        private void PushUp(object sender, ExecutedRoutedEventArgs e)
        {
            Top = 0;
        }
        private void PushRight(object sender, ExecutedRoutedEventArgs e)
        {
            Left = SystemParameters.WorkArea.Right - Width;
        }
        private void PushDown(object sender, ExecutedRoutedEventArgs e)
        {
            Top = SystemParameters.WorkArea.Bottom - Height;
        }
    }
}

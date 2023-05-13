using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TextEdit
{
    internal static class Commands
    {
        static Commands()
        {
            New = new RoutedCommand("New", typeof(MainWindow));
            Open = new RoutedCommand("Open", typeof(MainWindow));
            Save = new RoutedCommand("Save", typeof(MainWindow));
            SaveAs = new RoutedCommand("Save as", typeof(MainWindow));
            Exit = new RoutedCommand("Exit", typeof(MainWindow));
            Find = new RoutedCommand("Find", typeof(MainWindow));
            Replace = new RoutedCommand("Replace", typeof(MainWindow));
            Duplicate = new RoutedCommand("Duplicate", typeof(MainWindow));

            New.InputGestures.Add(new KeyGesture(Key.N, ModifierKeys.Control));
            Save.InputGestures.Add(new KeyGesture(Key.S, ModifierKeys.Control));
            SaveAs.InputGestures.Add(new KeyGesture(Key.S, ModifierKeys.Control | ModifierKeys.Shift));
            Exit.InputGestures.Add(new KeyGesture(Key.F4, ModifierKeys.Alt));

            Find.InputGestures.Add(new KeyGesture(Key.F, ModifierKeys.Control));
            Replace.InputGestures.Add(new KeyGesture(Key.H, ModifierKeys.Control));
            Duplicate.InputGestures.Add(new KeyGesture(Key.D, ModifierKeys.Control));
        }
        public static RoutedCommand New { get; set; }
        public static RoutedCommand Open { get; set; }
        public static RoutedCommand Save { get; set; }
        public static RoutedCommand SaveAs { get; set; }
        public static RoutedCommand Exit { get; set; }
        public static RoutedCommand Find { get; set; }
        public static RoutedCommand Replace { get; set; }
        public static RoutedCommand Duplicate { get; set; }
    }
}

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
        public static RoutedCommand New { get; set; } = new RoutedCommand("New", typeof(MainWindow));
        public static RoutedCommand Open { get; set; } = new RoutedCommand("Open", typeof(MainWindow));
        public static RoutedCommand Save { get; set; } = new RoutedCommand("Save", typeof(MainWindow));
        public static RoutedCommand SaveAs { get; set; } = new RoutedCommand("Save as", typeof(MainWindow));
        public static RoutedCommand Reload { get; set; } = new RoutedCommand("Reload", typeof(MainWindow));
        public static RoutedCommand Help { get; set; } = new RoutedCommand("Help", typeof(MainWindow));
        public static RoutedCommand Exit { get; set; } = new RoutedCommand("Exit", typeof(MainWindow));
        public static RoutedCommand Find { get; set; } = new RoutedCommand("Find", typeof(MainWindow));
        public static RoutedCommand Replace { get; set; } = new RoutedCommand("Replace", typeof(MainWindow));
        public static RoutedCommand Duplicate { get; set; } = new RoutedCommand("Duplicate", typeof(MainWindow));
        public static RoutedCommand PinWindow { get; set; } = new RoutedCommand("Pin window", typeof(MainWindow));
        public static RoutedCommand UnpinWindow { get; set; } = new RoutedCommand("Unpin window", typeof(MainWindow));
        public static RoutedCommand Simplify { get; set; } = new RoutedCommand("Simplify", typeof(MainWindow));
        public static RoutedCommand Unsimplify { get; set; } = new RoutedCommand("Unsimplify", typeof(MainWindow));
        public static RoutedCommand OpacityUp { get; set; } = new RoutedCommand("Opacity up", typeof(MainWindow));
        public static RoutedCommand OpacityDown { get; set; } = new RoutedCommand("Opacity down", typeof(MainWindow));

        public static RoutedCommand PushLeft { get; set; } = new RoutedCommand("Push left", typeof(MainWindow));
        public static RoutedCommand PushUp { get; set; } = new RoutedCommand("Push up", typeof(MainWindow));
        public static RoutedCommand PushRight { get; set; } = new RoutedCommand("Push right", typeof(MainWindow));
        public static RoutedCommand PushDown { get; set; } = new RoutedCommand("Push down", typeof(MainWindow));
        public static RoutedCommand MoveLeft { get; set; } = new RoutedCommand("Move left", typeof(MainWindow));
        public static RoutedCommand MoveUp { get; set; } = new RoutedCommand("Move up", typeof(MainWindow));
        public static RoutedCommand MoveRight { get; set; } = new RoutedCommand("Move right", typeof(MainWindow));
        public static RoutedCommand MoveDown { get; set; } = new RoutedCommand("Move down", typeof(MainWindow));

        static Commands()
        {
            New.InputGestures.Add(new KeyGesture(Key.N, ModifierKeys.Control));
            Save.InputGestures.Add(new KeyGesture(Key.S, ModifierKeys.Control));
            SaveAs.InputGestures.Add(new KeyGesture(Key.S, ModifierKeys.Control | ModifierKeys.Shift));
            Exit.InputGestures.Add(new KeyGesture(Key.F4, ModifierKeys.Alt));
            Reload.InputGestures.Add(new KeyGesture(Key.R, ModifierKeys.Control));

            Find.InputGestures.Add(new KeyGesture(Key.F, ModifierKeys.Control));
            Replace.InputGestures.Add(new KeyGesture(Key.H, ModifierKeys.Control));
            Duplicate.InputGestures.Add(new KeyGesture(Key.D, ModifierKeys.Control));

            Simplify.InputGestures.Add(new KeyGesture(Key.Escape, ModifierKeys.Shift));
            Unsimplify.InputGestures.Add(new KeyGesture(Key.Escape));
            OpacityUp.InputGestures.Add(new KeyGesture(Key.OemPlus, ModifierKeys.Alt));
            OpacityDown.InputGestures.Add(new KeyGesture(Key.OemMinus, ModifierKeys.Alt));

            MoveLeft.InputGestures.Add(new KeyGesture(Key.Left, ModifierKeys.Alt));
            MoveUp.InputGestures.Add(new KeyGesture(Key.Up, ModifierKeys.Alt));
            MoveRight.InputGestures.Add(new KeyGesture(Key.Right, ModifierKeys.Alt));
            MoveDown.InputGestures.Add(new KeyGesture(Key.Down, ModifierKeys.Alt));

            PushLeft.InputGestures.Add(new KeyGesture(Key.Left, ModifierKeys.Alt | ModifierKeys.Shift));
            PushUp.InputGestures.Add(new KeyGesture(Key.Up, ModifierKeys.Alt | ModifierKeys.Shift));
            PushRight.InputGestures.Add(new KeyGesture(Key.Right, ModifierKeys.Alt | ModifierKeys.Shift));
            PushDown.InputGestures.Add(new KeyGesture(Key.Down, ModifierKeys.Alt | ModifierKeys.Shift));
        }
    }
}

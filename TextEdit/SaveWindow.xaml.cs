using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TextEdit
{
    public partial class SaveWindow : Window
    {
        public bool DoSave { get; private set; }

        public SaveWindow()
        {
            InitializeComponent();
        }

        private void Do_Click(object sender, RoutedEventArgs e)
        {
            DoSave = true;
            this.DialogResult = true;
        }

        private void Donot_Click(object sender, RoutedEventArgs e)
        {
            DoSave = false;
            this.DialogResult = true;
        }
    }
}

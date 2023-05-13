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
    /// <summary>
    /// Логика взаимодействия для SearchAndReplace.xaml
    /// </summary>
    public partial class SearchAndReplace : Window
    {
        public SearchAndReplace()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Owner).SarExp.IsExpanded = false;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Owner).Find(search.Text, caseSensitive.IsChecked.Value);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Owner).ReplaceNext(search.Text, replace.Text, caseSensitive.IsChecked.Value);
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Owner).ReplaceAll(search.Text, replace.Text, caseSensitive.IsChecked.Value);
        }
    }
}

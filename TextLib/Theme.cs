using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;

namespace TextLib
{
    [ContentProperty("Resources")]
    public class Theme
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                PropertyChange();
            }
        }

        private string _author;
        public string Author
        {
            get => _author;
            set
            {
                _author = value;
                PropertyChange();
            }
        }
        public ResourceDictionary Resources { get; set; }

        public Theme()
        {
            Name = "Unnamed";
            Author = "Default";
        }

        private void PropertyChange([CallerMemberName] string property = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}

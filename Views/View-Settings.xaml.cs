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

namespace Turn_Timer_WPF
{
    /// <summary>
    /// Interaction logic for View_Settings.xaml
    /// </summary>
    public partial class View_Settings : Window
    {
        public View_Settings()
        {
            InitializeComponent();

            DataContext = new ViewModel_Settings();
        }
    }
}

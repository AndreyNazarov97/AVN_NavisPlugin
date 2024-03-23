using AVN_NavisPlugin.ViewModels;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace AVN_NavisPlugin.Views
{
    /// <summary>
    /// Логика взаимодействия для CreateSelectionSetsWind.xaml
    /// </summary>
    public partial class CreateSelectionSetsWind : System.Windows.Window
    {
        public CreateSelectionSetsWind()
        {
            InitializeComponent();
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void openTxtButton_Click(object sender, RoutedEventArgs e)
        {
            CreateSelectionSetsFromTxt txtWindow = new CreateSelectionSetsFromTxt();
            CreateSelectionSetsFromClassificatorVM vm = new CreateSelectionSetsFromClassificatorVM(txtWindow);

            txtWindow.DataContext = vm;
            txtWindow.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            txtWindow.Show();
        }
    }
}

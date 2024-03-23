using AVN_NavisPlugin.ViewModels;
using System.Windows;
using System.Windows.Forms;

namespace AVN_NavisPlugin.Views
{
    /// <summary>
    /// Логика взаимодействия для CreateSelectionSetsFromTxt.xaml
    /// </summary>
    public partial class CreateSelectionSetsFromTxt : Window
    {
        public CreateSelectionSetsFromTxt()
        {
            InitializeComponent();
        }
        private void OpenFileDialogButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "txt (*.txt*)|*.txt*"; // Установите необходимые фильтры для типов файлов

            openFileDialog.ShowDialog();

            CreateSelectionSetsFromClassificatorVM viewModel = (CreateSelectionSetsFromClassificatorVM)DataContext;
            viewModel.Filepath = openFileDialog.FileName;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}

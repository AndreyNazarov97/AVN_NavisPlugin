using Autodesk.Navisworks.Api;
using AVN_NavisPlugin.Commands;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using Application = Autodesk.Navisworks.Api.Application;

namespace AVN_NavisPlugin
{
    internal class CreateSelectionSetsVM : INotifyPropertyChanged
    {

        private Window _window;
        private string categoryName = "Объект";
        private string propertyName = "Рабочий набор";
        private string folderName = "Test";
        private bool isFolder = false;

        public CreateSelectionSetsVM(Window wind)
        {
            _window = wind;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public string CategoryName
        {
			get => categoryName; 
			set { 
				categoryName = value;
                OnPropertyChanged(nameof(categoryName));
            }
		}

        public string PropertyName
        {
            get => propertyName;
            set
            {
                propertyName = value;
                OnPropertyChanged(nameof(propertyName));
            }
        }

        public string FolderName    
        {
            get => folderName;
            set
            {
                folderName = value;
                OnPropertyChanged(nameof(folderName));
            }
        }
        
        public bool IsFolder
        {
            get => isFolder;
            set
            {
                isFolder = value;
                OnPropertyChanged(nameof(isFolder));
            }
        }


        public ICommand CreateSelectionSetsCommand => new RelayCommand(() =>
        {
            Document Doc = Application.ActiveDocument;

            var values = PropertiesParser.ParseProperties2(CategoryName, PropertyName);


            foreach (var value in values)
            {
                SearchCreator.CreateSelectionSet(CategoryName, PropertyName, value, IsFolder, FolderName);
            }

            _window.Close();

            System.Windows.Forms.MessageBox.Show("Готово", "AVN", MessageBoxButtons.OK, MessageBoxIcon.Information);

        });
        


        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}

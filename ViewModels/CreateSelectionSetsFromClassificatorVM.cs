using Autodesk.Navisworks.Api;
using AVN_NavisPlugin.Commands;
using AVN_NavisPlugin.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using Application = Autodesk.Navisworks.Api.Application;

namespace AVN_NavisPlugin.ViewModels
{
    /// <summary>
    /// Viewmodel для создание поисковых наборов из классификатора txt
    /// </summary>
    public class CreateSelectionSetsFromClassificatorVM : INotifyPropertyChanged
    {
        private Window _window;
        private string categoryName = "Тип в приложении Revit";
        private string propertyName = "Код по классификатору";
        private string folderName = "Код по классификатору";
        private string filepath = "Укажите путь до классификатора";
        private ParseClassificatorService parseClassificatorService;
        private CreateSelectionSetsService createSelectionSetsService;

        public CreateSelectionSetsFromClassificatorVM(Window wind)
        {
            _window = wind;           
        }

        public string Filepath
        {
            get => filepath;
            set
            {
                filepath = value;
                OnPropertyChanged(nameof(filepath));
            }
        }

        /// <summary>
        /// Команда по созданию поисковых наборов
        /// </summary>
        public ICommand CreateSelectionSetsCommand => new RelayCommand(() =>
        {
            Document Doc = Application.ActiveDocument;
            parseClassificatorService = new ParseClassificatorService();
            createSelectionSetsService = new CreateSelectionSetsService();

            using (Transaction tr = new Transaction(Doc, "Create Selection Sets"))
            {

                var dict = parseClassificatorService.GroupValuesByThirdColumn(Filepath);
                FolderItem rootFolder = new FolderItem() { DisplayName = folderName };
                Doc.SelectionSets.AddCopy(rootFolder);


                foreach (var item in dict)
                {
                    string itemFolderName = $"Группа {item.Key}";
                    foreach (var value in item.Value)
                    {

                        createSelectionSetsService.CreateSelectionSet(categoryName, propertyName, value, true, itemFolderName, rootFolder);
                    }
                }

            }

            _window.Close();

            System.Windows.Forms.MessageBox.Show("Готово", "AVN", MessageBoxButtons.OK, MessageBoxIcon.Information);

        });

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}

using Autodesk.Navisworks.Api;
using System.Linq;

namespace AVN_NavisPlugin.Services
{
    /// <summary>
    /// Сервис для создания поисковых наборов
    /// </summary>
    public class CreateSelectionSetsService
    {
        /// <summary>
        /// Создание поискового набора
        /// </summary>
        /// <param name="categoryName">имя вкладки со свойствами</param>
        /// <param name="propertyName">имя свойства</param>
        /// <param name="valueString">значение для поиска</param>
        /// <param name="isFolder">создавать ли папку</param>
        /// <param name="folderName">имя папки</param>
        public void CreateSelectionSet(string categoryName, string propertyName, string valueString, bool isFolder, string folderName = null, FolderItem rootFolder = null)
        {
            // Создаем новый поиск
            Search search = new Search();

            NamedConstant categoryCombinedName = new NamedConstant(categoryName);
            NamedConstant propertyCombinedName = new NamedConstant(propertyName);
            VariantData value = new VariantData(valueString);

            SearchCondition searchCondition = new SearchCondition(categoryCombinedName,
                propertyCombinedName,
                SearchConditionOptions.IgnoreNames,
                SearchConditionComparison.DisplayStringContains,
                value);

            search.SearchConditions.Add(searchCondition);
            search.Selection.SelectAll();
            search.Locations = SearchLocations.DescendantsAndSelf;
            search.PruneBelowMatch = true;


            // Cоздаем поисковой набо на основе поиска
            SelectionSet selectionSet = new SelectionSet(search) { DisplayName = valueString };
            var selectionSets = Application.ActiveDocument.SelectionSets;


            if (isFolder)
            {
                //находим в посковых наборах нужную папку
                FolderItem folder = (FolderItem)selectionSets.RootItem.Children.FirstOrDefault(x => x.DisplayName == folderName);
                //если ее нет, создаем
                if (folder == null)
                {
                    //cоздаем папку 
                    var folderItem = new FolderItem() { DisplayName = folderName };
                    //добавляем ее в поисковые наборы
                    
                    selectionSets.AddCopy(folderItem);
                        
             

                    folder = (FolderItem)selectionSets.RootItem.Children.FirstOrDefault(x => x.DisplayName == folderName);
                }

                //в эту папку добавляем поисковой набор
                selectionSets.AddCopy(folder, selectionSet);

            }
            else
            {
                selectionSets.AddCopy(selectionSet);
            }
        }
    }
}

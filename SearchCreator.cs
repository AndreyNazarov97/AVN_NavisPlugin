using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using Autodesk.Navisworks.Api;
using Autodesk.Navisworks.Api.Clash;
using Autodesk.Navisworks.Internal.ApiImplementation;
using Application = Autodesk.Navisworks.Api.Application;

namespace AVN_NavisPlugin
{
    internal class SearchCreator
    {
        /// <summary>
        /// Создание поискового набора
        /// </summary>
        /// <param name="categoryName">имя вкладки со свойствами</param>
        /// <param name="propertyName">имя свойства</param>
        /// <param name="valueString">значение для поиска</param>
        /// <param name="isFolder">создавать ли папку</param>
        /// <param name="folderName">имя папки</param>
        public static void CreateSelectionSet(string categoryName, string propertyName, string valueString, bool isFolder , string folderName = null)
        {
            // Создаем новый поиск
            Search search = new Search();

            NamedConstant categoryCombinedName = new NamedConstant(categoryName);
            NamedConstant propertyCombinedName = new NamedConstant(propertyName);
            VariantData value = new VariantData(valueString);
            
            SearchCondition searchCondition = new SearchCondition(categoryCombinedName, 
                propertyCombinedName, 
                SearchConditionOptions.IgnoreNames, 
                SearchConditionComparison.Equal,
                value);
            
            search.SearchConditions.Add(searchCondition);
            search.Selection.SelectAll();
            search.Locations = SearchLocations.DescendantsAndSelf;
            search.PruneBelowMatch = true;


            // Cоздаем поисковой набо на основе поиска
            SelectionSet selectionSet = new SelectionSet(search) {DisplayName = valueString};
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


        static public string WriteSelectionSetContent(SavedItem item, string label, string lineText)
        {
            //set the output
            string output = lineText + "+ " + label + "\n";

            //See if this SavedItem is a GroupItem
            if (item.IsGroup)
            {
                //Indent the lines below this item
                lineText += "   ";

                //iterate the children and output
                foreach (SavedItem childItem in ((Autodesk.Navisworks.Api.GroupItem)item).Children)
                {
                    output += WriteSelectionSetContent(childItem, childItem.DisplayName, lineText);
                }
            }

            //return the node information
            return output;
        }

    }
}

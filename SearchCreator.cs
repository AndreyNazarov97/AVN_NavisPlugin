using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autodesk.Navisworks.Api;
using Autodesk.Navisworks.Api.Clash;
using Autodesk.Navisworks.Internal.ApiImplementation;
using Application = Autodesk.Navisworks.Api.Application;

namespace AVN_NavisPlugin
{
    internal class SearchCreator
    {
        public static void CreateSelectionSet()
        {


            // Создаем новый поиск
            Search search = new Search();

            NamedConstant categoryCombinedName = new NamedConstant("Объект");
            NamedConstant propertyCombinedName = new NamedConstant("Рабочий набор");
            VariantData value = new VariantData("KMS");
            
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
            SelectionSet selectionSet = new SelectionSet(search) {DisplayName = "AVN_SelectionSet"};
            var selectionSets = Application.ActiveDocument.SelectionSets;

            //cоздаем папку 
            var folderItem = new FolderItem() { DisplayName = "TEST"};
            //добавляем ее в поисковые наборы
            selectionSets.AddCopy(folderItem);
            
            //находим в посковых наборах нужную папку
            FolderItem folder = (FolderItem)selectionSets.RootItem.Children.FirstOrDefault(x => x.DisplayName == "TEST");
            
            //в эту папку добавляем поисковой набор
            selectionSets.AddCopy(folder, selectionSet);
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
                foreach (SavedItem childItem in ((GroupItem)item).Children)
                {
                    output += WriteSelectionSetContent(childItem, childItem.DisplayName, lineText);
                }
            }

            //return the node information
            return output;
        }

        /// <summary>
        /// Outputs to a message box the structure of the selection sets
        /// </summary>
        static public void ListSelectionSets()
        {
            //build output string for display
            string output =
               WriteSelectionSetContent(
               Application.ActiveDocument.SelectionSets.RootItem,
               Application.ActiveDocument.Title,
               "");

            //output in a message box
            MessageBox.Show(output);
        }
        static public void TestSavedItemReference()
        {
            //first check we have some data to test this with
            if (Application.ActiveDocument.SelectionSets.RootItem.Children.Count > 0)
            {
                //get the first child item
                SavedItem item = Application.ActiveDocument.SelectionSets.RootItem.Children[0];

                //create the SavedItemReference
                SavedItemReference reference =
                   Application.ActiveDocument.SelectionSets.CreateReference(item);

                //check the resolved reference
                if (Application.ActiveDocument.SelectionSets.ResolveReference(reference)
                   == Application.ActiveDocument.SelectionSets.RootItem.Children[0])
                {
                    MessageBox.Show("resolved item matches the first child item");
                }

                //remove the SavedItem
                bool val = Application.ActiveDocument.SelectionSets.Remove(item);

                //check resolved reference is no longer valid
                if (Application.ActiveDocument.SelectionSets.ResolveReference(reference)
                   == null)
                {
                    MessageBox.Show("SavedItem no longer exists");
                }
            }
        }
    }
}

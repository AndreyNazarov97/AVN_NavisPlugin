using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Documents;
using System.Windows.Forms;
using Autodesk.Navisworks.Api;
using Autodesk.Navisworks.Api.Clash;
using Autodesk.Navisworks.Api.Plugins;
using Application = Autodesk.Navisworks.Api.Application;

namespace AVN_NavisPlugin
{
    public class PropertiesParser 
    {
        static StringBuilder sb = new StringBuilder();
        static HashSet<string> uniqueStrings = new HashSet<string>();
        static List<string> propList = new List<string>();



        // Рекурсивная функция для обхода всех элементов и получения их свойств
        static void GetProperties(ModelItem item, string categoryName, string propertyName)
        {
            // Получение свойств элемента
            var properties = item.PropertyCategories.ToList();

            

            // Вывод свойств элемента
            foreach (var property in properties)
            {
                // Находим все свойства со вкладки "categoryName", если они есть
                var objectsProperties = item.PropertyCategories.FirstOrDefault(x => x.DisplayName == categoryName)?.Properties;
                if (objectsProperties != null)
                {
                    // Находим на вкладке "categoryName" свойство "propertyName", если оно есть
                    var workSetProp = objectsProperties.FirstOrDefault(x => x.DisplayName == propertyName);
                    if (workSetProp != null)
                    {
                        // Считываем значение этого свойства
                        var mystring = workSetProp.Value.ToDisplayString();

                        if (!uniqueStrings.Contains(mystring))
                        {
                            propList.Add(mystring);
                            sb.AppendLine(mystring);
                            uniqueStrings.Add(mystring); // Добавляем строку в HashSet
                        }

                    }
                }
            }

            // Рекурсивный обход дочерних элементов
            foreach (ModelItem childItem in item.Children)
            {
                GetProperties(childItem, categoryName, propertyName);
            }
        }

        public static List<string> ParseProperties2(string categoryName, string propertyName)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();


            // Очищаем StringBuilder и HashSet перед новым вызовом
            sb.Clear();
            uniqueStrings.Clear();
            propList.Clear();

            // Получение корневого элемента модели
            ModelItem rootItem = Application.ActiveDocument.Models[0].RootItem;

            GetProperties(rootItem, categoryName, propertyName);

            SaveTxt.SaveToTxt(sb.ToString());
            stopwatch.Stop();
            MessageBox.Show($"Время обработки : {stopwatch.Elapsed}");

            return propList;
        }

        public static List<string> ParseProperties(string categoryName, string propertyName)
        {
            // Получение корневого элемента модели
            ModelItem rootItem = Application.ActiveDocument.Models[0].RootItem;

            StringBuilder sb = new StringBuilder();
            HashSet<string> uniqueStrings = new HashSet<string>();

            List<string> properties = new List<string>();

            //Create a new search
            Search s = new Search();

            //Add a search condition for ModelItems with Geometry only...
            s.SearchConditions.Add(
            SearchCondition.HasCategoryByName(PropertyCategoryNames.RevitElementId));

            //...and not hidden
            s.SearchConditions.Add(SearchCondition.HasPropertyByName(PropertyCategoryNames.Item,
            DataPropertyNames.ItemHidden).EqualValue(VariantData.FromBoolean(false)));

            //set the selection to everything
            s.Selection.SelectAll();
            s.Locations = SearchLocations.DescendantsAndSelf;
            //get the resulting collection by applying this search and apply it to the selection
            Application.ActiveDocument.CurrentSelection.CopyFrom(s.FindAll(Application.ActiveDocument, false));


            // Перебираем все элементы в модели
            foreach (var item in Application.ActiveDocument.CurrentSelection.SelectedItems)
            {
                if (item != null)
                {
                    // Находим все свойства со вкладки "categoryName", если они есть
                    var objectsProperties = item.PropertyCategories.FirstOrDefault(x => x.DisplayName == categoryName)?.Properties;
                    if (objectsProperties != null)
                    {
                        // Находим на вкладке "categoryName" свойство "propertyName", если оно есть
                        var workSetProp = objectsProperties.FirstOrDefault(x => x.DisplayName == propertyName);
                        if (workSetProp != null)
                        {
                            // Считываем значение этого свойства
                            var mystring = workSetProp.Value.ToDisplayString();

                            if (!uniqueStrings.Contains(mystring))
                            {
                                properties.Add(mystring);
                                sb.AppendLine(mystring);
                                uniqueStrings.Add(mystring); // Добавляем строку в HashSet
                            }

                        }
                    }
                }

            }


            Application.ActiveDocument.CurrentSelection.Clear();
            SaveTxt.SaveToTxt(sb.ToString());
            

            // MessageBox.Show("Готово", "AVN", MessageBoxButtons.OK, MessageBoxIcon.Information);

            return properties;
        }

        
    }
}

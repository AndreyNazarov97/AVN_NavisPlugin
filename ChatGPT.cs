using Autodesk.Navisworks.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchSetsByParameterValues
{
    //class Program
    //{
        
    //    static void Main(string[] args)
    //    {
    //        Document Doc = Application.ActiveDocument;
    //        try
    //        {
    //            // Открываем документ Navisworks
    //            string filePath = "путь_к_файлу";
                

    //            // Получаем список всех параметров в документе
    //            List<string> parameterNames = new List<string>();
    //            foreach (DataProperty dp in Doc.CurrentSelection.SelectedItems)
    //            {
    //                parameterNames.Add(dp.DisplayName);
    //            }

    //            // Создаем поисковые наборы для каждого значения параметра
    //            foreach (string parameterName in parameterNames)
    //            {
    //                // Получаем уникальные значения параметра
    //                List<string> uniqueValues = GetUniqueParameterValues(oDoc, parameterName);

    //                // Создаем поисковые наборы для каждого значения параметра
    //                foreach (string value in uniqueValues)
    //                {
    //                    SearchCondition searchCondition = SearchCondition.HasPropertyByDisplayName(parameterName, false, false);
    //                    searchCondition = searchCondition.EqualValue(value);
    //                    Search search = new Search();
    //                    search.Selection.SelectAll();
    //                    search.Selection.SearchCondition = searchCondition;
    //                    search.SearchType = SearchType.Standard;
    //                    search.DisplayName = $"{parameterName} = {value}";
    //                    oDoc.Searches.AddSearch(search);
    //                }
    //            }

    //            Console.WriteLine("Поисковые наборы успешно созданы.");

    //            // Закрываем документ Navisworks
    //            oDoc.Close();
    //        }
    //        catch (Exception ex)
    //        {
    //            Console.WriteLine("Произошла ошибка: " + ex.Message);
    //        }
    //    }

    //    // Метод для получения уникальных значений параметра
    //    static List<string> GetUniqueParameterValues(Document doc, string parameterName)
    //    {
    //        List<string> uniqueValues = new List<string>();

    //        // Получаем все значения параметра
    //        DataPropertyCategory category = doc.PropertyCategories.FindByName(parameterName);
    //        if (category != null)
    //        {
    //            foreach (DataProperty dp in category.Properties)
    //            {
    //                string value = dp.Value.ToString();
    //                if (!uniqueValues.Contains(value))
    //                {
    //                    uniqueValues.Add(value);
    //                }
    //            }
    //        }

    //        return uniqueValues;
    //    }
    //}
}

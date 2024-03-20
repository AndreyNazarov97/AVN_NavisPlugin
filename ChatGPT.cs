using Autodesk.Navisworks.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace SearchSetsByParameterValues
{
    class Program
    {
        static StringBuilder sb = new StringBuilder();
        static HashSet<string> uniqueStrings = new HashSet<string>();
        static List<string> propList = new List<string>();

        static void GetProperties(ModelItem item, string categoryName, string propertyName)
        {
            // Получение свойств элемента
            var properties = item.PropertyCategories.ToList();

            // Рекурсивный обход дочерних элементов
            foreach (ModelItem childItem in item.Children)
            {
                GetProperties(childItem, categoryName, propertyName);
            }

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
        }
    }
}

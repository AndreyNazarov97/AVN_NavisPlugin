using Autodesk.Navisworks.Api;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application = Autodesk.Navisworks.Api.Application;

namespace AVN_NavisPlugin.Services
{
    /// <summary>
    /// Сервис для парсинга свойств из документа Navisworks
    /// </summary>
    public class ParsePropertiesService
    {
        private StringBuilder sb = new StringBuilder();
        private HashSet<string> uniqueStrings = new HashSet<string>();
        private List<string> propList = new List<string>();


        // Рекурсивная функция для обхода всех элементов и получения их свойств
        /// <summary>
        /// Метод для получения всех свойст из документа Navisworks
        /// </summary>
        /// <param name="item">Элемент модели</param>
        /// <param name="categoryName">Имя категории свойств</param>
        /// <param name="propertyName">Имя свойства</param>
        public void GetProperties(ModelItem item, string categoryName, string propertyName)
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

        /// <summary>
        /// Обертка для метода GetProperties
        /// </summary>
        /// <param name="categoryName">Имя категории свойств</param>
        /// <param name="propertyName">Имя свойства</param>
        /// <returns>Список со всеми свойствами в документе</returns>
        public List<string> ParseProperties(string categoryName, string propertyName)
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

    }
}

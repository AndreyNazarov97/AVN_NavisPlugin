using System;
using System.Collections.Generic;
using System.IO;

namespace AVN_NavisPlugin.Services
{
    /// <summary>
    /// Сервис для парсинга классификатора из txt файла
    /// </summary>
    internal class ParseClassificatorService
    {
        /// <summary>
        ///Метод парсит классификатор в словарь 
        /// </summary>
        /// <param name="filePath">путь к файлу txt</param>
        public Dictionary<int, List<string>> GroupValuesByThirdColumn(string filePath)
        {
            Dictionary<int, List<string>> groupedValues = new Dictionary<int, List<string>>();

            string[] lines = File.ReadAllLines(filePath);

            foreach (string line in lines)
            {
                string[] parts = line.Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length >= 3 && int.TryParse(parts[2], out int groupKey))
                {
                    if (!groupedValues.ContainsKey(groupKey))
                    {
                        groupedValues[groupKey] = new List<string>();
                    }
                    groupedValues[groupKey].Add(parts[0]);
                }
            }
            return groupedValues;
        }
    }
}

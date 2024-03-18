using Autodesk.Navisworks.Api;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVN_NavisPlugin
{
    internal class SaveTxt
    {


        public static void SaveToTxt(string text)
        {
            string directoryPath = @"C:\Users\user\source\repos\AVN_NavisPlugin\bin\Debug";

            // Проверяем существует ли директория, если нет, то создаем ее
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            string filePath = Path.Combine(directoryPath, "output.txt");

            using (StreamWriter writer = new StreamWriter(filePath))
            {              
                writer.WriteLine(text);    
            }
        }
    }
}

using Autodesk.Navisworks.Api;
using Autodesk.Navisworks.Api.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using Application = Autodesk.Navisworks.Api.Application;

namespace AVN_NavisPlugin
{
    [Plugin("AVN_NavisPlugin", "AVN", DisplayName = "CollisionReport")]
    internal class Main : AddInPlugin
    {
        
        public override int Execute(params string[] parameters)
        {
            CreateSelectionSetsWind window = new CreateSelectionSetsWind();
            CreateSelectionSetsVM vm = new CreateSelectionSetsVM(window);

            window.DataContext = vm;
            window.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            window.Show();

            //PropertiesParser.ParseProperties2("Объект", "Рабочий набор");


            return 1;
        }
    }
}

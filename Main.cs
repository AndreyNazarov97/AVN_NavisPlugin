using Autodesk.Navisworks.Api;
using Autodesk.Navisworks.Api.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application = Autodesk.Navisworks.Api.Application;

namespace AVN_NavisPlugin
{
    [Plugin("AVN_NavisPlugin", "AVN", DisplayName = "CollisionReport")]
    internal class Main : AddInPlugin
    {
        
        public override int Execute(params string[] parameters)
        {
            Document Doc = Application.ActiveDocument;

            //PropertiesParser.ParseProperies();

            SearchCreator.CreateSelectionSet();

            return 1;
        }
    }
}

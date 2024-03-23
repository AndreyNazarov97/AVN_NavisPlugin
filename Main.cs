using Autodesk.Navisworks.Api.Plugins;
using AVN_NavisPlugin.ViewModels;
using AVN_NavisPlugin.Views;

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


            return 1;
        }
    }
}

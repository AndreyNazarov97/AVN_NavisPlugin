using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Autodesk.Navisworks.Api;
using Autodesk.Navisworks.Api.Clash;
using Autodesk.Navisworks.Api.Plugins;
using Application = Autodesk.Navisworks.Api.Application;

namespace AVN_NavisPlugin
{
    [Plugin("AVN_NavisPlugin", "AVN", DisplayName = "CollisionReport")]
    public class PropertiesParser : AddInPlugin
    {
        Document Doc = Application.ActiveDocument;

        

        public override int Execute(params string[] parameters)
        {
            StringBuilder sb = new StringBuilder();
            HashSet<string> uniqueStrings = new HashSet<string>();

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



            foreach (var item in Application.ActiveDocument.CurrentSelection.SelectedItems)
            {
                if (item != null)
                {
                    var objectsProperties = item.PropertyCategories.FirstOrDefault(x => x.DisplayName == "Объект")?.Properties;
                    if (objectsProperties != null)
                    {
                        var workSetProp = objectsProperties.FirstOrDefault(x => x.DisplayName == "Рабочий набор");
                        if (workSetProp != null)
                        {
                            var mystring = workSetProp.Value.ToDisplayString();

                            if (!uniqueStrings.Contains(mystring))
                            {
                                sb.AppendLine(mystring);
                                uniqueStrings.Add(mystring); // Добавляем строку в HashSet
                            }

                        }
                    }
                }

            }



            //var first = Application.ActiveDocument.CurrentSelection.SelectedItems.First;

            //var firstobjectsProperties = first.PropertyCategories.FirstOrDefault(x => x.DisplayName == "Объект")?.Properties;
            //var firstworkSetProp = firstobjectsProperties.FirstOrDefault(x => x.DisplayName == "Рабочий набор");
            //var firstmystring = firstworkSetProp.Value.ToDisplayString();
            //sb.Append(firstmystring);


            SaveTxt.SaveToTxt(sb.ToString());
            

            MessageBox.Show("Готово", "AVN", MessageBoxButtons.OK, MessageBoxIcon.Information);

            return 0;
        }


    }
}

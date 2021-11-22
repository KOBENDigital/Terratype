using System.Collections.Generic;
using Umbraco.Core.PropertyEditors;

namespace Terratype.ListView.DataEditors.ListView
{
	public class ListViewConfigurationEditor : ConfigurationEditor
	{

    private IDictionary<string, object> DefaultPreValues => new Dictionary<string, object>
    {
			{ "definition", "{ \"datatype\": { \"id\": null}, \"displayMap\": true, \"displayList\": true, \"listTemplate\": \"\", \"listPageSize\": 10, \"debug\": 0 }" }
		};

		public ListViewConfigurationEditor() : base()
		{
			Fields.Add(new ConfigurationField
			{
				Key = "definition",
				Name = "Config",
				View = "/App_Plugins/Terratype.ListView/views/config.html?cache=2.0.0",
				HideLabel = true,
				Description = "",
				Config = DefaultPreValues
			});
		}
	}
}
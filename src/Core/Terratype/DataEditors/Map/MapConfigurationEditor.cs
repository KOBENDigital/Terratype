using System.Collections.Generic;
using Umbraco.Core.IO;
using Umbraco.Core.PropertyEditors;

namespace Terratype.DataEditors.Map
{
	public class MapConfigurationEditor : ConfigurationEditor
	{

    private IDictionary<string, object> DefaultPreValues => new Dictionary<string, object>
    {
      { "definition", "{ \"config\": {\"height\": 400, \"gridHeight\": 400, \"debug\": 0, \"icon\": {\"id\":\"redmarker\"}, \"label\": {\"enable\": false, \"editPosition\":\"0\", \"id\": \"standard\"}}}" }
    };
    public override IDictionary<string, object> DefaultConfiguration  =>  DefaultPreValues;

		public MapConfigurationEditor() : base()
		{
			Fields.Add(new ConfigurationField
			{
				Key = "definition",
				Name = "Config",
				View = "/App_Plugins/Terratype/views/config.html?cache=2.0.0",
				HideLabel = true,
				Description = "",
			});
		}

  }
}
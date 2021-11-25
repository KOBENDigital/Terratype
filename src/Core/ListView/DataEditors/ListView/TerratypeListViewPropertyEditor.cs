using ClientDependency.Core;
using Umbraco.Core.Logging;
using Umbraco.Core.PropertyEditors;
using Umbraco.Web.PropertyEditors;

namespace Terratype.ListView.DataEditors.ListView
{
	[DataEditor(DataEditorAlias, 
    EditorType.PropertyValue,
    PropertyEditorName, 
    view: "/App_Plugins/Terratype.ListView/views/editor.html?cache=2.0.0",
    ValueType = ValueTypes.Json, 
    Group = "Map", 
    Icon = "icon-map-location")]
#if DEBUG
  [PropertyEditorAsset(ClientDependencyType.Javascript, "/App_Plugins/Terratype.ListView/scripts/terratype.listview.js?cache=2.0.0")]
#else
	[PropertyEditorAsset(ClientDependencyType.Javascript, "/App_Plugins/Terratype.ListView/scripts/terratype.listview.min.js?cache=2.0.0")]
#endif
	public class TerratypeListViewPropertyEditor : DataEditor
	{
		public const string DataEditorAlias    = "Terratype.ListView";
    public const string PropertyEditorName = "Terratype ListView";


		public TerratypeListViewPropertyEditor(ILogger logger) : base(logger)
		{
		}


		protected override IConfigurationEditor CreateConfigurationEditor() => new ListViewConfigurationEditor();

		protected override IDataValueEditor CreateValueEditor() => new ListViewDataValueEditor(Attribute);
	}
}

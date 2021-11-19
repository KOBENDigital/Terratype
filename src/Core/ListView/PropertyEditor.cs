using Umbraco.Core.Logging;
using Umbraco.Core.PropertyEditors;
namespace Terratype.ListView
{
  /// <summary>
  /// Represents a property editor for Tenant Preferences.
  /// </summary>
  [DataEditor(
    alias: PropertyEditorAlias,
    type: EditorType.PropertyValue,
    name: PropertyEditorName,
    view: "/App_Plugins/Terratype.ListView/views/editor.html?cache=2.0.0",
    Group = "Map",
    Icon = "icon-map-location")]
  public class TerratypeListViewPropertyEditor : DataEditor
  {
    public const string PropertyEditorAlias = "Terratype.ListView";
    public const string PropertyEditorName  = "Terratype ListView";

    /// <summary>
    /// Initializes a new instance of the <see cref="TerratypeListViewPropertyEditor"/> class.
    /// </summary>
    public TerratypeListViewPropertyEditor(ILogger logger)
      : base(logger)
    { }

    /// <inheritdoc />
    protected override IDataValueEditor CreateValueEditor() => new TenantPreferencesPropertyEditor();

    /// <inheritdoc />
    protected override IConfigurationEditor CreateConfigurationEditor() => new LabelConfigurationEditor();

    // provides the property value editor
    internal class TenantPreferencesPropertyEditor : DataValueEditor
    {
      public TenantPreferencesPropertyEditor()
        : base() { }

      [ConfigurationField("definition", "Config", "/App_Plugins/Terratype.ListView/views/config.html?cache=2.0.0", Description = "", HideLabel = true)]
      public Map Definition { get; set; }
    }
  }
}
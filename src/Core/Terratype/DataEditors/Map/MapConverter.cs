using System;
using Newtonsoft.Json.Linq;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Core.PropertyEditors;
using Umbraco.Core.Services;

namespace Terratype.DataEditors.Map
{
	// To use in a Razor template, where Map is the name of the Terratype property
	//
	//  CurrentPage.Map.Position.ToString()
	//  CurrentPage.Map.Zoom;

	public class MapConverter : IPropertyValueConverter
	{
		IDataTypeService DataTypeService;

		public MapConverter(IDataTypeService dataTypeService)
		{
			DataTypeService = dataTypeService;
		}


    public bool IsConverter(IPublishedPropertyType propertyType) => propertyType.EditorAlias == MapDataEditor.DataEditorAlias;
    public Type GetPropertyValueType(IPublishedPropertyType propertyType) => typeof(IMap);
    public PropertyCacheLevel GetPropertyCacheLevel(IPublishedPropertyType propertyType) => PropertyCacheLevel.Element;

		public bool? IsValue(object value, PropertyValueLevel level)
    {
      if (level == PropertyValueLevel.Source)
        return value != null && (!(value is string val) || string.IsNullOrWhiteSpace(val) == false);
      throw new NotSupportedException($"Invalid level: {level}.");
    }
		

		public object ConvertSourceToIntermediate(IPublishedElement owner, IPublishedPropertyType propertyType, object source,
      bool preview)
    {
      if (source == null) return null;


			var values = DataTypeService.GetDataType(propertyType.EditorAlias);
      var config = JObject.Parse((string)values.Configuration);

      JObject data;
      if (source is string val && !string.IsNullOrWhiteSpace(val))
      {
        data = JObject.Parse(val);
      }
      else
      {
        data = new JObject();
        MergeJson(data, config, nameof(IMap.Zoom));
        MergeJson(data, config, nameof(IMap.Position));
      }
      var innerConfig = config.GetValue("config") as JObject;
      MergeJson(data, innerConfig, nameof(IMap.Icon));
      MergeJson(data, innerConfig, nameof(IMap.Provider));
      MergeJson(data, innerConfig, nameof(IMap.Height));
      return new Terratype.Map(data);
		}

    public object ConvertIntermediateToObject(IPublishedElement owner, IPublishedPropertyType propertyType,
      PropertyCacheLevel referenceCacheLevel, object inter, bool preview)
    {
      return inter;
    }

    public object ConvertIntermediateToXPath(IPublishedElement owner, IPublishedPropertyType propertyType,
      PropertyCacheLevel referenceCacheLevel, object inter, bool preview)
    {
      return null;
    }


		private void MergeJson(JObject data, JObject config, string fieldName) => 
			data.Merge(new JObject(new JProperty(Json.PropertyName<IMap>(fieldName), config.GetValue(Json.PropertyName<IMap>(fieldName), StringComparison.InvariantCultureIgnoreCase))));
  }
}


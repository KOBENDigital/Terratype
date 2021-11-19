using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Terratype.Indexers.Sql.Persistance.Data.Dto;
using Umbraco.Core.Scoping;

namespace Terratype.Indexers.Sql.Persistance.Context
{
  public class EntryContext : IEntryContext
  {
    private static IScopeProvider _scopeProvider;
    private const string TableName = nameof(Terratype) + nameof(Indexers) + nameof(Sql) + nameof(Data.Dto.Entry);

    public EntryContext(IScopeProvider scopeProvider)
    {
      _scopeProvider = scopeProvider;
    }

    public void Write(string entryKey, Guid umbracoNode, IMap map, DateTime lastModified)
    {
      using (var scope = _scopeProvider.CreateScope())
      {
        var db = scope.Database;
        var entry = db.SingleOrDefault<Data.Dto.Entry>(
        "WHERE " +
        nameof(Data.Dto.Entry.Identifier) + " = @0",
        entryKey);
        var wgs84 = map.Position.ToWgs84();
        var json = JsonConvert.SerializeObject(map);
        if (entry == null)
        {
          entry = new Data.Dto.Entry
          {
            Identifier = entryKey,
            UmbracoNode = umbracoNode,
            Map = json,
            Latitude = wgs84.Latitude,
            Longitude = wgs84.Longitude,
            LastModified = lastModified
          };
          db.Insert(TableName, nameof(Data.Dto.Entry.Identifier), false, entry);
        }
        else
        {
          entry.UmbracoNode = umbracoNode;
          entry.Map = json;
          entry.Latitude = wgs84.Latitude;
          entry.Longitude = wgs84.Longitude;
          entry.LastModified = lastModified;
          db.Update(entry);
        }
        scope.Complete();
      }
    }

    public IEnumerable<Entry> List(Guid umbracoNode)
    {
      using (var scope = _scopeProvider.CreateScope())
      {
        var db = scope.Database;
        return db.Query<Entry>()
          .Where(x => x.UmbracoNode == umbracoNode)
          .ToList();
      }
    }

    public void Delete(Guid umbracoNode, DateTime? beforeThisDate = null)
    {
      using (var scope = _scopeProvider.CreateScope())
      {
        var db = scope.Database;
        var currentItem = db.Query<Entry>()
          .FirstOrDefault(e => e.UmbracoNode == umbracoNode && (beforeThisDate == null || e.LastModified < beforeThisDate));

        if (currentItem == null)
        {
          throw new ArgumentNullException("record does not exist");
        }
        var result = scope.Database.Delete(currentItem);
        scope.Complete();
      }
    }

    public void Delete(Entry entry)
    {
      if (entry == null)
      {
        throw new ArgumentNullException(nameof(entry));
      }
      using (var scope = _scopeProvider.CreateScope())
      {
        var db = scope.Database;
        var currentItem = db.Query<Ancestor>()
          .FirstOrDefault(a => a.Entry == entry.Identifier);

        if (currentItem == null)
        {
          throw new ArgumentNullException("record does not exist");
        }
        var result = scope.Database.Delete(currentItem);
        scope.Complete();
      }
    }
  }
}
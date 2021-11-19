using System;
using System.Collections.Generic;
using Terratype.Indexers.Sql.Persistance.Data.Dto;
using Umbraco.Core.Persistence;
using Umbraco.Core.Scoping;

namespace Terratype.Indexers.Sql.Persistance.Context
{
  public class AncestorContext : IAncestorContext
  {
    private IScopeProvider _scopeProvider;
    private const string TableName = nameof(Terratype) + nameof(Indexers) + nameof(Sql) + nameof(Data.Dto.Entry);

    public AncestorContext(IScopeProvider scopeProvider)
    {
      _scopeProvider = scopeProvider;
    }

    public void Write(Guid ancestor, string entryKey, DateTime lastModified)
    {
      using (var scope = _scopeProvider.CreateScope())
      {
        var db = scope.Database;
        var record = new Data.Dto.Ancestor
        {
          UmbracoNode = ancestor,
          Entry = entryKey,
          LastModified = lastModified
        };
        if (db.SingleOrDefault<Ancestor>(
        "WHERE " +
        nameof(Ancestor.UmbracoNode) + " = @0 AND " +
        nameof(Ancestor.Entry) + " = @1",
        ancestor, entryKey) == null)
        {
          db.Insert(record);
        }
        else
        {
          db.Update(record);
        }
        scope.Complete();
      }
    }

    public IEnumerable<Entry> List(Guid ancestor)
    {
      using (var scope = _scopeProvider.CreateScope())
      {
        var db = scope.Database;
        return db.Query<Entry>()
          .Where(x => x.UmbracoNode == ancestor)
          .ToList();
      }
    }

    public void Delete(Guid ancestor, DateTime? beforeThisDate = null)
    {

      using (var scope = _scopeProvider.CreateScope())
      {
        var db = scope.Database;
        var currentItem = db.Query<Entry>()
          .FirstOrDefault(a => a.UmbracoNode == ancestor && (beforeThisDate == null || a.LastModified < beforeThisDate));

        if (currentItem == null)
        {
          throw new ArgumentNullException("record does not exist");
        }
        var result = scope.Database.Delete(currentItem);
        scope.Complete();
      }
    }

    public void Delete(string entryKey)
    {
      using (var scope = _scopeProvider.CreateScope())
      {
        var db = scope.Database;
        var currentItem = db.Query<Ancestor>()
          .FirstOrDefault(a => a.Entry == entryKey);

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
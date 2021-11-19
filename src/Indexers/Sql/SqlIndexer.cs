using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using Terratype.Indexer;
using Terratype.Indexer.Searchers;
using Terratype.Indexers.Sql.Persistance.Context;
using Umbraco.Core.Logging;

namespace Terratype.Indexers
{
  public class SqlIndexer : IndexerBase, IAncestorSearch<AncestorSearchRequest>
  {
    private readonly ILogger _logger;
    private readonly IAncestorContext _ancestorContext;
    private readonly IEntryContext _entryContext;

    public SqlIndexer(ILogger logger, IEntryContext entryContext, IAncestorContext ancestorContext)
    {
      _logger = logger;
      _entryContext = entryContext;
      _ancestorContext = ancestorContext;
    }
    public const string _Id = "Terratype.Indexer.Sql";
    public override string Id => _Id;

    public override bool MasterOnly => true;

    public override string Name => throw new NotImplementedException();

    public override string Description => throw new NotImplementedException();

    public override bool Sync(IEnumerable<Guid> remove, IEnumerable<Entry> add)
    {
      try
      {

        var now = DateTime.UtcNow;

        if (add != null)
        {
          foreach (var entry in add)
          {
            _entryContext.Write(entry.Key, entry.Id, entry.Map, now);
            _ancestorContext.Write(entry.Id, entry.Key, now);
            foreach (var ancestor in entry.Ancestors)
            {
              _ancestorContext.Write(ancestor, entry.Key, now);
            }
          }
        }

        if (remove != null)
        {
          foreach (var guid in remove)
          {
            _ancestorContext.Delete(guid, now);
            _entryContext.Delete(guid, now);
          }
        }
        return true;
      }
      catch (Exception ex)
      {
        _logger.Error<SqlIndexer>($"Error trying to sync content with indexer", ex);
      }

      return false;
    }

    public IEnumerable<IMap> Execute(AncestorSearchRequest request)
    {
      return _ancestorContext
        .List(request.Ancestor)
        .Select(x => ToMap(x.Map));
    }

    private IMap ToMap(string map)
    {
      return JsonConvert.DeserializeObject<IMap>(map);
    }
  }
}
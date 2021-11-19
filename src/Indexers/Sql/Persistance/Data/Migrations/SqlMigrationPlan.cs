using System;
using System.Collections.Generic;
using Terratype.Indexer;
using Terratype.Indexers.Sql.Persistance.Context;
using Terratype.Indexers.Sql.Persistance.Data.Dto;
using Terratype.Indexers.Sql.Persistance.Data.Migrations.Dto.Entry;
using Umbraco.Core;
using Umbraco.Core.Logging;
using Umbraco.Core.Migrations;
using Umbraco.Core.Services;
using Entry = Terratype.Indexers.Sql.Persistance.Data.Dto.Entry;

namespace Terratype.Indexers.Sql.Persistance.Data.Migrations
{
  public class SqlMigrationPlan : MigrationBase
  {

    private readonly IContentService                  _contentService;
    private readonly IAncestorContext                 _ancestorContext;
    private readonly IEntryContext                    _entryContext;
    private readonly ContentService _indexerContentService;

    public SqlMigrationPlan(IMigrationContext context, IContentService contentService, IEntityService entityService, IDataTypeService dataTypeService, IContentTypeService contentTypeService, ILogger logger) : base(context)
    {
      _contentService = contentService;
      _indexerContentService = new ContentService(entityService, contentService, dataTypeService, contentTypeService, logger);

    }

    public override void Migrate()
    {
      try
      {
        Logger.Debug(GetType(), "Running migration {MigrationStep}", "MigrationCreateTables");
        var entryTableName    = nameof(Terratype) + nameof(Indexers) + nameof(Sql) + nameof(Entry);
        var ancestorTableName = nameof(Terratype) + nameof(Indexer) + nameof(Sql) + nameof(Ancestor);
        var now               = DateTime.UtcNow;

        //	Create tables
        if (TableExists(entryTableName) == false)
        {
          Create.Table<Entry100>().Do();
        }
        else
        {
          Logger.Debug(GetType(), $"The database table {entryTableName} already exists, skipping", "Entry100Poco");
        }

        if (TableExists(ancestorTableName) == false)
        {

          Create.Table<Ancestor>().Do();
        }
        else
        {
          Logger.Debug(GetType(), $"The database table {ancestorTableName} already exists, skipping", "Entry100Poco");
        }

        var contents = new Stack<Umbraco.Core.Models.IContent>();
        foreach (var content in _contentService.GetPagedChildren(Constants.System.Root, 1, int.MaxValue,
          out var totalChildren))
        {
          if (content.Published)
          {
            contents.Push(content);
          }
        }

        while (contents.Count != 0)
        {
          var content = contents.Pop();
          foreach (var child in _contentService.GetPagedChildren(content.Id, 1, int.MaxValue, out var totalChildren))
          {
            if (child.Published)
            {
              contents.Push(child);
            }
          }

          foreach (var entry in _indexerContentService.Entries(new[] { content }))
          {
            _entryContext.Write(entry.Key, entry.Id, entry.Map, now);
            _ancestorContext.Write(entry.Id, entry.Key, now);
            foreach (var ancestor in entry.Ancestors)
            {
              _ancestorContext.Write(ancestor, entry.Key, now);
            }
          }

          System.Threading.Thread.Sleep(50);
        }
      }
      catch (Exception ex)
      {
        Logger.Error<SqlIndexer>($"Error trying to create content with indexer", ex);
      }
    }
  }
}
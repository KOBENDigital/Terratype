using Terratype.Indexers.Sql.Persistance.Context;
using Terratype.Indexers.Sql.Persistance.Data.Migrations;
using Umbraco.Core;
using Umbraco.Core.Composing;
using Umbraco.Core.Logging;
using Umbraco.Core.Migrations;
using Umbraco.Core.Migrations.Upgrade;
using Umbraco.Core.Scoping;
using Umbraco.Core.Services;

namespace Terratype.Indexers.Lucene
{
  [RuntimeLevel(MinLevel = RuntimeLevel.Run)]
  public class Register : IUserComposer
  {
    public void Compose(Composition composition)
    {
      var container = new LightInject.ServiceContainer();
      container.Register<Indexer.IndexerBase, SqlIndexer>(SqlIndexer._Id);
      composition.Register<IEntryContext, EntryContext>(Lifetime.Scope);
      composition.Register<IAncestorContext, AncestorContext>(Lifetime.Scope);
    }
  }

  public class SqlComponent : IComponent
  {
    private IScopeProvider _scopeProvider;
    private IMigrationBuilder _migrationBuilder;
    private IKeyValueService _keyValueService;
    private ILogger _logger;

    public SqlComponent(IScopeProvider scopeProvider, IMigrationBuilder migrationBuilder, IKeyValueService keyValueService, ILogger logger)
    {
      _scopeProvider = scopeProvider;
      _migrationBuilder = migrationBuilder;
      _keyValueService = keyValueService;
      _logger = logger;
    }

    public void Initialize()
    {

      var migrationPlan = new MigrationPlan(nameof(Terratype));
      migrationPlan.From(string.Empty)
          .To<SqlMigrationPlan>($"1.0.0_{nameof(Terratype) + nameof(Indexer) + nameof(Sql)}");
      var upgrader = new Upgrader(migrationPlan);
      upgrader.Execute(_scopeProvider, _migrationBuilder, _keyValueService, _logger);
    }

    public void Terminate()
    {
    }
  }
}
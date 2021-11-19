using System.Collections.Generic;
using NPoco;
using Terratype.Indexers.Sql.Persistance.Data.Migrations.Dto.Ancestor;

namespace Terratype.Indexers.Sql.Persistance.Data.Dto
{
	public class Ancestor : Ancestor100
	{
        [Ignore]
        public IEnumerable<Entry> Entries { get; set; }
	}
}
using System;
using System.Collections.Generic;
using Terratype.Indexers.Sql.Persistance.Data.Dto;

namespace Terratype.Indexers.Sql.Persistance.Context
{
  public interface IEntryContext
  {
    void Delete(Entry entry);
    void Delete(Guid umbracoNode, DateTime? beforeThisDate = null);
    IEnumerable<Entry> List(Guid umbracoNode);
    void Write(string entryKey, Guid umbracoNode, IMap map, DateTime lastModified);
  }
}
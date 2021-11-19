using System;
using System.Collections.Generic;
using Terratype.Indexers.Sql.Persistance.Data.Dto;

namespace Terratype.Indexers.Sql.Persistance.Context
{
  public interface IAncestorContext
  {
    void Delete(Guid ancestor, DateTime? beforeThisDate = null);
    void Delete(string entryKey);
    IEnumerable<Entry> List(Guid ancestor);
    void Write(Guid ancestor, string entryKey, DateTime lastModified);
  }
}
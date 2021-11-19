using System.Collections.Generic;
using System.Linq;
using Umbraco.Core.Persistence;

namespace Terratype.Indexers.Sql.Persistance.Context
{
  internal static class DbContextExtention
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="database"></param>
        /// <returns></returns>
        internal static IEnumerable<T> FetchAll<T>(this UmbracoDatabase database)
        {
            return database.Fetch<T>();
        }
    }
}

using Elasticsearch.Data.Entities;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elasticsearch.Data
{
    public static class Mapping
    {
        public static CreateIndexDescriptor CityMapping(this CreateIndexDescriptor @this)
        {
            return @this.Map<City>(m =>
                m.Properties(p =>
                    p.Keyword(k => k.Name(n => n.Id))
                    .Text(t => t.Name(n => n.Name))
                    .Text(t => t.Name(n => n.Region))
                    .Number(t => t.Name(n => n.Population))
                    .Number(t => t.Name(n => n.PostalCode))
                    .Date(t => t.Name(n => n.CreatedAt))
                    )
            );
        }
    }
}

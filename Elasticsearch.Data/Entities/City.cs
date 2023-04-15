using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elasticsearch.Data.Entities
{
    public class City
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Region { get; set; }
        [Range(0, 5000)]
        public int PostalCode { get; set; }
        [Range(0, 65000)]
        public int Population { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

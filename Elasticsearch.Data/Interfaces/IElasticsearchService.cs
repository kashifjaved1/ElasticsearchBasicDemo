using Elasticsearch.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elasticsearch.Data.Interfaces
{
    public interface IElasticsearchService
    {
        Task CheckIndex(string indexName);
        Task DeleteIndex(string indexName);
        Task InsertDocument(string indexName, City city);
        Task DeleteDocumentById(string indexName, City city);
        Task DocumentBulkInsert(string indexName, List<City> cities);
        Task<City> GetDocument(string indexName, string documentId);
        Task<List<City>> GetDocuments(string indexName);
    }
}

using Elasticsearch.Data.Entities;
using Elasticsearch.Data.Interfaces;
using Microsoft.Extensions.Configuration;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elasticsearch.Data.Services
{
    public class ElasticsearchService : IElasticsearchService
    {
        private readonly IConfiguration _configuration;
        private readonly IElasticClient _client;

        public ElasticsearchService(IConfiguration configuration)
        {
            _configuration = configuration;
            _client = CreateInstance();
        }

        private ElasticClient CreateInstance()
        {
            string host = _configuration.GetSection("ElasticsearchServer:Host").Value;
            string port = _configuration.GetSection("ElasticsearchServer:Port").Value;
            string username = _configuration.GetSection("ElasticsearchServer:Username").Value;
            string password = _configuration.GetSection("ElasticsearchServer:Password").Value;
            var settings = new ConnectionSettings(new Uri(host + ":" + port));

            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
                settings.BasicAuthentication(username, password);

            return new ElasticClient(settings);
        }
        public async Task CheckIndex(string indexName)
        {
            var any = await _client.Indices.ExistsAsync(indexName);
            if (any.Exists)
                return;

            var response = await _client.Indices.CreateAsync(
                    indexName,
                    i => i
                    .Index(indexName)
                    .CityMapping()
                    .Settings(s => s.NumberOfShards(3).NumberOfReplicas(1))
                );

            return;
        }

        public async Task DeleteIndex(string indexName)
        {
            await _client.Indices.DeleteAsync(indexName);
        }

        public async Task DeleteDocumentById(string indexName, City city)
        {
            var result = await _client.CreateAsync(city, q => q.Index(indexName));
            if(result.ApiCall?.HttpStatusCode == 409)
            {
                await _client.DeleteAsync(DocumentPath<City>.Id(city.Id).Index(indexName));
            }
        }

        public async Task DocumentBulkInsert(string indexName, List<City> cities)
        {
            await _client.IndexManyAsync(cities, index: indexName);
        }

        public async Task<City> GetDocument(string indexName, string documentId)
        {
            var result = await _client.GetAsync<City>(documentId, q => q.Index(indexName));
            return result.Source;
        }

        public async Task<List<City>> GetDocuments(string indexName)
        {
            #region Wildcard completes the letter itself        
            //var result = await _client.SearchAsync<Cities>(s => s
            //        .From(0)
            //        .Take(10)
            //        .Index(indexName)
            //        .Query(q => q
            //        .Bool(b => b
            //        .Should(m => m
            //        .Wildcard(w => w
            //        .Field("city")
            //        .Value("r*ze"))))));
            #endregion

            #region Fuzzy word can be in parametric self-complements
            //var result = await _client.SearchAsync<Cities>(s => s
            //                  .Index(indexName)
            //                  .Query(q => q
            //        .Fuzzy(fz => fz.Field("city")
            //            .Value("anka").Fuzziness(Fuzziness.EditDistance(4))
            //        )
            //    ));
            //harflerin yer değiştirmesi
            //var result = await _client.SearchAsync<Cities>(s => s
            //                  .Index(indexName)
            //                  .Query(q => q
            //        .Fuzzy(fz => fz.Field("city")
            //            .Value("rie").Transpositions(true))
            //        ));
            #endregion

            #region MatchPhrasePrefix completes the letter itself. It is higher in performance than Wildcard
            //var result = await _client.SearchAsync<Cities>(s => s
            //                    .Index(indexName)
            //                    .Query(q => q.MatchPhrasePrefix(m => m.Field(f => f.City).Query("iz").MaxExpansions(10)))
            //                   );
            #endregion

            #region MultiMatch does not have multi-case sensitivity
            // MultiMatch
            //    var response = await _client.SearchAsync<Cities>(s => s
            //                   .Index(indexName)
            //                   .Query(q => q
            //.MultiMatch(mm => mm
            //    .Fields(f => f
            //        .Field(ff => ff.City)
            //        .Field(ff => ff.Region)
            //    )
            //    .Type(TextQueryType.PhrasePrefix)
            //    .Query("iz")
            //    .MaxExpansions(10)
            //)));
            #endregion

            #region Term here must be all lowercase
            //var result = await _client.SearchAsync<Cities>(s => s
            //                    .Index(indexName)
            //                  .Size(10000)
            //                   .Query(query => query.Term(f => f.City, "rize"))
            //                   );
            #endregion

            #region Match does not have case sensitivity
            //var result = await _client.SearchAsync<Cities>(s => s
            //                      .Index(indexName)
            //                    .Size(10000)
            //                    .Query(q => q
            //                    .Match(m => m.Field("city").Query("ankara")
            //                     )));
            #endregion

            #region It works in the logic of Analyze Wildcard like query
            var result = await _client.SearchAsync<City>(s => s
                                  .Index(indexName)
                                        .Query(q => q
                                .QueryString(qs => qs
                                .AnalyzeWildcard()
                                   .Query("*" + "iz" + "*")
                                   .Fields(fs => fs
                                       .Fields(f1 => f1.Name
                                               )

                                ))));
            #endregion         

            return result.Documents.ToList();
        }

        public async Task InsertDocument(string indexName, City city)
        {
            var result = await _client.Indices.CreateAsync(city.Id, q => q.Index(indexName));
            if(result.ApiCall?.HttpStatusCode == 409)
            {
                await _client.UpdateAsync<City>(city.Id, u => u.Index(indexName).Doc(city));
            }
        }
    }
}

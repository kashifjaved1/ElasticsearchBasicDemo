using Elasticsearch.Data.Entities;
using Elasticsearch.Data.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Elasticsearch.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class HomesController : ControllerBase
    {
        private readonly IElasticsearchService _elasticsearchService;

        public HomesController(IElasticsearchService elasticsearchService)
        {
            _elasticsearchService = elasticsearchService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            #region InsertBulkData
            await InsertBulkData();
            #endregion

            #region If index doesn't exist, create it
            //await _elasticsearchService.CheckIndex("cities");
            #endregion

            #region Deletion by id
            //await _elasticsearchService.DeleteDocumentById("cities", new City { Id = "c651489f-43fa-4a19-97c9-f789e8f630fd", Name = "Rize" });
            #endregion

            #region Deletion by index
            //await _elasticsearchService.DeleteIndex("cities");
            #endregion

            #region fetch data by id
            //City city = await _elasticsearchService.GetDocument("cities", "e6120a5d-3346-4671-ae4f-af97e2daa3e4");
            #endregion

            #region Inserting data by Id
            //await _elasticsearchService.InsertDocument(
            //    "cities",
            //    new City
            //    {
            //        Id = Guid.NewGuid().ToString(),
            //        Name = "Eskişehir",
            //        CreatedAt = DateTime.Now,
            //        Population = 50000,
            //        Region = "İç Anadolu",
            //        PostalCode = 0001
            //    }
            //);
            #endregion

            #region Data update process by id
            //await _elasticsearchService.InsertDocument(
            //    "cities",
            //    new City
            //    {
            //        Id = "",
            //        Name = "Bolu",
            //        CreatedAt = DateTime.Now,
            //        Population = 50000,
            //        Region = "Karadeniz",
            //        PostalCode = 0002
            //    }
            //);
            #endregion

            #region query operations
            //List<City> cities = await _elasticsearchService.GetDocuments("cities");
            #endregion

            return Ok();
        }

        private async Task InsertBulkData()
        {
            List<City> cities = new List<City> {
                new City
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Ankara",
                    CreatedAt = DateTime.Now,
                    Population = 50000,
                    Region = "İç Anadolu",
                    PostalCode = 0001
                },
                new City {
                  Id = Guid.NewGuid().ToString(),
                  Name = "İzmir",
                  CreatedAt = DateTime.Now,
                  Population = 30500,
                  Region = "Ege",
                  PostalCode = 0002
                },
                new City {
                  Id = Guid.NewGuid().ToString(),
                  Name = "Aydın",
                  CreatedAt = DateTime.Now,
                  Population = 65000,
                  Region = "Ege",
                  PostalCode = 0003
                },
                new City {
                  Id = Guid.NewGuid().ToString(),
                  Name = "Rize",
                  CreatedAt = DateTime.Now,
                  Population = 36522,
                  Region = "Karadeniz",
                  PostalCode = 0004
                },
                new City {
                  Id = Guid.NewGuid().ToString(),
                  Name = "İstanbul",
                  CreatedAt = DateTime.Now,
                  Population = 25620,
                  Region = "Marmara",
                  PostalCode = 0005
                },
                new City {
                  Id = Guid.NewGuid().ToString(),
                  Name = "Sinop",
                  CreatedAt = DateTime.Now,
                  Population = 50669,
                  Region = "Karadeniz",
                  PostalCode = 0006
                },
                new City {
                  Id = Guid.NewGuid().ToString(),
                  Name = "Kars",
                  CreatedAt = DateTime.Now,
                  Population = 55500,
                  Region = "Doğu Anadolu",
                  PostalCode = 0007
                },
                new City {
                  Id = Guid.NewGuid().ToString(),
                  Name = "Van",
                  CreatedAt = DateTime.Now,
                  Population = 55500,
                  Region = "Doğu Anadolu",
                  PostalCode = 0008
                },
                new City {
                  Id = Guid.NewGuid().ToString(),
                  Name = "Adıyaman",
                  CreatedAt = DateTime.Now,
                  Population = 55500,
                  Region = "Güneydoğu Anadolu",
                  PostalCode = 0009
                },
            };

            await _elasticsearchService.DocumentBulkInsert("cities", cities);
        }
    }
}

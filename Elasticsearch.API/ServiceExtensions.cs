using Elasticsearch.Data.Interfaces;
using Elasticsearch.Data.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;

namespace Elasticsearch.API
{
    public static class ServiceExtensions
    {
        public static void ProjectSettings(this IServiceCollection @this)
        {

            @this.AddControllers();
            @this.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Elasticsearch.API", Version = "v1" });
            });

            @this.AddScoped<IElasticsearchService, ElasticsearchService>();
        }
    }
}

using Microsoft.Azure.Cosmos;
using ProgramApi.Helpers.AutoMapper;
using ProgramApi.Helpers.ConfigurationSettings.ConfigManager;
using ProgramApi.Helpers.Models;
using ProgramApi.Interfaces;
using ProgramApi.Repositories;

namespace ProgramApi.Helpers.Extensions
{
    public static class ServiceCollectionExtensions
    {
        //Register the service lifetime for the cosmos db
        public static IServiceCollection AddCosmosDBServices(this IServiceCollection services)
        {
            string url = ConfigSettings.ApplicationSetting.URI;
            string primaryKey = ConfigSettings.ApplicationSetting.primaryKey;
            string dbName = ConfigSettings.ApplicationSetting.cosmosDatabase;
            string programContainer = ConfigSettings.ApplicationSetting.programContainer;
            string candidatesContainer = ConfigSettings.ApplicationSetting.candidatesContainer;

            var cosmosClient = new CosmosClient(url, primaryKey);
            services.AddSingleton(cosmosClient);

            services.AddScoped<IProgramRepository>(provider =>
                new ProgramRepository(cosmosClient, dbName, programContainer));

            services.AddScoped<ICandidateApplicationRepository>(provider =>
                new CandidateApplicationRepository(cosmosClient, dbName, candidatesContainer));

            return services;
        }
        public static IServiceCollection AddOtherServices(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingProfile).Assembly);
            
            return services;
        }
    }
}

using Microsoft.Azure.Cosmos;
using ProgramApi.Helpers.DTOs;
using ProgramApi.Helpers.Models;
using ProgramApi.Interfaces;
using Serilog;

namespace ProgramApi.Repositories
{
    public class CandidateApplicationRepository : ICandidateApplicationRepository
    {
        private readonly Container _container;
        private readonly string _databaseName;
        private readonly string _containerName;
        public CandidateApplicationRepository(CosmosClient dbClient, string databaseName, string containerName)
        {
            _container = dbClient.GetContainer(databaseName, containerName);
            _databaseName = databaseName;
            _containerName = containerName;

        }

        public async Task<string> CreateApplicationAsync(CandidateApplication application)
        {
            string result = string.Empty;
            try
            {
                Random ran = new Random();
                application.id = ran.Next(100, 999).ToString();//Assign a unique reference for the application

                var createResult = await _container.CreateItemAsync(application, new PartitionKey(application.id));
                if (createResult.StatusCode == System.Net.HttpStatusCode.Created) result = "SUCCESS";
            }
            catch (Exception ex)
            {
                Log.Error("An error occured when retreiving programs from the DB", ex);
                result = null;
            }
            return result;
        }

        public async Task<IEnumerable<CandidateApplication>> GetApplicationsByProgramIdAsync(string programId)
        {
            var results = new List<CandidateApplication>();
            try
            {
                var query = _container.GetItemQueryIterator<CandidateApplication>(
                new QueryDefinition("SELECT * FROM c WHERE c.programId = @programId")
                .WithParameter("@programId", programId));
                
                while (query.HasMoreResults)
                {
                    var response = await query.ReadNextAsync();
                    results.AddRange(response);
                }
            }
            catch (Exception ex)
            {
                Log.Error("An error occured when retreiving candidates applications from the DB", ex);
                results = null;
            }
            return results;
        }
        public async Task<CandidateApplication> GetApplicationByEmailAsync(string email)
        {
            try
            {
                var query = new QueryDefinition("SELECT * FROM c WHERE c.Email = @Email")
                                .WithParameter("@Email", email);
                var iterator = _container.GetItemQueryIterator<CandidateApplication>(query);
                if (iterator.HasMoreResults)
                {
                    var response = await iterator.ReadNextAsync();
                    return response.FirstOrDefault();
                }
                return null;
            }
            catch (Exception ex)
            {
                Log.Error($"An error occurred while checking for duplicate application by email: {ex.Message}", ex);
                return null;
            }
        }

    }
}

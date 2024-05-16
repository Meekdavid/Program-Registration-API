using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Serialization.HybridRow;
using Newtonsoft.Json;
using ProgramApi.Helpers.Models;
using ProgramApi.Interfaces;
using Serilog;
using System.Net;

namespace ProgramApi.Repositories
{

    public class ProgramRepository : IProgramRepository
    {
        private readonly Container _container;
        private readonly string _databaseName;
        private readonly string _containerName;
        public ProgramRepository(CosmosClient dbClient, string databaseName, string containerName)
        {
            _container = dbClient.GetContainer(databaseName, containerName);
            _databaseName = databaseName;
            _containerName = containerName;
        }

        public async Task<string> AddQuestionAsync(string programId, Question newQuestion)
        {
            string result = string.Empty;
            try
            {
                //First check if the program exists
                var program = await GetProgramByIdAsync(programId);
                if (program == null) return "Program not found";

                if (program.ApplicationForms.Questions == null)
                {
                    //If no question already exists, instantiate a new object to store the question
                    program.ApplicationForms.Questions = new List<Question>();
                }

                program.ApplicationForms.Questions.Add(newQuestion);

                var updateResult = await UpdateProgramAsync(programId, program);
                if (updateResult == "SUCCESS") result = "SUCCESS";//Ensure apprioprate response is returned when a question is sucessfully added
            }
            catch (Exception ex)
            {
                //Log exceptions to file, to aid debugging any issues
                Log.Error("An error occurred while adding a question to the program in the DB", ex);
                result = null;
            }
            return result;
        }
        public async Task<Programs> GetProgramByIdAsync(string id)
        {
            try
            {
                ItemResponse<Programs> response = await _container.ReadItemAsync<Programs>(id, new PartitionKey(id));
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                Log.Debug($"Program for the programId {id} not found", ex);
                return null;
            }
            catch (Exception ex)
            {
                //Log exceptions to file, to aid debugging any issues
                Log.Error("An error occurred while retrieving the program from the DB", ex);
                return null;
            }
        }


        public async Task<string> UpdateProgramAsync(string id, Programs program)
        {
            string result = string.Empty;
            try
            {
                var tegd = JsonConvert.SerializeObject(program);
                var updateResult = await _container.UpsertItemAsync(program, new PartitionKey(id));
                if (updateResult.StatusCode == System.Net.HttpStatusCode.OK) result = "SUCCESS";

            }
            catch (Exception ex)
            {
                //Log exceptions to file, to aid debugging any issues
                Log.Error("An error occured when retreiving programs from the DB", ex);
                result = null;
            }
            return result;
        }
        public async Task<string> UpdateQuestionAsync(string programId, Question updatedQuestion)
        {       

            string result = string.Empty;
            try
            {
                //Ensure the program exists, to avoid exceptions
                var program = await GetProgramByIdAsync(programId);
                if (program == null) return "Program not found";

                //Also ensure the question to be updated exists
                var questionIndex = program.ApplicationForms.Questions.FindIndex(q => q.Id == updatedQuestion.Id);
                if (questionIndex != -1)
                {
                    program.ApplicationForms.Questions[questionIndex] = updatedQuestion;
                    var updateResult = await UpdateProgramAsync(programId, program);
                    if (updateResult == "SUCCESS") result = "SUCCESS";//Ensure apprioprate response is retuuned when update is sucessful
                }
                else
                {
                    result = "Question not found";
                }

            }
            catch (Exception ex)
            {
                //Log exceptions to file, to aid debugging any issues
                Log.Error("An error occured when retreiving programs from the DB", ex);
                result = null;
            }
            return result;
        }
    }
}

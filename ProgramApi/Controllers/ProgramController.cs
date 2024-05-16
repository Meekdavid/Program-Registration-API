using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProgramApi.Helpers.DTOs;
using ProgramApi.Helpers.Models;
using ProgramApi.Interfaces;

namespace ProgramApi.Controllers
{
    [ApiController]
    [Route("api/programs")]
    //Controller for the Employer
    public class ProgramController : Controller
    {
        private readonly IProgramRepository _programRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ProgramController> _logger;   

        public ProgramController(IProgramRepository programRepository, IMapper mapper, ILogger<ProgramController> logger)
        {
            _programRepository = programRepository;
            _mapper = mapper;
            _logger = logger;
        }


        //Endpoint to correctly store the different types of questions
        [ProducesResponseType(typeof(CreateQuestionResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(NotSuccessfulResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(NotSuccessfulResponse), StatusCodes.Status500InternalServerError)]
        [Consumes("application/json")]
        [HttpPost("{programId}/questions")]
        public async Task<ActionResult<CreateQuestionResponse>> CreateQuestion(string programId, [FromBody] QuestionDto questionDto)
        {

            var response = new CreateQuestionResponse();
            var returnHttpStatusCode = StatusCodes.Status200OK;
            _logger.LogInformation($"Request received to create question for program id {programId} is {JsonConvert.SerializeObject(questionDto)}");
            try
            {
                //Use auto maper to map the DTO to the model
                var question = _mapper.Map<Question>(questionDto);
                Random ran = new Random();
                question.Id = $"{questionDto.Type}-{ran.Next(100, 999)}";//Assign a unique reference to question

                //Proceed to add the question to DB
                var result = await _programRepository.AddQuestionAsync(programId, question);

                if (result == "SUCCESS")
                {
                    response.ResponseCode = "00";
                    response.ResponseMessage = "Question Sucessfully Created";
                    response.ProgramId = programId;
                    response.QuestionId = question.Id;
                    _logger.LogInformation($"New Question Sucessfully added to the program, programId: {programId}, questionId: {response.QuestionId}");
                }
                else if (result == "Program not found")
                {
                    //If the program was not found, return apprioprate response
                    response.ResponseCode = "01";
                    response.ResponseMessage = "Program not found";

                    _logger.LogWarning($"Failed to create question - program not found, questionId: {question.Id}");
                }
                else
                {
                    response.ResponseCode = "01";
                    response.ResponseMessage = "Unable to Create Question";
                    response.ProgramId = programId;
                    _logger.LogInformation($"Failed to add question to the program, programId: {programId}, questionId: {response.QuestionId}");
                }
            }
            catch(Exception ex)
            {
                //Incases of exception, return apprioprate response to employer
                response.ResponseCode = "06";
                response.ResponseMessage = "An error occured, please try again";
                response.ProgramId = programId;
                _logger.LogError($"An exception occured when adding a new question  programId: {programId}, questionId: {response.QuestionId}", ex);
            }
            return StatusCode(returnHttpStatusCode, response);
        }


        //Endpoint used if the user wants to edit the question after creating the application
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(NotSuccessfulResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(NotSuccessfulResponse), StatusCodes.Status500InternalServerError)]
        [Consumes("application/json")]
        [HttpPut("{questionId}")]
        public async Task<ActionResult<BaseResponse>> UpdateQuestion(string questionId, [FromBody] QuestionDto updateQuestionDto)
        {
            var response = new BaseResponse();
            var returnHttpStatusCode = StatusCodes.Status200OK;
            _logger.LogInformation($"Request received to update question with ID {questionId} is {JsonConvert.SerializeObject(updateQuestionDto)}");

            try
            {
                //Use auto maper to map the DTO to the model
                var question = _mapper.Map<Question>(updateQuestionDto);
                var result = await _programRepository.UpdateQuestionAsync(questionId, question);

                if (result == "SUCCESS")
                {
                    response.ResponseCode = "00";
                    response.ResponseMessage = "Question Successfully Updated";
                    _logger.LogInformation($"Question successfully updated, questionId: {questionId}");
                }
                else if (result == "Program not found")
                {
                    //If program isn't found, return response to the employer
                    response.ResponseCode = "01";
                    response.ResponseMessage = "Program not found";
                    
                    _logger.LogWarning($"Failed to update question - program not found, questionId: {questionId}");
                }
                else if (result == "Question not found")
                {
                    //If question isn't found, return response to the employer
                    response.ResponseCode = "02";
                    response.ResponseMessage = "Question not found";
                    
                    _logger.LogWarning($"Failed to update question - question not found, questionId: {questionId}");
                }
                else
                {
                    response.ResponseCode = "01";
                    response.ResponseMessage = "Unable to Update Question";
                    _logger.LogWarning($"Failed to update question, questionId: {questionId}");
                }
            }
            catch (Exception ex)
            {
                //Incases of exception, return apprioprate response to employer
                response.ResponseCode = "06";
                response.ResponseMessage = "An error occurred, please try again";
                _logger.LogError($"An exception occurred when updating the question, questionId: {questionId}", ex);
                
            }

            return StatusCode(returnHttpStatusCode, response);
        }

    }
}

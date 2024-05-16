using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProgramApi.Helpers.DTOs;
using ProgramApi.Helpers.Models;
using ProgramApi.Interfaces;

namespace ProgramApi.Controllers
{
    [ApiController]
    [Route("api/applications")]
    //Candidates application controller
    public class CandidateApplicationController : Controller
    {
        private readonly ICandidateApplicationRepository _applicationRepository;
        private readonly IProgramRepository _programRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CandidateApplicationController> _logger;
        public CandidateApplicationController(ICandidateApplicationRepository applicationRepository, IMapper mapper, ILogger<CandidateApplicationController> logger, IProgramRepository programRepository)
        {
            _applicationRepository = applicationRepository;
            _mapper = mapper;
            _logger = logger;
            _programRepository = programRepository;
        }

        //Endpoint for the front-end team to render the questions based on the question type.
        [ProducesResponseType(typeof(RetreiveQuestionResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(NotSuccessfulResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(NotSuccessfulResponse), StatusCodes.Status500InternalServerError)]
        [Consumes("application/json")]
        [HttpGet("{programId}/questions")]
        public async Task<ActionResult<RetreiveQuestionResponse>> GetQuestions(string programId)
        {
            var response = new RetreiveQuestionResponse();
            var returnHttpStatusCode = StatusCodes.Status200OK;
            _logger.LogInformation($"Request received to retrieve questions for program id {programId}");

            try
            {
                //First check if the program exists, and return apprioprate response to the candidate
                var program = await _programRepository.GetProgramByIdAsync(programId);
                if (program == null)
                {
                    response.ResponseCode = "01";
                    response.ResponseMessage = $"Program with ID {programId} not found.";
                    _logger.LogWarning($"Program with ID {programId} not found.");
                    return StatusCode(returnHttpStatusCode, response);
                }

                var questions = program.ApplicationForms;
                //Use auto maper to map the model to the DTO
                var questionsDto = _mapper.Map<ApplicationForm>(questions);

                response.ResponseCode = "00";
                response.ResponseMessage = "Successfully retrieved";
                response.programId = programId;
                response.Questions = questionsDto;

                _logger.LogInformation($"Successfully retrieved questions for program id {programId}");
            }
            catch (Exception ex)
            {
                //Incases of exception, return apprioprate response to the candidate/front end team
                response.ResponseCode = "06";
                response.ResponseMessage = "An error occurred, please try again";
                _logger.LogError($"An exception occurred when retrieving questions for programId: {programId}", ex);
            }

            return StatusCode(returnHttpStatusCode, response);
        }

        // Endpoint for the front-end team with the payload structure so that once the candidate submits the application
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(NotSuccessfulResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(NotSuccessfulResponse), StatusCodes.Status500InternalServerError)]
        [Consumes("application/json")]
        [HttpPost("SubmitApplication")]
        public async Task<ActionResult<BaseResponse>> SubmitApplication([FromBody] CandidateApplicationDto applicationDto)
        {
            var response = new BaseResponse();
            var returnHttpStatusCode = StatusCodes.Status200OK;
            _logger.LogInformation($"Request received to submit application: {JsonConvert.SerializeObject(applicationDto)}");

            try
            {
                // Check if the application already exists
                var existingApplication = await _applicationRepository.GetApplicationByEmailAsync(applicationDto.Email);
                if (existingApplication != null)
                {
                    response.ResponseCode = "02";
                    response.ResponseMessage = "Duplicate application found for the given email.";
                    _logger.LogWarning($"Duplicate application attempt for email: {applicationDto.Email}");
                    return StatusCode(returnHttpStatusCode, response);
                }

                var application = _mapper.Map<CandidateApplication>(applicationDto);
                application.id = applicationDto.Email;
                var result = await _applicationRepository.CreateApplicationAsync(application);

                if (result == "SUCCESS")
                {
                    response.ResponseCode = "00";
                    response.ResponseMessage = "Application successfully submitted";
                    _logger.LogInformation($"Application successfully submitted for programId: {application.ProgramId}, email: {application.id}");
                    returnHttpStatusCode = StatusCodes.Status201Created;
                }
                else
                {
                    response.ResponseCode = "01";
                    response.ResponseMessage = "Unable to submit application";
                    _logger.LogWarning($"Failed to submit application for programId: {application.ProgramId}, email: {application.id}");
                }
            }
            catch (Exception ex)
            {
                //Incases of exception, return apprioprate response to the candidate/from end team
                response.ResponseCode = "06";
                response.ResponseMessage = "An error occurred, please try again";
                _logger.LogError($"An exception occurred when submitting the application for email: {applicationDto.Email}", ex);
            }

            return StatusCode(returnHttpStatusCode, response);
        }

    }
}

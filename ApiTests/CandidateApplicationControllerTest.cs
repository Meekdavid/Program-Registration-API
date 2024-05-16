using AutoMapper;
using FakeItEasy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProgramApi.Controllers;
using ProgramApi.Helpers.DTOs;
using ProgramApi.Helpers.Models;
using ProgramApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiTests
{
    public class CandidateApplicationControllerTests
    {
        private readonly ICandidateApplicationRepository _applicationRepository;
        private readonly IProgramRepository _programRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CandidateApplicationController> _logger;
        private readonly CandidateApplicationController _controller;

        public CandidateApplicationControllerTests()
        {
            _applicationRepository = A.Fake<ICandidateApplicationRepository>();
            _programRepository = A.Fake<IProgramRepository>();
            _mapper = A.Fake<IMapper>();
            _logger = A.Fake<ILogger<CandidateApplicationController>>();
            _controller = new CandidateApplicationController(_applicationRepository, _mapper, _logger, _programRepository);
        }

        [Fact]
        public async Task GetQuestions_ProgramNotFound()
        {
            // Arrange
            var programId = "123";

            A.CallTo(() => _programRepository.GetProgramByIdAsync(programId)).Returns(Task.FromResult<Programs>(null));

            // Act
            var result = await _controller.GetQuestions(programId);
            var response = (RetreiveQuestionResponse)((ObjectResult)result.Result).Value;

            // Assert
            A.CallTo(() => _programRepository.GetProgramByIdAsync(programId)).MustHaveHappenedOnceExactly();
            Assert.Equal("01", response.ResponseCode);
            Assert.Equal($"Program with ID {programId} not found.", response.ResponseMessage);
        }

        [Fact]
        public async Task SubmitApplication_Successful()
        {
            // Arrange
            var applicationDto = new CandidateApplicationDto { Email = "test@example.com", ProgramId = "123" };
            var application = new CandidateApplication { Id = "test@example.com", ProgramId = "123" };

            A.CallTo(() => _mapper.Map<CandidateApplication>(applicationDto)).Returns(application);
            A.CallTo(() => _applicationRepository.GetApplicationByEmailAsync(applicationDto.Email)).Returns(Task.FromResult<CandidateApplication>(null));
            A.CallTo(() => _applicationRepository.CreateApplicationAsync(application)).Returns(Task.FromResult("SUCCESS"));

            // Act
            var result = await _controller.SubmitApplication(applicationDto);
            var response = (BaseResponse)((ObjectResult)result.Result).Value;

            // Assert
            A.CallTo(() => _applicationRepository.GetApplicationByEmailAsync(applicationDto.Email)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _applicationRepository.CreateApplicationAsync(application)).MustHaveHappenedOnceExactly();
            Assert.Equal("00", response.ResponseCode);
            Assert.Equal("Application successfully submitted", response.ResponseMessage);
        }

        [Fact]
        public async Task SubmitApplication_Duplicate()
        {
            // Arrange
            var applicationDto = new CandidateApplicationDto { Email = "test@example.com", ProgramId = "123" };
            var existingApplication = new CandidateApplication { Id = "test@example.com", ProgramId = "123" };

            A.CallTo(() => _applicationRepository.GetApplicationByEmailAsync(applicationDto.Email)).Returns(Task.FromResult(existingApplication));

            // Act
            var result = await _controller.SubmitApplication(applicationDto);
            var response = (BaseResponse)((ObjectResult)result.Result).Value;

            // Assert
            A.CallTo(() => _applicationRepository.GetApplicationByEmailAsync(applicationDto.Email)).MustHaveHappenedOnceExactly();
            Assert.Equal("02", response.ResponseCode);
            Assert.Equal("Duplicate application found for the given email.", response.ResponseMessage);
        }

        [Fact]
        public async Task GetQuestions_InternalServerError()
        {
            // Arrange
            var programId = "123";
            A.CallTo(() => _programRepository.GetProgramByIdAsync(programId)).Throws(new Exception("Database connection failed"));

            // Act
            var result = await _controller.GetQuestions(programId);
            var response = (RetreiveQuestionResponse)((ObjectResult)result.Result).Value;

            // Assert
            Assert.Equal("06", response.ResponseCode);
            Assert.Equal("An error occurred, please try again", response.ResponseMessage);
        }

        [Fact]
        public async Task SubmitApplication_InternalServerError()
        {
            // Arrange
            var applicationDto = new CandidateApplicationDto { Email = "test@example.com", ProgramId = "123" };
            var application = new CandidateApplication { Id = "test@example.com", ProgramId = "123" };

            A.CallTo(() => _mapper.Map<CandidateApplication>(applicationDto)).Returns(application);
            A.CallTo(() => _applicationRepository.GetApplicationByEmailAsync(applicationDto.Email)).Returns(Task.FromResult<CandidateApplication>(null));
            A.CallTo(() => _applicationRepository.CreateApplicationAsync(application)).Throws(new Exception("Database connection failed"));

            // Act
            var result = await _controller.SubmitApplication(applicationDto);
            var response = (BaseResponse)((ObjectResult)result.Result).Value;

            // Assert
            Assert.Equal("06", response.ResponseCode);
            Assert.Equal("An error occurred, please try again", response.ResponseMessage);
        }

    }

}

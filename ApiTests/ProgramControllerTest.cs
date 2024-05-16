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
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ApiTests
{
    public class ProgramControllerTests
    {
        private readonly IProgramRepository _programRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ProgramController> _logger;
        private readonly ProgramController _controller;

        public ProgramControllerTests()
        {
            _programRepository = A.Fake<IProgramRepository>();
            _mapper = A.Fake<IMapper>();
            _logger = A.Fake<ILogger<ProgramController>>();
            _controller = new ProgramController(_programRepository, _mapper, _logger);
        }

        [Fact]
        public async Task CreateQuestion_Successful()
        {
            // Arrange
            var programId = "123";
            var questionDto = new QuestionDto { Type = "MultipleChoice" };
            var question = new Question { Id = "MultipleChoice-123", Type = "MultipleChoice" };

            A.CallTo(() => _mapper.Map<Question>(questionDto)).Returns(question);
            A.CallTo(() => _programRepository.AddQuestionAsync(programId, question)).Returns(Task.FromResult("SUCCESS"));

            // Act
            var result = await _controller.CreateQuestion(programId, questionDto);
            var response = (CreateQuestionResponse)((ObjectResult)result.Result).Value;

            // Assert
            A.CallTo(() => _programRepository.AddQuestionAsync(programId, question)).MustHaveHappenedOnceExactly();
            Assert.Equal("00", response.ResponseCode);
            Assert.Equal("Question Sucessfully Created", response.ResponseMessage);
            Assert.Equal(programId, response.ProgramId);
            Assert.Equal(question.Id, response.QuestionId);
        }

        [Fact]
        public async Task CreateQuestion_ProgramNotFound()
        {
            // Arrange
            var programId = "123";
            var questionDto = new QuestionDto { Type = "MultipleChoice" };
            var question = new Question { Id = "MultipleChoice-123", Type = "MultipleChoice" };

            A.CallTo(() => _mapper.Map<Question>(questionDto)).Returns(question);
            A.CallTo(() => _programRepository.AddQuestionAsync(programId, question)).Returns(Task.FromResult("Program not found"));

            // Act
            var result = await _controller.CreateQuestion(programId, questionDto);
            var response = (CreateQuestionResponse)((ObjectResult)result.Result).Value;

            // Assert
            A.CallTo(() => _programRepository.AddQuestionAsync(programId, question)).MustHaveHappenedOnceExactly();
            Assert.Equal("01", response.ResponseCode);
            Assert.Equal("Program not found", response.ResponseMessage);
        }

        [Fact]
        public async Task UpdateQuestion_Successful()
        {
            // Arrange
            var questionId = "123";
            var questionDto = new QuestionDto { Type = "MultipleChoice" };
            var question = new Question { Id = "123", Type = "MultipleChoice" };

            A.CallTo(() => _mapper.Map<Question>(questionDto)).Returns(question);
            A.CallTo(() => _programRepository.UpdateQuestionAsync(questionId, question)).Returns(Task.FromResult("SUCCESS"));

            // Act
            var result = await _controller.UpdateQuestion(questionId, questionDto);
            var response = (BaseResponse)((ObjectResult)result.Result).Value;

            // Assert
            A.CallTo(() => _programRepository.UpdateQuestionAsync(questionId, question)).MustHaveHappenedOnceExactly();
            Assert.Equal("00", response.ResponseCode);
            Assert.Equal("Question Successfully Updated", response.ResponseMessage);
        }

        [Fact]
        public async Task UpdateQuestion_QuestionNotFound()
        {
            // Arrange
            var questionId = "123";
            var questionDto = new QuestionDto { Type = "MultipleChoice" };
            var question = new Question { Id = "123", Type = "MultipleChoice" };

            A.CallTo(() => _mapper.Map<Question>(questionDto)).Returns(question);
            A.CallTo(() => _programRepository.UpdateQuestionAsync(questionId, question)).Returns(Task.FromResult("Question not found"));

            // Act
            var result = await _controller.UpdateQuestion(questionId, questionDto);
            var response = (BaseResponse)((ObjectResult)result.Result).Value;

            // Assert
            A.CallTo(() => _programRepository.UpdateQuestionAsync(questionId, question)).MustHaveHappenedOnceExactly();
            Assert.Equal("02", response.ResponseCode);
            Assert.Equal("Question not found", response.ResponseMessage);
        }

        [Fact]
        public async Task CreateQuestion_InternalServerError()
        {
            // Arrange
            var programId = "123";
            var questionDto = new QuestionDto { Type = "MultipleChoice" };
            var question = new Question { Id = "MultipleChoice-123", Type = "MultipleChoice" };

            A.CallTo(() => _mapper.Map<Question>(questionDto)).Returns(question);
            A.CallTo(() => _programRepository.AddQuestionAsync(programId, question)).Throws(new Exception("Database connection failed"));

            // Act
            var result = await _controller.CreateQuestion(programId, questionDto);
            var response = (CreateQuestionResponse)((ObjectResult)result.Result).Value;

            // Assert
            Assert.Equal("06", response.ResponseCode);
            Assert.Equal("An error occured, please try again", response.ResponseMessage);
        }

        [Fact]
        public async Task UpdateQuestion_InternalServerError()
        {
            // Arrange
            var questionId = "123";
            var questionDto = new QuestionDto { Type = "MultipleChoice" };
            var question = new Question { Id = "123", Type = "MultipleChoice" };

            A.CallTo(() => _mapper.Map<Question>(questionDto)).Returns(question);
            A.CallTo(() => _programRepository.UpdateQuestionAsync(questionId, question)).Throws(new Exception("Database connection failed"));

            // Act
            var result = await _controller.UpdateQuestion(questionId, questionDto);
            var response = (BaseResponse)((ObjectResult)result.Result).Value;

            // Assert
            Assert.Equal("06", response.ResponseCode);
            Assert.Equal("An error occurred, please try again", response.ResponseMessage);
        }

    }

}

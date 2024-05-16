using ProgramApi.Helpers.DTOs;
using ProgramApi.Helpers.Models;

namespace ProgramApi.Interfaces
{
    public interface IQuestionModifier
    {
        Task<object> ConvertToQuestion(QuestionDto dto);
    }
}

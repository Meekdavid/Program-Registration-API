using ProgramApi.Helpers.Models;

namespace ProgramApi.Interfaces
{
    public interface IProgramRepository
    {
        Task<Programs> GetProgramByIdAsync(string id);
        Task<string> UpdateProgramAsync(string id, Programs program);
        Task<string> UpdateQuestionAsync(string programId, Question question);
        Task<string> AddQuestionAsync(string programId, Question question);
    }
}

using ProgramApi.Helpers.Models;

namespace ProgramApi.Interfaces
{
    public interface ICandidateApplicationRepository
    {
        Task<IEnumerable<CandidateApplication>> GetApplicationsByProgramIdAsync(string programId);
        Task<string> CreateApplicationAsync(CandidateApplication application);
        Task<CandidateApplication> GetApplicationByEmailAsync(string email);
    }
}

using ProgramApi.Helpers.Models;

namespace ProgramApi.Helpers.DTOs
{
    public class RetreiveQuestionResponse : BaseResponse
    {
        public string programId { get; set; }
        public ApplicationForm Questions { get; set; }
    }
}

namespace ProgramApi.Helpers.DTOs
{
    public class CreateQuestionResponse : BaseResponse
    {
        public string QuestionId { get; set; }
        public string ProgramId { get; set; }
    }
}

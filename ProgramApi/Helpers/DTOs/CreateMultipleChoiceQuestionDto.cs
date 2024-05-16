namespace ProgramApi.Helpers.DTOs
{
    public class CreateMultipleChoiceQuestionDto : QuestionDto
    {
        public List<string> Choices { get; set; }
        public bool AllowOtherOption { get; set; }
        public int MaxChoicesAllowed { get; set; }
    }
}

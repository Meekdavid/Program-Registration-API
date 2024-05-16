namespace ProgramApi.Helpers.DTOs
{
    public class CreateDropdownQuestionDto : QuestionDto
    {
        public List<string> Choices { get; set; }
        public bool AllowOtherOption { get; set; }
    }
}

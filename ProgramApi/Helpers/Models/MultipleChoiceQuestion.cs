namespace ProgramApi.Helpers.Models
{
    public class MultipleChoiceQuestion : Question
    {
        public List<string> Choices { get; set; }
        public bool AllowOtherOption { get; set; }
        public int MaxChoicesAllowed { get; set; }
    }
}

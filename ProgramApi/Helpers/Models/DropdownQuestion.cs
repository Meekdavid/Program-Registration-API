namespace ProgramApi.Helpers.Models
{
    public class DropdownQuestion : Question
    {
        public List<string> Choices { get; set; }
        public bool AllowOtherOption { get; set; }
    }
}

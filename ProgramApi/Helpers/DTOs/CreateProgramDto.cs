namespace ProgramApi.Helpers.DTOs
{
    public class CreateProgramDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public CreateApplicationFormDto ApplicationForms { get; set; }
    }
}

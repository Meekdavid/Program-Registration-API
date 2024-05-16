using ProgramApi.Helpers.Models;

namespace ProgramApi.Helpers.DTOs
{
    public class CreateApplicationFormDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public FieldEntity Phone { get; set; }
        public FieldEntity Nationality { get; set; }
        public FieldEntity CurrentResidence { get; set; }
        public FieldEntity IdNumber { get; set; }
        public FieldEntity DateOfBirth { get; set; }
        public FieldEntity Gender { get; set; }
        public List<QuestionDto> Questions { get; set; }
    }
}

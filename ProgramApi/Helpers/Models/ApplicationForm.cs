namespace ProgramApi.Helpers.Models
{
    public class ApplicationForm
    {
        public FieldEntity FirstName { get; set; }
        public FieldEntity LastName { get; set; }
        public FieldEntity Email { get; set; }
        public FieldEntity Phone { get; set; }
        public FieldEntity Nationality { get; set; }
        public FieldEntity CurrentResidence { get; set; }
        public FieldEntity IdNumber { get; set; }
        public FieldEntity DateOfBirth { get; set; }
        public FieldEntity Gender { get; set; }
        public List<Question> Questions { get; set; }
    }
}

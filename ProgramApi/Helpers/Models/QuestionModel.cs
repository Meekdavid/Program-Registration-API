using System.Text.Json.Serialization;

namespace ProgramApi.Helpers.Models
{
    public class QuestionModel
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("applicationForms")]
        public ApplicationForms ApplicationForms { get; set; }
    }

    public class ApplicationForms
    {
        [JsonPropertyName("firstName")]
        public ApplicationFormField FirstName { get; set; }

        [JsonPropertyName("lastName")]
        public ApplicationFormField LastName { get; set; }

        [JsonPropertyName("email")]
        public ApplicationFormField Email { get; set; }

        [JsonPropertyName("phone")]
        public ApplicationFormField Phone { get; set; }

        [JsonPropertyName("nationality")]
        public ApplicationFormField Nationality { get; set; }

        [JsonPropertyName("currentResidence")]
        public ApplicationFormField CurrentResidence { get; set; }

        [JsonPropertyName("idNumber")]
        public ApplicationFormField IdNumber { get; set; }

        [JsonPropertyName("dateOfBirth")]
        public ApplicationFormField DateOfBirth { get; set; }

        [JsonPropertyName("gender")]
        public ApplicationFormField Gender { get; set; }

        [JsonPropertyName("questions")]
        public List<QuestionT> Questions { get; set; }
    }

    public class ApplicationFormField
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("isInternal")]
        public bool IsInternal { get; set; }

        [JsonPropertyName("isHidden")]
        public bool IsHidden { get; set; }

        [JsonPropertyName("isMandatory")]
        public bool IsMandatory { get; set; }
    }

    public class QuestionT
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("questionText")]
        public string QuestionText { get; set; }

        [JsonPropertyName("options")]
        public List<string> Options { get; set; }
    }

}

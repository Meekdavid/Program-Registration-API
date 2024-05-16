using System.Text.Json.Serialization;

namespace ProgramApi.Helpers.Models
{
    public class Programs
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public ApplicationForm ApplicationForms { get; set; }
    }    

}

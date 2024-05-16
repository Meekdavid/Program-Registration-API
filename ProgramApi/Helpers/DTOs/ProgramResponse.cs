using ProgramApi.Helpers.Models;

namespace ProgramApi.Helpers.DTOs
{
    public class ProgramResponse : BaseResponse
    {
        public CreateProgramDto Programs { get; set; }
    }
}

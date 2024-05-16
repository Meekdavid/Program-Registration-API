using AutoMapper;
using ProgramApi.Helpers.DTOs;
using ProgramApi.Helpers.Models;
using ProgramApi.Interfaces;

namespace ProgramApi.Repositories
{
    public class QuestionModifier : IQuestionModifier
    {
        private readonly IMapper _mapper;
        public QuestionModifier(IMapper mapper)
        {
            _mapper = mapper;
        }
        public async Task<object> ConvertToQuestion(QuestionDto dto)
        {
            switch(dto.Type)
            {
                case "YesNo":
                    return new YesNoQuestion { Type = dto.Type, QuestionText = dto.QuestionText };
                    break;
                case "Number":
                    return new NumericQuestion { Type = dto.Type, QuestionText = dto.QuestionText };
                    break;
                case "Date":
                    return new DateQuestion { Type = dto.Type, QuestionText = dto.QuestionText };
                    break;
                case "Paragraph":
                    return new ParagraphQuestion { Type = dto.Type, QuestionText = dto.QuestionText };
                    break;
                case "MultipleChoice":
                    return _mapper.Map<MultipleChoiceQuestion>(dto.QuestionObj);
                    break;
                case "Dropdown":
                    return _mapper.Map<DropdownQuestion>(dto.QuestionObj);
                    break;
                default:
                    throw new ArgumentException("Invalid question type");
            }
        }

    }
}

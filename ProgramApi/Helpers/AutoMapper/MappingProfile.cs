using AutoMapper;
using ProgramApi.Helpers.DTOs;
using ProgramApi.Helpers.Models;

namespace ProgramApi.Helpers.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateProgramDto, Program>();
            CreateMap<CreateApplicationFormDto, ApplicationForm>();
            CreateMap<QuestionDto, Question>()
                .Include<CreateYesNoQuestionDto, YesNoQuestion>()
                .Include<CreateParagraphQuestionDto, ParagraphQuestion>()
                .Include<CreateDropdownQuestionDto, DropdownQuestion>()
                .Include<CreateMultipleChoiceQuestionDto, MultipleChoiceQuestion>()
                .Include<CreateDateQuestionDto, DateQuestion>()
                .Include<CreateNumberQuestionDto, NumericQuestion>();

            CreateMap<CreateYesNoQuestionDto, YesNoQuestion>();
            CreateMap<CreateParagraphQuestionDto, ParagraphQuestion>();
            CreateMap<CreateDropdownQuestionDto, DropdownQuestion>();
            CreateMap<CreateMultipleChoiceQuestionDto, MultipleChoiceQuestion>();
            CreateMap<CreateDateQuestionDto, DateQuestion>();
            CreateMap<CreateNumberQuestionDto, NumericQuestion>();

            CreateMap<CandidateApplicationDto, CandidateApplication>();
            CreateMap<AnswerDto, Answer>();

            CreateMap<YesNoQuestion, QuestionDto>();
            CreateMap<MultipleChoiceQuestion, Object>();
            CreateMap<DropdownQuestion, Object>();
        }
    }

}

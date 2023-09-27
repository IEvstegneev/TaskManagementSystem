using AutoMapper;
using TaskManagementSystem.Core;
using TaskManagementSystem.WebApi.Dto;

namespace ProDelivery.Ordering.Core
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<IssueNode, IssueNodeShortDto>();
        }
    }
}

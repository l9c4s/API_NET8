using API.Domain.Dto;
using API.Domain.Entities;
using AutoMapper;

namespace API_VSB.Domain.Helper
{
	public class MappingProfiles : Profile
	{
        public MappingProfiles()
        {

			CreateMap<RegisterModel, ApplicationUser>();

		}
	}
}

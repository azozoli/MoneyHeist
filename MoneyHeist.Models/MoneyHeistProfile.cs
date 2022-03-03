using AutoMapper;
using MoneyHeist.Models.Dtos;
using MoneyHeist.Models.Model;

namespace MoneyHeist.Models
{
	public class MoneyHeistProfile : Profile
	{
		public MoneyHeistProfile()
		{
			#region Member
			CreateMap<Member, MemberDto>()
				.ForMember( x => x.Skills, opt => opt.MapFrom( x => x.Skills ) )
				.ReverseMap();
			#endregion Member

			#region Heist
			CreateMap<Heist, HeistDto>()
				.ForMember( x => x.Members, opt => opt.MapFrom( x => x.Members ) )
				.ForMember( x => x.Skills, opt => opt.MapFrom( x => x.Skills ) )
				.ReverseMap();
			#endregion Heist

			#region Skills
			CreateMap<Skills, SkillsDto>()
				.ReverseMap();
			#endregion Skills

		}
	}
}

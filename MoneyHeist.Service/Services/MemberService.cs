using AutoMapper;
using MoneyHeist.Models.Dtos;
using MoneyHeist.Models.Interfaces.IRepositories;
using MoneyHeist.Models.Interfaces.IServices;
using MoneyHeist.Models.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace MoneyHeist.Service.Services
{
	public class MemberService : IMemberService
	{
		private readonly IMemberRepository _memberRepository;
		private readonly IMapper _mapper;

		public MemberService(IMemberRepository memberRepository, IMapper mapper)
		{
			_memberRepository = memberRepository;
			_mapper = mapper;
		}

		public async Task<IEnumerable<MemberDto>> GetMembersAsync()
		{
			return _mapper.Map<IEnumerable<MemberDto>>( await _memberRepository.GetMembersAsync() );
		}

		public async Task<MemberDto> GetMemberByIdAsync(int member_id)
		{
			return _mapper.Map<MemberDto>( await _memberRepository.GetMemberByIdAsync( member_id ) );
		}

		public async Task<int> AddMemberAsync(MemberDto member)
		{
			Member newMember = _mapper.Map<Member>( member );
			return await _memberRepository.AddMemberAsync( newMember );
		}

		public async Task<int> UpdateMemberAsync(MemberDto member)
		{
			return await _memberRepository.UpdateMemberAsync( _mapper.Map<Member>( member ) );
		}

		public async Task<int> UpdateMemberSkillsAsync(MemberDto member, SkillsDto[] skills, string mainSkill)
		{
			member.Skills = skills;
			return await _memberRepository.UpdateMemberAsync( _mapper.Map<Member>( member ) );
		}

		public async Task RemoveMemberSkillAsync(MemberDto member, string skillName)
		{
			SkillsDto skill = member.Skills.FirstOrDefault( x => x.Name == skillName );
			member.Skills = member.Skills.Where( val => val != skill ).ToArray();
			await _memberRepository.UpdateMemberAsync( _mapper.Map<Member>( member ) );
		}

		public async Task<bool> IsMemberValid(MemberDto member)
		{
			if ( AreSkillsWithSameNameProvided( member.Skills ) || await _memberRepository.EmailAlreadyInUseAsync( member.Email ) )
				return false;
			return true;
		}

		public bool AreSkillsWithSameNameProvided(SkillsDto[] skills)
		{
			return skills.GroupBy( x => x.Name ).Any( g => g.Count() > 1 );
		}

		public bool ContainsSkill(SkillsDto[] skills, string skill)
		{
			return skills.Any( x => x.Name == skill );
		}

	}
}

using MoneyHeist.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MoneyHeist.Models.Interfaces.IServices
{
	public interface IMemberService
	{

		Task<IEnumerable<MemberDto>> GetMembersAsync();

		Task<MemberDto> GetMemberByIdAsync(int member_id);

		Task<int> AddMemberAsync(MemberDto member);

		Task<int> UpdateMemberAsync(MemberDto member);

		Task<int> UpdateMemberSkillsAsync(MemberDto member, SkillsDto[] skills, string mainSkill);

		Task RemoveMemberSkillAsync(MemberDto member, string skillName);

		Task<bool> IsMemberValid(MemberDto member);

		bool AreSkillsWithSameNameProvided(SkillsDto[] skills);

		bool ContainsSkill(SkillsDto[] skills, string skill);

	}
}

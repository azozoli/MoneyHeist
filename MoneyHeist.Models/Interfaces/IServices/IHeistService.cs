using MoneyHeist.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MoneyHeist.Models.Interfaces.IServices
{
	public interface IHeistService
	{
		Task<IEnumerable<HeistDto>> GetAllHeistsAsync();

		Task<HeistDto> GetHeistByIdAsync(int heist_id);

		Task<int> AddHeistAsync(HeistDto heist);

		Task<int> UpdateHeistAsync(HeistDto heist);

		Task<IEnumerable<MemberDto>> GetEligibleMembersAsync(HeistDto heist);

		Task<bool> IsHeistValidAsync(HeistDto heist);

		bool AreSkillsWithSameNameAndLevelProvided(SkillsDto[] skills);

		Task CalculateOutcome(HeistDto heist);
	}
}

using MoneyHeist.Models.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MoneyHeist.Models.Interfaces.IRepositories
{
	public interface IMemberRepository
	{
		Task<IEnumerable<Member>> GetMembersAsync();

		Task<Member> GetMemberByIdAsync(int memberId);

		Task<int> AddMemberAsync(Member member);

		Task<int> UpdateMemberAsync(Member member);

		Task<bool> EmailAlreadyInUseAsync(string email);

	}
}

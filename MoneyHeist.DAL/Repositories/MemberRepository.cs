using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MoneyHeist.Models;
using MoneyHeist.Models.Interfaces.IRepositories;
using MoneyHeist.Models.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyHeist.DAL.Repositories
{
	public class MemberRepository : IMemberRepository
	{
		private readonly MoneyHeistContext _context;
		private readonly string _connectionString;

		public MemberRepository(IOptions<ConnectionSettingsMoneyHeist> connectionSettings, MoneyHeistContext context)
		{
			_connectionString = connectionSettings.Value.MoneyHeistConnectionSettings;
			_context = context;
		}

		public MemberRepository(MoneyHeistContext context)
		{
			_context = context;
		}

		public async Task<IEnumerable<Member>> GetMembersAsync()
		{
			return await _context.Member
				//.Include( x => x.Skills )
				.ToArrayAsync();
		}

		public async Task<Member> GetMemberByIdAsync(int memberId)
		{
			return await _context.Member.Where( x => x.Id == memberId ).FirstOrDefaultAsync();
		}

		public async Task<int> AddMemberAsync(Member member)
		{
			await _context.Member.AddAsync( member );
			await _context.SaveChangesAsync();
			return member.Id;
		}

		public async Task<int> UpdateMemberAsync(Member member)
		{
			_context.Member.Update( member );
			await _context.SaveChangesAsync();
			return member.Id;
		}

		public async Task<bool> EmailAlreadyInUseAsync(string email)
		{
			return await _context.Member.AnyAsync( x => x.Email == email );
		}

	}
}

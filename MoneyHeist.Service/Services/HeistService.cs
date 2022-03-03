using AutoMapper;
using MoneyHeist.Models.Dtos;
using MoneyHeist.Models.Interfaces.IRepositories;
using MoneyHeist.Models.Interfaces.IServices;
using MoneyHeist.Models.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MoneyHeist.Models.Enums;

namespace MoneyHeist.Service.Services
{
	public class HeistService : IHeistService
	{
		private readonly IHeistRepository _heistRepository;
		private readonly IMemberService _memberService;
		private readonly IMapper _mapper;

		public HeistService(IHeistRepository heistRepository, IMemberService memberService, IMapper mapper)
		{
			_heistRepository = heistRepository;
			_memberService = memberService;
			_mapper = mapper;
		}

		public async Task<IEnumerable<HeistDto>> GetAllHeistsAsync()
		{
			return _mapper.Map<IEnumerable<HeistDto>>( await _heistRepository.GetHeistsAsync() );
		}

		public async Task<HeistDto> GetHeistByIdAsync(int heist_id)
		{
			return _mapper.Map<HeistDto>( await _heistRepository.GetHeistByIdAsync( heist_id ) );
		}

		public async Task<int> AddHeistAsync(HeistDto heist)
		{
			Heist newHeist = _mapper.Map<Heist>( heist );
			return await _heistRepository.AddHeistAsync( newHeist );
		}

		public async Task<bool> IsHeistValidAsync(HeistDto heist)
		{
			if ( await _heistRepository.HeistAlreadyExistsAsync( heist.Name )
				|| heist.StartTime > heist.EndTime
					|| heist.EndTime < DateTime.Now
						|| AreSkillsWithSameNameAndLevelProvided( heist.Skills ) )
				return false;
			return true;
		}

		public async Task<int> UpdateHeistAsync(HeistDto heist)
		{
			return await _heistRepository.UpdateHeistAsync( _mapper.Map<Heist>( heist ) );
		}

		public async Task<IEnumerable<MemberDto>> GetEligibleMembersAsync(HeistDto heist)
		{
			var statuses = new byte[] { (byte) EnMemberStatus.AVAILABLE, (byte) EnMemberStatus.RETIRED };
			var members = await _memberService.GetMembersAsync();
			members = members.Where( member => statuses.Contains( (byte) member.Status ) );
			members = members.Where( member => member.Skills.Any( skill => heist.Skills.Any( heistSkill => skill.Name == heistSkill.Name && skill.Level.Length >= heistSkill.Level.Length ) ) );
			var confirmedHeists = GetAllHeistsAsync().Result.Where( x => x.Status == EnHeistStatus.CONFIRMED );
			foreach ( var confirmedHeist in confirmedHeists )
				members = members.Where( member => !confirmedHeist.Members.Any( x => x.Name == member.Name ) );
			return members;
		}

		/*


		public async Task<int> UpdateHeistAsync(Heist heist)
		{
			member.Skills = skills;
			return await _memberRepository.UpdateMemberAsync( _mapper.Map<Member>( member ) );
		}
		var startedHeist = await _heistService.StartHeistManuallyAsync( heist ); //update to IN_PROGRESS
		public async Task<int> StartHeistManuallyAsync(HeistDto heist)
		{
			heist.Status = EnHeistStatus.IN_PROGRESS;
			return await _memberRepository.UpdateMemberAsync( _mapper.Map<Heist>( heist ) );
		}
		*/


		public bool AreSkillsWithSameNameAndLevelProvided(SkillsDto[] skills)
		{
			return skills.GroupBy( x => new { x.Name, x.Level } ).Any( g => g.Count() > 1 );
		}

		public async Task CalculateOutcome(HeistDto heist)
		{
			foreach ( MemberDto member in heist.Members )
			{
				Random rand = new Random();
				if ( rand.Next( 0, 2 ) == 0 )
					continue;
				else
				{
					if ( rand.Next( 0, 2 ) == 0 )
						member.Status = EnMemberStatus.EXPIRED;
					else
						member.Status = EnMemberStatus.INCARCERATED;

					await _memberService.UpdateMemberAsync( member );
				}
			}

			heist = await GetHeistByIdAsync( heist.Id );

			var statuses = new byte[] { (byte) EnMemberStatus.EXPIRED, (byte) EnMemberStatus.INCARCERATED };

			int membersCount = heist.Members.Length;
			var statusGroup = heist.Members.GroupBy( x => new { x.Status } ).Select( g => new { Status = g.Key, Count = g.Count() } );
			var failedMembersCount = statusGroup.Where( x => statuses.Contains( (byte) x.Status.Status ) ).Sum( c => c.Count );

			heist.Outcome = ( failedMembersCount / membersCount ) < ( 1 / 3 ) ? EnHeistOutcome.FAILED : EnHeistOutcome.SUCCEEDED;
			await UpdateHeistAsync( heist );
		}
	}
}

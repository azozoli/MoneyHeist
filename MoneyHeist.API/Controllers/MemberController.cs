using Microsoft.AspNetCore.Mvc;
using MoneyHeist.Models.Dtos;
using MoneyHeist.Models.Interfaces.IServices;
using System.Threading.Tasks;

namespace MoneyHeist.API.Controllers
{
	[ApiController]
	[Route( "[controller]/[action]" )]
	public class MemberController : Controller
	{
		private readonly IMemberService _memberService;
		public MemberController(IMemberService memberService)
		{
			_memberService = memberService;
		}

		/// <summary>
		/// Get all heist members
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		[Route( "/members" )]
		public async Task<IActionResult> GetMembersAsync()
		{
			var members = await _memberService.GetMembersAsync();
			return Ok( members );
		}

		/// <summary>
		/// Gets a member by user ID
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		[Route( "/member/<member_id>" )]
		public async Task<IActionResult> GetMemberByIdAsync(int member_id)
		{
			MemberDto member = await _memberService.GetMemberByIdAsync( member_id );
			if ( member == null )
				return NotFound();
			return Ok( member );
		}

		/// <summary>
		/// Get members skills
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		[Route( "/member/<member_id>/skills" )]
		public async Task<IActionResult> GetMembersSkillsAsync(int member_id)
		{
			MemberDto member = await _memberService.GetMemberByIdAsync( member_id );
			if ( member == null )
				return NotFound();
			return Ok( member.Skills );
		}

		/// <summary>
		/// Add a potential heist member
		/// </summary>
		/// <returns></returns>
		[HttpPost]
		[Route( "/member" )]
		public async Task<IActionResult> AddMemberAsync([FromBody] MemberDto member)
		{
			if ( !( await _memberService.IsMemberValid( member ) ) )
				return BadRequest();

			int newMemberId = await _memberService.AddMemberAsync( member );
			return Created( Url.RouteUrl( newMemberId ), null );
		}

		/// <summary>
		/// Update member skills
		/// </summary>
		/// <returns></returns>
		[HttpPut]
		[Route( "/member/<member_id>/skills" )]
		public async Task<IActionResult> UpdateMemberSkillsAsync(int member_id, SkillsDto[] skills, string mainSkill)
		{
			MemberDto member = await _memberService.GetMemberByIdAsync( member_id );

			if ( member == null )
				return NotFound();

			if ( member.MainSkill.Name != mainSkill && ( !_memberService.ContainsSkill( member.Skills, mainSkill ) || !_memberService.ContainsSkill( skills, mainSkill ) )
				|| _memberService.AreSkillsWithSameNameProvided( skills ) )
				return BadRequest();

			await _memberService.UpdateMemberSkillsAsync( member, skills, mainSkill );
			return NoContent();
		}

		/// <summary>
		/// Remove a member's skill
		/// </summary>
		/// <returns></returns>
		[HttpDelete]
		[Route( "/member/<member_id>/skills/<skill_name>" )]
		public async Task<IActionResult> RemoveMembersSkillAsync(int member_id, string skill_name)
		{
			MemberDto member = await _memberService.GetMemberByIdAsync( member_id );

			if ( member == null || !_memberService.ContainsSkill( member.Skills, skill_name ) )
				return NotFound();

			await _memberService.RemoveMemberSkillAsync( member, skill_name );
			return NoContent();
		}

	}
}

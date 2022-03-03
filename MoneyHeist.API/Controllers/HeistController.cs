using Microsoft.AspNetCore.Mvc;
using MoneyHeist.Models.Dtos;
using MoneyHeist.Models.Interfaces.IServices;
using MoneyHeist.Service.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static MoneyHeist.Models.Enums;

namespace MoneyHeist.API.Controllers
{
	[ApiController]
	//[Authorize( Policy = "Reader" )]
	[Route( "api/[controller]/[action]" )]
	public class HeistController : Controller
	{
		private readonly IHeistService _heistService;
		private readonly IMemberService _memberService;
		private readonly IMailSender _mailSender;
		public HeistController(IHeistService heistService, IMemberService memberService, IMailSender mailSender)
		{
			_heistService = heistService;
			_memberService = memberService;
			_mailSender = mailSender;
		}

		/// <summary>
		/// Get all heist
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public async Task<IActionResult> GetAllHeists()
		{
			var heists = await _heistService.GetAllHeistsAsync();
			return Ok( heists );
		}

		/// <summary>
		/// Get heist by id
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		[Route( "/heist/<heist_id>" )]
		public async Task<IActionResult> GetHeistByIdAsync(int heist_id)
		{
			HeistDto heist = await _heistService.GetHeistByIdAsync( heist_id );
			if ( heist == null )
				return NotFound();
			return Ok( heist );
		}

		/// <summary>
		/// Get heist members
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		[Route( "/heist/<heist_id>/members" )]
		public async Task<IActionResult> GetHeistMembersAsync(int heist_id)
		{
			HeistDto heist = await _heistService.GetHeistByIdAsync( heist_id );
			if ( heist == null )
				return NotFound();
			if ( heist.Status == EnHeistStatus.PLANNING )
				return StatusCode( 405 );
			return Ok( heist.Members );
		}

		/// <summary>
		/// Get heist skills
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		[Route( "/heist/<heist_id>/skills" )]
		public async Task<IActionResult> GetHeistSkillsAsync(int heist_id)
		{
			HeistDto heist = await _heistService.GetHeistByIdAsync( heist_id );
			if ( heist == null )
				return NotFound();
			return Ok( heist.Skills );
		}

		/// <summary>
		/// Get heist status
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		[Route( "/heist/<heist_id>/status" )]
		public async Task<IActionResult> GetHeistStatusAsync(int heist_id)
		{
			HeistDto heist = await _heistService.GetHeistByIdAsync( heist_id );
			if ( heist == null )
				return NotFound();
			return Ok( heist.Status );
		}

		/// <summary>
		/// Add new heist
		/// </summary>
		/// <returns></returns>
		[HttpPost]
		[Route( "/heist" )]
		public async Task<IActionResult> AddHeistAsync(HeistDto heist)
		{
			if ( !( await _heistService.IsHeistValidAsync( heist ) ) )
				return BadRequest();

			int heistId = await _heistService.AddHeistAsync( heist );
			return Created( Url.RouteUrl( heistId ), null );
		}

		/// <summary>
		/// Add or update member skills
		/// </summary>
		/// <returns></returns>
		[HttpPatch]
		[Route( "/heist/<heist_id>/skills" )]
		public async Task<IActionResult> UpdateSkillsOnHeistAsync(int heist_id, SkillsDto[] skills)
		{
			HeistDto heist = await _heistService.GetHeistByIdAsync( heist_id );

			if ( !_heistService.AreSkillsWithSameNameAndLevelProvided( skills ) )
				return BadRequest();

			if ( heist == null )
				return NotFound();

			if ( heist.Status == EnHeistStatus.IN_PROGRESS )
				return StatusCode( 405 );

			heist.Skills = skills;
			await _heistService.UpdateHeistAsync( heist );
			return NoContent();
		}

		/// <summary>
		/// View members eligible to participate in a heist
		/// /// </summary>
		/// <returns></returns>
		[HttpGet]
		[Route( "/heist/<heist_id>/eligible_members" )]
		public async Task<IActionResult> GetEligibleMembersAsync(int heist_id)
		{
			HeistDto heist = await _heistService.GetHeistByIdAsync( heist_id );

			if ( heist == null )
				return NotFound();

			if ( heist.Status == EnHeistStatus.CONFIRMED )
				return StatusCode( 405 );

			var eligibleMembers = await _heistService.GetEligibleMembersAsync( heist );
			foreach ( MemberDto member in heist.Members )
				await _mailSender.SendMail( new MailItem( MailSenderItemType.NewMemberAdded, heist.Name, heist.StartTime, heist.EndTime, member.Email ) );
			return Ok( eligibleMembers );
		}

		/// <summary>
		/// Confirm members that should participate in a heist
		/// /// </summary>
		/// <returns></returns>
		[HttpPut]
		[Route( "/heist/<heist_id>/members" )]
		public async Task<IActionResult> ConfirmMembersAsync(int heist_id, MemberDto[] members)
		{
			HeistDto heist = await _heistService.GetHeistByIdAsync( heist_id );

			var eligibleMembers = _heistService.GetEligibleMembersAsync( heist ).Result.Select( x => x.Name ).ToArray();
			if ( members.Any( member => !eligibleMembers.Contains( member.Name ) ) )
				return BadRequest();

			if ( heist == null )
				return NotFound();

			if ( heist.Status != EnHeistStatus.PLANNING )
				return StatusCode( 405 );

			var updatedMember = await _heistService.GetEligibleMembersAsync( heist );
			foreach ( MemberDto member in heist.Members )
				await _mailSender.SendMail( new MailItem( MailSenderItemType.ConfirmedToParticipate, heist.Name, heist.StartTime, heist.EndTime, member.Email ) );
			return NoContent();
		}

		/// <summary>
		/// Start a heist manually
		/// /// </summary>
		/// <returns></returns>
		[HttpPut]
		[Route( "/heist/<heist_id>/start" )]
		public async Task<IActionResult> StartHeistManuallyAsync(int heist_id)
		{
			HeistDto heist = await _heistService.GetHeistByIdAsync( heist_id );

			if ( heist == null )
				return NotFound();

			if ( heist.Status != EnHeistStatus.READY )
				return StatusCode( 405 );

			heist.Status = EnHeistStatus.IN_PROGRESS;
			await _heistService.UpdateHeistAsync( heist );
			return Ok();
		}

		/// <summary>
		/// View the outcome of a heist
		/// /// </summary>
		/// <returns></returns>
		[HttpGet]
		[Route( "/heist/<heist_id>/outcome" )]
		public async Task<IActionResult> GetHeistOutcomeAsync(int heist_id)
		{
			HeistDto heist = await _heistService.GetHeistByIdAsync( heist_id );

			if ( heist == null )
				return NotFound();

			if ( heist.Status == EnHeistStatus.FINISHED )
				return StatusCode( 405 );

			return Ok( heist.Outcome );
		}

	}
}

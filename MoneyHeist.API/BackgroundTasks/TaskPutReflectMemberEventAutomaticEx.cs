using System;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using System.Linq;
using MoneyHeist.Service.BackgroundTasks;
using MoneyHeist.Models.Interfaces.IServices;
using MoneyHeist.Models.Dtos;
using static MoneyHeist.Models.Enums;
using MoneyHeist.Service.Mail;
using MoneyHeist.Service.TaskScheduler;
using Microsoft.Extensions.Configuration;

namespace Capacity.API.BackgroudTasks
{
	public class TaskPutReflectMemberEventAutomaticEx : TaskPutReflectMemberEventAutomatic
	{
		public TaskPutReflectMemberEventAutomaticEx(int memberId, int heistId, DateTime executionTime)
			: base( memberId, heistId, executionTime )
		{

		}

		private IHeistService _heistService => SP.GetService<IHeistService>();
		private IMemberService _memberService => SP.GetService<IMemberService>();
		private ITaskCreatorService _taskCreator => SP.GetService<ITaskCreatorService>();
		private IBackgroundWorker _backgroundWorker => SP.GetService<IBackgroundWorker>();
		private IConfiguration configuration => SP.GetService<IConfiguration>();

		public override async Task<bool> ExecuteAsync()
		{
			try
			{
				HeistDto heist = await _heistService.GetHeistByIdAsync( HeistId );
				if ( heist.Status == EnHeistStatus.IN_PROGRESS && heist.Members.Any( x => x.Id == MemberId ) )
				{
					MemberDto member = await _memberService.GetMemberByIdAsync( MemberId );
					member.Skills.ToList().ForEach( x => x.Level = x.Level + "*" );
					await _memberService.UpdateMemberSkillsAsync( member, member.Skills, member.MainSkill.Name );

					int levelUpTime = configuration.GetValue<int>( "levelUpTime" );
					TaskPutReflectMemberEventAutomatic task = _taskCreator.CreatePutReflectMemberEventTask( MemberId, HeistId, DateTime.Now.AddSeconds( levelUpTime ) );
					_backgroundWorker.AddToQueue( task );
				}
				return true;
			}
			catch
			{
				return false;
			}
		}

	}
}

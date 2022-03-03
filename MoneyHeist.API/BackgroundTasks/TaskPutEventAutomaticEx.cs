using System;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using System.Linq;
using MoneyHeist.Service.BackgroundTasks;
using MoneyHeist.Models.Interfaces.IServices;
using MoneyHeist.Models.Dtos;
using static MoneyHeist.Models.Enums;
using MoneyHeist.Service.Mail;

namespace Capacity.API.BackgroudTasks
{
	public class TaskPutEventAutomaticEx : TaskPutEventAutomatic
	{
		public TaskPutEventAutomaticEx(bool startHeist, int heistId, DateTime executionTime)
			: base( startHeist, heistId, executionTime )
		{

		}

		private IHeistService _heistService => SP.GetService<IHeistService>();
		private IMailSender _mailSender => SP.GetService<IMailSender>();

		public override async Task<bool> ExecuteAsync()
		{
			try
			{
				HeistDto heist = await _heistService.GetHeistByIdAsync( HeistId );
				if ( StartHeist )
				{
					heist.Status = EnHeistStatus.IN_PROGRESS;
					await _heistService.UpdateHeistAsync( heist );
					foreach ( MemberDto member in heist.Members )
						await _mailSender.SendMail( new MailItem( MailSenderItemType.HaistHasStarted, heist.Name, heist.StartTime, heist.EndTime, member.Email ) );
				}
				else
				{
					heist.Status = EnHeistStatus.FINISHED;
					await _heistService.UpdateHeistAsync( heist );
					await _heistService.CalculateOutcome( heist );
					foreach ( MemberDto member in heist.Members )
						await _mailSender.SendMail( new MailItem( MailSenderItemType.HaistHasFinished, heist.Name, heist.StartTime, heist.EndTime, member.Email ) );
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

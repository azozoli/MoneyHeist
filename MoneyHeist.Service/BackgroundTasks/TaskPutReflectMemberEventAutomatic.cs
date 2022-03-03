using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using MoneyHeist.Models.Interfaces.IServices;
using MoneyHeist.Service.TaskScheduler;

namespace MoneyHeist.Service.BackgroundTasks
{
	public class TaskPutReflectMemberEventAutomatic : TaskItemScheduleBase
	{
		public TaskPutReflectMemberEventAutomatic(int memberId, int heistId, DateTime executionTimeUtc)
			: base( executionTimeUtc )
		{
			MemberId = memberId;
			HeistId = heistId;
		}

		public int MemberId { get; }
		public int HeistId { get; }
		public override string Name => $"MoneyHeist TaskPutReflectMemberEventAutomatic";
		public IHeistService HeistService => SP.GetService<IHeistService>();
		public IMemberService MemberService => SP.GetService<IMemberService>();

		public override Task<bool> ExecuteAsync()
		{
			return Task.FromResult( true );
		}
	}
}

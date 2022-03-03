using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using MoneyHeist.Models.Interfaces.IServices;
using MoneyHeist.Service.TaskScheduler;

namespace MoneyHeist.Service.BackgroundTasks
{
	public class TaskPutEventAutomatic : TaskItemScheduleBase
	{
		public TaskPutEventAutomatic(bool startHeist, int heistId, DateTime executionTimeUtc)
			: base( executionTimeUtc )
		{
			StartHeist = startHeist;
			HeistId = heistId;
		}

		public bool StartHeist { get; }
		public int HeistId { get; }
		public override string Name => $"MoneyHeist TaskStartStopHeistAutomatic";
		public IHeistService HeistService => SP.GetService<IHeistService>();

		public override Task<bool> ExecuteAsync()
		{
			return Task.FromResult( true );
		}
	}
}

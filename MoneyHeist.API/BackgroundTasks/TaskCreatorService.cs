using Capacity.API.BackgroudTasks;
using MoneyHeist.Service.BackgroundTasks;
using System;

namespace MoneyHeist.API.BackgroudTasks
{
	public class TaskCreatorService : ITaskCreatorService
	{
		public TaskPutEventAutomatic CreatePutEventTask(bool startHeist, int heistId, DateTime executionTimeUtc)
		{
			return new TaskPutEventAutomaticEx( startHeist, heistId, executionTimeUtc );
		}

		public TaskPutReflectMemberEventAutomatic CreatePutReflectMemberEventTask(int memberId, int heistId, DateTime executionTimeUtc)
		{
			return new TaskPutReflectMemberEventAutomaticEx( memberId, heistId, executionTimeUtc );
		}
	}
}

using System;
using System.Collections.Generic;
using System.Text;

namespace MoneyHeist.Service.BackgroundTasks
{
	public interface ITaskCreatorService
	{
		TaskPutEventAutomatic CreatePutEventTask(bool startHeist, int heistId, DateTime executionTimeUtc);

		TaskPutReflectMemberEventAutomatic CreatePutReflectMemberEventTask(int memberId, int heistId, DateTime executionTimeUtc);
	}
}

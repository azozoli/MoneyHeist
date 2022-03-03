using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MoneyHeist.Service.TaskScheduler
{
	public interface ITaskItemSchedule : ITaskItem
	{
		DateTime ExecutionTimeUtc { get; }
	}
}

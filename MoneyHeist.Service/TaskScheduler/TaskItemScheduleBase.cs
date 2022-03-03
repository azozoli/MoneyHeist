using System;

namespace MoneyHeist.Service.TaskScheduler
{
	public abstract class TaskItemScheduleBase : TaskItemBase, ITaskItemSchedule
	{
		public TaskItemScheduleBase(DateTime execTimeUtc)
		{
			ExecutionTimeUtc = execTimeUtc;
		}

		public override TaskItemType ItemType => TaskItemType.ScheduledItem;

		public DateTime ExecutionTimeUtc { get; set; }
	}
}

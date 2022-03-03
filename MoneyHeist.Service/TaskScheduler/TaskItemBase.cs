using System;
using System.Threading.Tasks;

namespace MoneyHeist.Service.TaskScheduler
{
	public abstract class TaskItemBase : ITaskItem
	{
		public virtual TaskItemType ItemType => TaskItemType.TaskItem;

		public abstract string Name { get; }

		public IServiceProvider SP { get; set; }

		public abstract Task<bool> ExecuteAsync();
	}
}

using System;
using System.Threading.Tasks;

namespace MoneyHeist.Service.TaskScheduler
{
	public interface ITaskItem
	{
		TaskItemType ItemType { get; }
		string Name { get; }
		IServiceProvider SP { get; set; }
		Task<bool> ExecuteAsync();
	}
}

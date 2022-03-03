using System;
using System.Collections.Generic;
using System.Text;

namespace MoneyHeist.Service.TaskScheduler
{
	public interface IBackgroundWorker
	{
		void AddToQueue(ITaskItem item);
	}
}

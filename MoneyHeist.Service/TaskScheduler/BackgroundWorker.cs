using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MoneyHeist.Service.TaskScheduler
{
	public class BackgroundWorker : IBackgroundWorker, IDisposable
	{
		private Thread _thread = null;
		private WaitHandle[] _waitHandles = new WaitHandle[2]
		{
			new AutoResetEvent( false ),
			new ManualResetEvent( false )
		};

		private readonly BackgroundQueue<ITaskItem> _queue;
		private readonly ILogger _logger;
		private readonly IServiceScopeFactory _scopeFactory;
		private BackgroundWorkerInterval _workerInterval;
		private BackgroundWorkerScheduler _workerScheduler;

		public BackgroundWorker(IServiceScopeFactory spf)
		{
			_queue = new BackgroundQueue<ITaskItem>();
			_logger = Log.ForContext<BackgroundWorker>();
			_scopeFactory = spf;
			StartThread();
		}

		public void AddToQueue(ITaskItem item)
		{
			switch ( item.ItemType )
			{
				case TaskItemType.IntervalItem:
					if ( _workerInterval == null )
						_workerInterval = new BackgroundWorkerInterval( _scopeFactory );
					_workerInterval.AddToQueue( item as ITaskItemInterval );
					break;
				case TaskItemType.ScheduledItem:
					if ( _workerScheduler == null )
						_workerScheduler = new BackgroundWorkerScheduler( _scopeFactory );
					_workerScheduler.AddToQueue( item as ITaskItemSchedule );
					break;
				case TaskItemType.TaskItem:
				default:
					_queue.Enqueue( item );
					_logger.LogDebug( $"Enqueueing to queue {item.Name}" );
					WakeUp();
					break;
			}
		}

		private void StartThread()
		{
			if ( IsAlive )
				return;

			StopHandle.Reset();
			_thread = new Thread( new ThreadStart( ThreadProcess ) )
			{
				Name = "BackgroundWorker Thread"
			};
			_thread.Start();
			_logger.LogInformation( "BackgroundWorker is now running in the background." );
		}


		public void Dispose()
		{
			_logger.LogInformation( "The BackgroundWorker is stopping due to a host shutdown, queued items might not be processed anymore." );
			StopHandle.Set();
			if ( _thread != null )
			{
				_thread.Join();
				_thread = null;
			}

			if ( _workerInterval != null )
			{
				_workerInterval.Dispose();
				_workerInterval = null;
			}

			if ( _workerScheduler != null )
			{
				_workerScheduler.Dispose();
				_workerScheduler = null;
			}
		}

		private void WakeUp()
		{
			StartHandle.Set();
		}

		private async void ThreadProcess()
		{
			using ( var scope = _scopeFactory.CreateScope() )
			{
				while ( true )
				{
					int iRet = WaitHandle.WaitAny( _waitHandles );
					if ( iRet == 1 || StopHandle.WaitOne( 0 ) )
						break;

					ITaskItem task = _queue.Dequeue();
					while ( task != null )
					{
						task.SP = scope.ServiceProvider;
						await task.ExecuteAsync().ConfigureAwait( true );

						if ( StopHandle.WaitOne( 0 ) )
							break;
						task = _queue.Dequeue();
					}
				}
			}
		}

		public bool IsAlive
		{
			get { return _thread != null && _thread.IsAlive; }
		}

		public AutoResetEvent StartHandle
		{
			get { return _waitHandles[0] as AutoResetEvent; }
			set { _waitHandles[0] = value; }
		}

		public ManualResetEvent StopHandle
		{
			get { return _waitHandles[1] as ManualResetEvent; }
			set { _waitHandles[1] = value; }
		}
	}
}

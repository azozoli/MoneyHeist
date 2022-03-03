using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Diagnostics;

namespace MoneyHeist.Service.Threading
{
	public class ThreadPriorityQueue<T>
	{
		private Thread _thread;
		private WaitHandle[] _arWaitHandle = new WaitHandle[] { null, null };
		private List<T>[] _arrQueue;

		public event EventHandler EnterThread;
		public event EventHandler ExitThread;
		public event ProcessItemEventHandler<T> ProcessItem;
		public event SkipItemEventHandler<T> SkipItem;

		public ThreadPriorityQueue(int priorityCount, bool isStaThread = false)
		{
			IsSTAThread = isStaThread;
			_arWaitHandle[0] = new AutoResetEvent( false );
			_arWaitHandle[1] = new ManualResetEvent( false );
			_arrQueue = new List<T>[priorityCount];
			for ( int i = 0; i < _arrQueue.Length; i++ )
				_arrQueue[i] = new List<T>();
		}


		public void StartThread()
		{
			if ( IsAlive )
				return;

			( (ManualResetEvent) _arWaitHandle[1] ).Reset();
			_thread = new Thread( new ThreadStart( ThreadProcess ) );
			if ( IsSTAThread )
				_thread.SetApartmentState( ApartmentState.STA );
			_thread.Start();
		}

		public void StopThread()
		{
			if ( !IsAlive )
				return;

			( (ManualResetEvent) _arWaitHandle[1] ).Set();
			_thread.Join();
			_thread = null;
		}

		public virtual void AddItem(T item, int priority)
		{
			if ( !IsAlive )
			{
				Debug.Assert( false );
				return;
			}

			Monitor.Enter( this );
			_arrQueue[priority].Add( item );
			Monitor.Exit( this );
			WakeUp();
		}

		public void WakeUp()
		{
			( (AutoResetEvent) _arWaitHandle[0] ).Set();
		}

		public void Remove(T item)
		{
			Remove( item, null );
		}

		public void Remove(T item, IEqualityComparer<T> comparer)
		{
			lock ( this )
			{
				if ( comparer == null )
					comparer = EqualityComparer<T>.Default;

				for ( int queueIndex = 0; queueIndex < _arrQueue.Length; queueIndex++ )
				{
					List<T> queue = _arrQueue[queueIndex];
					for ( int j = queue.Count - 1; j >= 0; j-- )
					{
						if ( !comparer.Equals( item, queue[j] ) )
							continue;

						Debug.WriteLine( "Thread priority queue: Removing '" + queue[j].ToString() + "'." );
						queue.RemoveAt( j );
					}
				}
			}
		}


		private void ThreadProcess()
		{
			OnEnterThread( this, new EventArgs() );
			while ( true )
			{
				int iRet = WaitHandle.WaitAny( _arWaitHandle );
				if ( iRet == 1 )
					break;

				int[] arrSkipped = new int[_arrQueue.Length];
				for ( int i = 0; i < _arrQueue.Length; i++ )
				{
					Monitor.Enter( this );
					if ( _arrQueue[i].Count == arrSkipped[i] )
					{
						Monitor.Exit( this );
						continue;
					}
					T item = _arrQueue[i][arrSkipped[i]];
					SkipItemEventArgs<T> skipArgs = new SkipItemEventArgs<T>( item, i );
					OnSkipItem( this, skipArgs );
					if ( skipArgs.Skip )
						arrSkipped[i]++;
					else
						_arrQueue[i].RemoveAt( arrSkipped[i] );
					Monitor.Exit( this );
					if ( !skipArgs.Skip )
						OnProcessItem( this, new ProcessItemEventArgs<T>( item, i ) );
					i = -1;
				}
			}
			OnExitThread( this, new EventArgs() );
		}


		protected virtual void OnEnterThread(object sender, EventArgs e)
		{
			if ( EnterThread != null )
				EnterThread( sender, e );
		}

		protected virtual void OnExitThread(object sender, EventArgs e)
		{
			if ( ExitThread != null )
				ExitThread( sender, e );
		}

		protected virtual void OnProcessItem(object sender, ProcessItemEventArgs<T> e)
		{
			if ( ProcessItem != null )
				ProcessItem( sender, e );
		}

		protected virtual void OnSkipItem(object sender, SkipItemEventArgs<T> e)
		{
			if ( SkipItem != null )
				SkipItem( sender, e );
		}


		public bool IsAlive
		{
			get { return _thread != null && _thread.IsAlive; }
		}

		public bool IsSTAThread { get; private set; }

	}

	public class ProcessItemEventArgs<T> : EventArgs
	{
		private T _item;
		private int _priority;

		public ProcessItemEventArgs(T item, int priority)
		{
			_item = item;
			_priority = priority;
		}


		public T Item
		{
			get { return _item; }
		}

		public int Priority
		{
			get { return _priority; }
		}

	}

	public class SkipItemEventArgs<T> : ProcessItemEventArgs<T>
	{
		public SkipItemEventArgs(T item, int priority)
			: base( item, priority )
		{
			Skip = false;
		}


		public bool Skip { get; set; }

	}

	public delegate void ProcessItemEventHandler<T>(object sender, ProcessItemEventArgs<T> e);
	public delegate void SkipItemEventHandler<T>(object sender, SkipItemEventArgs<T> e);
}

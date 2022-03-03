using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Diagnostics;

namespace MoneyHeist.Service.Threading
{
	public class ThreadQueue<T> : ThreadPriorityQueue<T>
	{
		public ThreadQueue(bool isStaThread = false)
			: base( 1, isStaThread )
		{
		}

		public void AddItem(T item)
		{
			base.AddItem( item, 0 );
		}

		public override void AddItem(T item, int priority)
		{
			throw new Exception( "Method not supported." );
		}

	}
}

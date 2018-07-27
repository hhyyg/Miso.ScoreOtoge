using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Game.OtoGe.Library.Infra
{
	public class StopwatchDisposable : IDisposable
	{
		public StopwatchDisposable()
		{
			_stopwatch = new Stopwatch();
			_stopwatch.Start();
		}

		private Stopwatch _stopwatch;

		public void Dispose()
		{
#if DEBUG
			Console.WriteLine($"setup: {_stopwatch.ElapsedMilliseconds} ms");
#endif

			_stopwatch = null;
		}
	}
}

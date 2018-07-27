using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.OtoGe.Library.Models
{
	public class TimeManager
	{
		private static readonly TimeManager instance = new TimeManager();

		/// <summary>
		/// 経過 総 距離
		/// </summary>
		public float Distance { get; set; } = 0;

		/// <summary>
		/// 経過 総 時間 (秒)
		/// </summary>
		public float Time { get; set; } = 0;

		/// <summary>
		/// Tick
		/// </summary>
		public float Tick { get; set; } = 0;

		public int Tempo { get; set; } = 0;

		public static TimeManager Instance => instance;

		private TimeManager() { }
	}

}


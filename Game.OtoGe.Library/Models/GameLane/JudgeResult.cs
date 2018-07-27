namespace Game.OtoGe.Library.Models.GameLane
{
	public struct JudgeResult
	{
		public JudgeResult(JudgeResultType type, float diff) : this()
		{
			Type = type;
			Diff = diff;
		}

		public static JudgeResult Ignore => new JudgeResult(JudgeResultType.Ignore, 0);

		public JudgeResultType Type { get; set; }
		/// <summary>
		/// 判定ラインとの差
		/// </summary>
		public float Diff { get; set; }
	}
}

namespace Game.OtoGe.Library.Models.GameLane
{
	/// <summary>
	/// 判定の結果
	/// </summary>
	public enum JudgeResultType
	{
		/// <summary>
		/// 判定しなくてよい（右にありすぎたり）
		/// </summary>
		Ignore,
		Miss,
		Good,
		Success
	}
}

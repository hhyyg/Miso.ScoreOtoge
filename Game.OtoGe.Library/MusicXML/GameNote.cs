using Game.OtoGe.MusicXML.Poco;

namespace Game.OtoGe.Library.MusicXML
{
	/// <summary>
	/// 音符
	/// </summary>
	public struct GameNote
	{
		public GameNote(float x, long tick, ScoreNoteType type, bool isRest, bool hasDot)
		{
			X = x;
			Tick = tick;
			NoteType = type;
			IsRest = isRest;
			HasDot = hasDot;
			IsCreated = false;
		}
		/// <summary>
		/// X座標（相対座標。Boardの左端を0として）
		/// </summary>
		public float X { get; }
		/// <summary>
		/// 何Tick目か
		/// </summary>
		public long Tick { get; }

		public ScoreNoteType NoteType { get; set; }

		public bool IsRest { get; set; }

		public bool HasDot { get; set; }
		/// <summary>
		/// インスタンス化されたか
		/// </summary>
		public bool IsCreated { get; set; }
	}
}

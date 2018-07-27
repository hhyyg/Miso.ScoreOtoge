using System;

namespace Game.OtoGe.MusicXML.Poco
{
	public struct ScoreNote
	{
		//TODO: IsRest or Unpitch or Chord
		public bool IsRest { get; set; }
		public bool IsUnpitch { get; set; }
		public int Duration { get; set; }
		public ScoreNoteType Type { get; set; }

		public bool HasFermata { get; set; }

		/// <summary>
		/// 付点
		/// </summary>
		public bool HasDot { get; set; }
	}
}

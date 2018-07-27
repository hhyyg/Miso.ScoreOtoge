namespace Game.OtoGe.MusicXML.Poco
{
	/*
	Type indicates the graphic note type, Valid values(from
	shortest to longest) are 1024th, 512th, 256th, 128th,
	64th, 32nd, 16th, eighth, quarter, half, whole, breve,
	long, and maxima.The size attribute indicates full, cue,
	grace-cue, or large size.The default is full for regular
	notes, grace-cue for notes that contain both grace and cue
	elements, and cue for notes that contain either a cue or a
	grace element, but not both.
	*/
	public enum ScoreNoteType
	{
		/// <summary>
		/// 四分音符
		/// </summary>
		quarter,
		/// <summary>
		/// 八分音符
		/// </summary>
		eighth,
		/// <summary>
		/// 16分音符
		/// </summary>
		sixteenth,
	}
}

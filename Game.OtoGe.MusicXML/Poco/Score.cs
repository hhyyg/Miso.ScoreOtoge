using System;
using System.Collections.Generic;
using System.Text;

namespace Game.OtoGe.MusicXML.Poco
{
	public class Score
	{
		public int? Tempo { get; set; }

		public List<ScorePart> ScoreParts { get; set; }
	}
}

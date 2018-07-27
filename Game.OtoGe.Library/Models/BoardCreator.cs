using Game.OtoGe.Library.Models.GameLane;
using Game.OtoGe.Library.MusicXML;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game.OtoGe.Library.Models
{
    public class BoardCreator
    {
		public void Setup(string musicXmlText)
		{
			this.GameScore = _scoreParser.CreateGameScore(musicXmlText);
			
			for(int i = 0; i < _laneCount; i++)
			{
				NoteQueues[i] = 
					new NoteQueue(this.GameScore.Parts[i].Notes.Length);
			}

			SetupCompleted = true;
		}
		
		private const int _laneCount = 3;

		private GameScoreParser _scoreParser = new GameScoreParser();

		public GameScore GameScore { get; private set; }
		public bool SetupCompleted { get; private set; } = false;

		public INoteQueue[] NoteQueues { get; private set; } = new NoteQueue[_laneCount];
	}
}

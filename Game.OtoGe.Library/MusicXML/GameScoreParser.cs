using Game.OtoGe.Library.Models;
using Game.OtoGe.MusicXML;
using Game.OtoGe.MusicXML.Poco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.OtoGe.Library.MusicXML
{
	public class GameScoreParser
	{
		public GameScoreParser()
			:this(noteXAdjustment: Const.NoteXAdjustment)
		{
		}

		public GameScoreParser(float noteXAdjustment)
		{
			_movePointInTick = Const.MovePointInTick;
			_tickInTempo = Const.TickInTempo;
			_noteXAdjustment = noteXAdjustment;
		}

		private float _movePointInTick;
		private int _tickInTempo;
		/// <summary>
		/// 音符配布場所の調整値
		/// </summary>
		private float _noteXAdjustment;

		/// <summary>
		/// musicXMLTextから、何Tick目かを表すGameScoreに変換する
		/// </summary>
		/// <param name="musicXMLText"></param>
		/// <param name="movePointInTick">1Tickで動く座標</param>
		/// <returns></returns>
		public GameScore CreateGameScore(string musicXMLText)
		{
			/*
			 * 1 tempo = 480 tick
			 * 
			 * part
			 *		notes[]
			 *				何tick目か: 四分音符 -> 1:0, 2:480, 3:960
			 * 
			 */

			var score = MusicXMLParser.GetScorePartwise(musicXMLText);
			return CreateGameScore(score);
		}

		/// <summary>
		/// 譜面Scoreから、何Tick目かを表すGameScoreに変換する
		/// </summary>
		/// <param name="score"></param>
		/// <returns></returns>
		public GameScore CreateGameScore(Score score)
		{
			var gameScore = new GameScore();
			gameScore.Parts = CreateGameParts(score).ToArray();
			gameScore.Tempo = score.Tempo;

			return gameScore;
		}

		private IEnumerable<GamePart> CreateGameParts(Score score)
		{
			var gameParts = new List<GamePart>();

			int partIndex = 0;
			foreach (var part in score.ScoreParts)
			{
				partIndex++;
				//TODO: 最初のパートは無視する
				//if (partIndex == 1) continue;
				
				var gamePart = new GamePart();
				gamePart.Notes = CreateGameNotes(part).ToArray();

				yield return gamePart;
			}
		}

		private float GetX(long tick)
		{
			//Boardのどの x に配置するか
			var absX = (tick * _movePointInTick);
			var xPoint = absX + _noteXAdjustment;
			return xPoint;
		}

		/// <summary>
		/// PartからGameNotesを作成します。
		/// </summary>
		/// <param name="scorePart"></param>
		/// <returns></returns>
		private IEnumerable<GameNote> CreateGameNotes(ScorePart scorePart)
		{
			long currentTick = 0;

			var gamePart = new GamePart();
			var gameNoteList = new List<GameNote>();

			foreach (var measure in scorePart.MeasureList)
			{
				foreach (var note in measure.Notes)
				{
					GameNote? gameNote = null;
					if (note.IsRest)
					{
						//フェルマータがついている休符のみ休符として追加する
						if (note.HasFermata)
						{
							gameNote = new GameNote(x: GetX(currentTick), tick: currentTick, type: note.Type, isRest: note.IsRest, hasDot: note.HasDot);
						}
					}
					else
					{
						gameNote = new GameNote(x: GetX(currentTick), tick: currentTick, type: note.Type, isRest: note.IsRest, hasDot: note.HasDot);
					}

					//消費分すすめる
					currentTick += GetTicks(note);

					if (gameNote != null)
						yield return gameNote.Value;
				}
			}
		}

		/// <summary>
		/// その音が消費するTickを取得します
		/// </summary>
		/// <param name="scoreNote"></param>
		/// <returns></returns>
		private long GetTicks(ScoreNote scoreNote)
		{
			return GetTicksBase(scoreNote) + GetTicksDot(scoreNote);
		}

		private long GetTicksDot(ScoreNote scoreNote)
		{
			if (scoreNote.HasDot == false)
				return 0;

			switch (scoreNote.Type)
			{
				case ScoreNoteType.quarter:
					return _tickInTempo / 2;
				case ScoreNoteType.eighth:
					return _tickInTempo / 4;
				case ScoreNoteType.sixteenth:
					return _tickInTempo / 8;
				default:
					throw new NotSupportedException($"not supported: {scoreNote.Type}");
			}
		}
		
		private long GetTicksBase(ScoreNote scoreNote)
		{
			switch (scoreNote.Type)
			{
				case ScoreNoteType.quarter:
					return _tickInTempo;
				case ScoreNoteType.eighth:
					return _tickInTempo / 2;
				case ScoreNoteType.sixteenth:
					return _tickInTempo / 4;
				default:
					throw new NotSupportedException($"not supported: {scoreNote.Type}");
			}
		}
	}
}

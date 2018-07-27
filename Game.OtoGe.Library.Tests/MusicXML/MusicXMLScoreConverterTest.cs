using Game.OtoGe.Library.MusicXML;
using Game.OtoGe.MusicXML.Poco;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Xunit;

namespace Game.OtoGe.Library.Tests.MusicXML
{
    public class MusicXMLScoreConverterTest
    {
		[Fact]
		public void Test()
		{
			//TODO: xmlからではなくScoreからのテストでよい
			var testFileFolder = "Game.OtoGe.Library.Tests.TestData";
			var testFileName = "MusicXML1.xml";
			var asm = Assembly.GetExecutingAssembly();
			string text = "";
			using (var sr = new StreamReader(asm.GetManifestResourceStream($"{testFileFolder}.{testFileName}"), Encoding.UTF8))
			{
				text = sr.ReadToEnd();
			}

			var parser = new GameScoreParser(noteXAdjustment: 0);
			var gameScore = parser.CreateGameScore(text);

			Assert.Equal(3, gameScore.Parts.Length);

			{
				var part1 = gameScore.Parts[0];
				Assert.Equal(2, part1.Notes.Length);
				Assert.Equal(0, part1.Notes[0].Tick);
				Assert.Equal(0, part1.Notes[0].X);
				Assert.Equal(480 * 4, part1.Notes[1].Tick);
				Assert.Equal(480 * 4 * Const.MovePointInTick, part1.Notes[1].X);
			}
			{
				var part2 = gameScore.Parts[1];
				Assert.Equal(4, part2.Notes.Length);
				Assert.Equal(480, part2.Notes[0].Tick);
				Assert.Equal(480 * Const.MovePointInTick, part2.Notes[0].X);

				Assert.Equal(480 * 3, part2.Notes[1].Tick);
				Assert.Equal(480 * 3 * Const.MovePointInTick, part2.Notes[1].X);
			}
			{
				var part3 = gameScore.Parts[2];
				Assert.Equal(2, part3.Notes.Length);
				Assert.Equal(0, part3.Notes[0].Tick);
				Assert.Equal(0, part3.Notes[0].X);
				Assert.Equal(480 * 4, part3.Notes[1].Tick);
				Assert.Equal(480 * 4 * Const.MovePointInTick, part3.Notes[1].X);
			}

			Assert.NotNull(gameScore);
		}

		[Fact]
		public void Test_Huten_16th()
		{
			//arr
			var score = new Score();

			var notes = new ScoreNote[]
			{
				new ScoreNote()
				{
					IsRest = false,
					Type = ScoreNoteType.eighth,
					HasDot = true,
				},
				new ScoreNote()
				{
					IsRest = true, HasFermata = true,
					Type = ScoreNoteType.sixteenth,
					HasDot = false,
				}
			};

			score.ScoreParts = new List<ScorePart>()
			{
				new ScorePart(),
				new ScorePart()
				{
					MeasureList = new Measure[] { new Measure() { Notes = notes } }
				}
			};

			//do
			var parser = new GameScoreParser(noteXAdjustment: 0);
			var gameScore = parser.CreateGameScore(score);

			//assert
			var part = gameScore.Parts[0];
			{
				var note = part.Notes[0];
				long expectTick = 0;
				Assert.Equal(expectTick, note.Tick);
			}
			{
				var note = part.Notes[1];
				long expectTick = 480 * 3/4;
				Assert.Equal(expectTick, note.Tick);
			}
			
		}
    }
}

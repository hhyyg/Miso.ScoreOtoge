using System;
using System.IO;
using System.Reflection;
using System.Text;
using Xunit;
using Game.OtoGe.MusicXML;
using System.Linq;
using Game.OtoGe.MusicXML.Poco;

namespace Game.OtoGe.MusicXML.Tests
{
    public class Program
    {
		private string ReadTestData(string fileName)
		{
			var testFileFolder = "Game.OtoGe.MusicXML.Tests.TestData";
			var asm = Assembly.GetExecutingAssembly();
			string text = "";
			using (var sr = new StreamReader(asm.GetManifestResourceStream($"{testFileFolder}.{fileName}"), Encoding.UTF8))
			{
				text = sr.ReadToEnd();
			}
			return text;
		}

		[Fact]
		public void Test_Tempo()
		{
			var testFileName = "test_tempo.xml";
			string text = ReadTestData(testFileName);

			//do
			var score = MusicXMLParser.GetScorePartwise(text);

			Assert.Equal(80, score.Tempo);
		}

		[Fact]
		public void Test()
		{
			var testFileName = "MusicXML1.xml";
			string text = ReadTestData(testFileName);

			//do
			var score = MusicXMLParser.GetScorePartwise(text);

			Assert.Equal(4, score.ScoreParts.Count);
			{
				var part1 = score.ScoreParts.First();
				Assert.Equal("P1", part1.Id);
				Assert.Equal(2, part1.MeasureList.Length);
				var attr = part1.Attribute;
				Assert.Equal(1, attr.Divisions);
				Assert.Equal(4, attr.Time.Beats);
				Assert.Equal(4, attr.Time.BeatType);
				var note1 = part1.MeasureList.First().Notes[0];
				Assert.False(note1.IsUnpitch);
			}
			{
				var part2 = score.ScoreParts[1];
				var measure1 = part2.MeasureList[0];
				Assert.Equal(1, measure1.Number);
				Assert.Equal(4, measure1.Notes.Length);
				var note1 = measure1.Notes[0];
				Assert.False(note1.IsRest);
				Assert.True(note1.IsUnpitch);
				Assert.Equal(1, note1.Duration);
				Assert.Equal(ScoreNoteType.quarter, note1.Type);

				var note2 = measure1.Notes[1];
				Assert.True(note2.IsRest);
			}
		}


		[Fact]
		public void Test_Fermata()
		{
			var testFileName = "test1.xml";
			string text = ReadTestData(testFileName);

			//do
			var score = MusicXMLParser.GetScorePartwise(text);

			//フェルマータの付いた休符かどうか
			var note1 = score.ScoreParts[0].MeasureList[0].Notes[0];
			Assert.False(note1.HasFermata);

			var note2 = score.ScoreParts[0].MeasureList[0].Notes[1];
			Assert.True(note2.HasFermata);
		}

		[Fact]
		public void Test_Huten_16th()
		{
			//付点、16分音符のテスト
			var testFileName = "test1_huten.xml";
			string text = ReadTestData(testFileName);

			//do
			var score = MusicXMLParser.GetScorePartwise(text);
			var measure = score.ScoreParts[0].MeasureList[0];
			{
				var note = measure.Notes[0];
				//付点8分音符か
				Assert.Equal(ScoreNoteType.eighth, note.Type);
				Assert.False(note.IsRest);
				Assert.True(note.HasDot);
			}
			{
				var note = measure.Notes[1];
				//16分休符か
				Assert.Equal(ScoreNoteType.sixteenth, note.Type);
				Assert.True(note.IsRest);
				Assert.False(note.HasDot);
			}
			{
				var note = measure.Notes[2];
				//付点8分休符か
				Assert.Equal(ScoreNoteType.eighth, note.Type);
				Assert.True(note.IsRest);
				Assert.True(note.HasDot);
			}
			{
				var note = measure.Notes[3];
				//16分音符か
				Assert.Equal(ScoreNoteType.sixteenth, note.Type);
				Assert.False(note.IsRest);
				Assert.False(note.HasDot);
			}
		}
	}

}

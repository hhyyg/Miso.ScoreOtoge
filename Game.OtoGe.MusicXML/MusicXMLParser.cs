using Game.OtoGe.MusicXML.Poco;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Linq;

namespace Game.OtoGe.MusicXML
{
    public static class MusicXMLParser
    {
		public static Score GetScorePartwise(string text)
		{
			var doc = new XmlDocument();
			doc.LoadXml(text);

			var score = new Score();
			score.ScoreParts = GetScoreParts(doc);

			foreach(var scorePart in score.ScoreParts)
			{
				scorePart.Attribute = GetAttribute(scorePart.Id, doc);
				scorePart.MeasureList = GetMeasures(scorePart.Id, doc).ToArray();
			}

			score.Tempo = GetScoreTempo(doc);

			return score;
		}

		private static int GetScoreTempo(XmlDocument doc)
		{
			//get first tempo
			//not support multiple tempo
			var soundElements = doc.SelectNodes("score-partwise/part/measure/direction/sound");
			if (soundElements == null)
			{
				throw new Exception("not found sound");
			}
			var tempo = ((IEnumerable)soundElements).Cast<XmlNode>().First().Attributes["tempo"].InnerText;
			return Int32.Parse(tempo);
		}

		private static IEnumerable<Measure> GetMeasures(string partId, XmlDocument doc)
		{
			var measureNodes = doc.SelectNodes($"score-partwise/part[@id='{partId}']/measure");

			foreach (XmlNode measureNode in measureNodes)
			{
				var measure = new Measure();
				measure.Number = Int64.Parse(measureNode.Attributes["number"].InnerText);
				measure.Notes = GetNotes(measureNode).ToArray();

				yield return measure;
			}
		}

		private static IEnumerable<ScoreNote> GetNotes(XmlNode measureNode)
		{
			//note
			var noteNodes = measureNode.SelectNodes("note");

			foreach (XmlNode noteNode in noteNodes)
			{
				yield return new ScoreNote()
				{
					IsUnpitch = (noteNode.SelectSingleNode("pitch") == null),
					Duration = Int32.Parse(noteNode.SelectSingleNode("duration").InnerText),
					Type = GetScoreNoteType(noteNode.SelectSingleNode("type").InnerText),
					IsRest = (noteNode.SelectSingleNode("rest") != null),
					HasFermata = noteNode.SelectSingleNode("notations/fermata") != null,
					HasDot = noteNode.SelectSingleNode("dot") != null,
				};
			}
		}

		private static ScoreNoteType GetScoreNoteType(string text)
		{
			switch (text)
			{
				case "quarter":
					return ScoreNoteType.quarter;
				case "eighth":
					return ScoreNoteType.eighth;
				case "16th":
					return ScoreNoteType.sixteenth;
				default:
					throw new NotSupportedException($"cannot convert to NodeType. note type value is:{text}");
			}
		}

		private static Poco.Attribute GetAttribute(string partId, XmlDocument doc)
		{
			//attribute
			//attributeは、measure配下に存在するが、ここでは、最初のmeasureのattribute情報をpartのものとする
			//(つまり、小節の途中で拍子が変わった場合に対応できない)
			var attributeNode = doc.SelectSingleNode($"score-partwise/part[@id='{partId}']/measure[1]/attributes");
			var attribute = new Poco.Attribute();
			attribute.Divisions = int.Parse(attributeNode.SelectSingleNode("divisions").InnerText);
			var timeNode = attributeNode.SelectSingleNode("time");
			attribute.Time = new Time()
			{
				Beats = Int32.Parse(timeNode.SelectSingleNode("beats").InnerText),
				BeatType = Int32.Parse(timeNode.SelectSingleNode("beat-type").InnerText),
			};

			return attribute;
		}

		private static List<ScorePart> GetScoreParts(XmlDocument doc)
		{
			var scorePartNodes = doc.SelectNodes("score-partwise/part-list/score-part");

			var scoreParts = new List<ScorePart>();
			foreach (XmlNode scorePartNode in scorePartNodes)
			{
				var partName = scorePartNode.SelectSingleNode("part-name").InnerText;
				if (partName.StartsWith("part"))
				{
					var partId = scorePartNode.Attributes["id"].InnerText;
					scoreParts.Add(new ScorePart() { Id = partId });
				}
			}
			return scoreParts;
		}
    }
}

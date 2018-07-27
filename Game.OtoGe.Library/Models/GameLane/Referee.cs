using Game.OtoGe.Library.MusicXML;
using System;

namespace Game.OtoGe.Library.Models.GameLane
{
	public interface IReferee
	{
		/// <summary>
		/// 判定する
		/// </summary>
		/// <param name="note"></param>
		/// <returns></returns>
		JudgeResult Judge(GameNote note);
		/// <summary>
		/// 判定エリアの左のラインを過ぎているかどうか
		/// </summary>
		/// <returns></returns>
		bool IsOverJudgeAreaLeft(GameNote note);
		/// <summary>
		/// 画面左端を超えたか
		/// </summary>
		/// <param name="note"></param>
		/// <returns></returns>
		bool IsOverVisibleAreaLeft(GameNote note);
	}

	public class Referee : IReferee
	{
		public Referee(
			float judgeLineX)
		{
			_judgeLineX = judgeLineX;
			_judgeAreaLeftX = _judgeLineX - judgeAreaWidth_Half;
			_judgeAreaRightX = _judgeLineX + judgeAreaWidth_Half;
		}

		private TimeManager _timeManager = TimeManager.Instance;
		/// <summary>
		/// 判定エリアの横幅の半分
		/// </summary>
		private const float judgeAreaWidth_Half = 150 / 2;
		// <summary>
		/// 判定エリアの中心
		/// </summary>
		private float _judgeLineX = 0;
		/// <summary>
		/// 判定エリアの左
		/// </summary>
		private float _judgeAreaLeftX = 0;
		/// <summary>
		/// 判定エリアの右
		/// </summary>
		private float _judgeAreaRightX = 0;
		/// <summary>
		/// 画面左端X
		/// </summary>
		private float _visibleLeftX = -Const.BoardWidthHalf - 100;//TODO:猶予

		public JudgeResult Judge(GameNote note)
		{
			//判定エリアの中心からズレを取得する
			var diff = (GetAbsoluteX(note) - _judgeLineX);
			var diffAbs = Math.Abs(diff);

			Console.WriteLine($"diff:{diff}");

			var type = JudgeResultType.Ignore;
			if (diffAbs < 50)
				type = JudgeResultType.Success;
			else if (diffAbs < 100)
				type = JudgeResultType.Good;
			else if (diffAbs < 150)
				type = JudgeResultType.Miss;

			return new JudgeResult(type, diff);
		}

		public bool IsOverJudgeAreaLeft(GameNote note)
		{
			var noteAbsX = GetAbsoluteX(note);
			return noteAbsX < _judgeAreaLeftX;
		}

		/// <summary>
		/// 音符の絶対座標を取得する
		/// </summary>
		/// <param name="note"></param>
		/// <returns></returns>
		private float GetAbsoluteX(GameNote note)
		{
			//Boardは左に動いているので、現在のBoardの座標X＋音符の相対座標X
			var boardX = (Const.JudgeLineX - _timeManager.Distance);
			return boardX + note.X;
		}

		public bool IsOverVisibleAreaLeft(GameNote note)
		{
			return GetAbsoluteX(note) < _visibleLeftX;
		}
	}
}

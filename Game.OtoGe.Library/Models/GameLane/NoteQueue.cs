using Game.OtoGe.Library.MusicXML;
using System.Collections.Generic;
using System.Diagnostics;

namespace Game.OtoGe.Library.Models.GameLane
{
	public interface INoteQueue
	{
		void Enqueue(GameNote note);
		JudgeResult Judge();

		/// <summary>
		/// 判定を要する要素
		/// </summary>
		GameNote? JudgeHead { get; }
	}

	public class NoteQueue: INoteQueue
	{
		public NoteQueue(int max)
		{
			_max = max;
			_notes = new GameNote[max];
		}

		private int _max = 0;
		private GameNote[] _notes = null;
		private TimeManager _timeManager = TimeManager.Instance;
		private IReferee _referee = new Referee(judgeLineX: Const.JudgeLineX);

		/// <summary>
		/// 判定を要する要素の先頭Index
		/// </summary>
		private int _judgeHead = 0;
		/// <summary>
		/// 画面左端を越えていない要素の先頭Index
		/// </summary>
		private int _visibleHead = 0;
		/// <summary>
		/// Enqeueのときに入れる場所Index(=Length)
		/// </summary>
		private int _tail = 0;

		public GameNote? JudgeHead
		{
			get
			{
				if (_tail == 0)
					return null;
				return _notes[_judgeHead];
			}
		}
		public void Enqueue(GameNote note)
		{
			//TODO:note.Tickが重複するのはありえない
			var tail = _tail;
			_notes[tail] = note;
			_tail += 1;
		}

		/// <summary>
		/// 判定するべき要素を判定する
		/// </summary>
		/// <returns></returns>
		public JudgeResult Judge()
		{
			var head = _judgeHead;
			while (_referee.IsOverJudgeAreaLeft(_notes[head]))
			{
				//判定エリアより左側だったら次の音符を見る
				head += 1;

				if (_tail <= head)
				{
					//末尾以降を参照しようとしたとき
					return JudgeResult.Ignore;
				}

				_judgeHead = head;
			}
			//判定する
			return _referee.Judge(_notes[head]);
		}

		private void UpdateVisibleHead()
		{
			var head = _visibleHead;
			while(_referee.IsOverVisibleAreaLeft(_notes[head]))
			{
				head += 1;
				_visibleHead = head;
			}
		}
	}
	
}

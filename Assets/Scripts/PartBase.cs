using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Game.OtoGe.Library;
using Game.OtoGe.Library.Models;
using Game.OtoGe.Library.Models.GameLane;
using Game.OtoGe.Library.MusicXML;

namespace Assets.Scripts
{
	public abstract class PartBase: MonoBehaviour
	{
		/// <summary>
		/// パートに対応するエリア
		/// </summary>
		protected AreaBase targetArea { get; set; }
		public INoteQueue NoteQueue { get; set; }

		protected virtual void Start()
		{
		}

		/// <summary>
		/// TouchBarからタッチしたときに呼ばれる
		/// </summary>
		public virtual void OnTouch()
		{
			if (NoteQueue.JudgeHead == null)
			{
				Debug.Log("音符が一個もない");
				return;
			}
			var judgeResult = NoteQueue.Judge();
			var judgedNote = NoteQueue.JudgeHead.Value;

			if (judgedNote.IsRest)
			{
				//休符は無視
				return;
			}

			Debug.Log($"{judgeResult.Type}, diff:{judgeResult.Diff}, noteTick:{NoteQueue.JudgeHead?.Tick}");

			switch (judgeResult.Type)
			{
				case JudgeResultType.Ignore:
					targetArea.PopupPointText(judgeResult);
					return;
				default:
					targetArea.PopupPointText(judgeResult);

					var target = Find(judgedNote);
					if (target != null) //連打するともう消えていることがある
					{
						Destroy(target);
					}
						
					break;
			}
		}

		private GameObject Find(GameNote note)
		{
			//TODO: 重そう
			var obj = transform.Find(note.Tick.ToString());
			return obj?.gameObject;
		}

		private float GetAbsolutePositionX(Transform trans)
		{
			var myX = trans.localPosition.x;
			Transform parent = trans.parent;
			while(parent != null)
			{
				myX += parent.localPosition.x;

				parent = parent.parent;
			}
			return myX;
		}
	}
}

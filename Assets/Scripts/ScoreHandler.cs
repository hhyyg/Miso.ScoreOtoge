using Game.OtoGe.Library;
using Game.OtoGe.Library.Models;
using Game.OtoGe.Library.MusicXML;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 音符
/// </summary>
public class ScoreHandler : MonoBehaviour
{
	public bool	IsRest { get; set; }
	public GameNote GameNote { get; set; }

	private string methodName_whenOutDestroy = nameof(WhenOutDestroy);
	private TimeManager _timeManager = TimeManager.Instance;

	private void Start()
	{
		Invoke(methodName_whenOutDestroy, 10);
	}

	private void Update()
	{
		if (!IsRest)
			return;

		//休符の場合、ジャッジラインを過ぎたら消す
		var absX = GetAbsoluteX(GameNote);
		if ((Const.JudgeLineX - 10) <= absX &&
			absX <= (Const.JudgeLineX + 10))
		{
			Destroy(transform.gameObject);
		}
	}

	//TODO: 下記コピペになってる

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

	/// <summary>
	/// 時間になったら消す
	/// </summary>
	private void WhenOutDestroy()
	{
		Destroy(gameObject);
	}
}

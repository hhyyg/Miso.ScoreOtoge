using Game.OtoGe.Library.Models;
using Game.OtoGe.Library.Models.GameLane;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public abstract class AreaBase : MonoBehaviour
{
	[SerializeField]
	private GameObject ringPrefab;

	[SerializeField]
	private GameObject perfectTextPrefab;
	[SerializeField]
	private GameObject goodTextPrefab;
	[SerializeField]
	private GameObject missTextPrefab;

	[SerializeField]
	private Text diffText;

	private SEManager soundManager;
	private AudioSource audioSource;
	private bool enableDiffView = true;

	protected virtual void Start()
	{
		soundManager = FindObjectOfType<SEManager>();
		audioSource = soundManager.GetComponent<AudioSource>();
	}

	/// <summary>
	/// TouchBarからタッチしたときに呼ばれる
	/// </summary>
	public void OnTouch()
	{
		//タッチリングの表示
		var ring = Instantiate(ringPrefab);
		ring.transform.SetParent(transform);
		ring.transform.position = transform.position;
		ring.transform.localScale = Vector3.zero;

		//アニメ再生
		ring.GetComponent<Animator>().Play(0);
	}

	/// <summary>
	/// 判定結果テキストを表示する
	/// </summary>
	/// <param name="text"></param>
	public void PopupPointText(JudgeResult judgeResult)
	{
		if (enableDiffView)
			ViewDiff(judgeResult);

		if (judgeResult.Type == JudgeResultType.Ignore)
			return;

		audioSource.PlayOneShot(soundManager.TapAudio);

		GameObject prefab = null;
		if (judgeResult.Type == JudgeResultType.Success)
		{
			prefab = Instantiate(perfectTextPrefab);
		}
		else if (judgeResult.Type == JudgeResultType.Good)
		{
			prefab = Instantiate(goodTextPrefab);
		}
		else if (judgeResult.Type == JudgeResultType.Miss)
		{
			prefab = Instantiate(missTextPrefab);
		}
		else
		{
			return;
		}
		prefab.transform.SetParent(transform);
		prefab.transform.position = transform.position;
		//TODO: 固定？
		prefab.transform.localScale = new Vector3(120, 120, 0);

		prefab.GetComponent<Animator>().Play(0);
	}

	private void ViewDiff(JudgeResult judgeResult)
	{
		if (judgeResult.Diff > 0)
		{
			//はやい
			diffText.text = "early " + judgeResult.Diff.ToString("0");
		}
		else if (judgeResult.Diff == 0)
		{
			diffText.text = judgeResult.Diff.ToString("0");
		}
		else
		{
			diffText.text = "late " + judgeResult.Diff.ToString("0");
		}
		
	}
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.OtoGe.Library;
using Game.OtoGe.Library.Infra;
using Game.OtoGe.Library.Models;
using Game.OtoGe.Library.MusicXML;
using Game.OtoGe.Library.Models.GameLane;
using Game.OtoGe.MusicXML.Poco;

/// <summary>
/// ボード
/// </summary>
/// <remarks>
/// 時間と移動を担当する
/// </remarks>
public class BoardManager: MonoBehaviour {
	
	//16拍前から音符を生成
	const long _prepareTicks = 16 * 480;

	private readonly TimeManager _timeManager = TimeManager.Instance;
	private BoardCreator _boardCreator = new BoardCreator();
	private AudioSource _audioSource;
	private SpriteManager _spriteManager;

	private Transform part1;
	private Transform part2;
	private Transform part3;

	public BoardCreator BoardCreator { get { return _boardCreator; } }
	
	/// <summary>
	/// 曲のJsonデータ
	/// </summary>
	[SerializeField]
	private TextAsset _musicXMLData;
	/// <summary>
	/// 音符プレハブ
	/// </summary>
	[SerializeField]
	private GameObject notePrefab;

	[SerializeField]
	private GameObject noteRestPrefab;

	void Start()
	{
		_spriteManager = FindObjectOfType<SpriteManager>();
		_audioSource = GetComponent<AudioSource>();

		_timeManager.Time = 0;
		_timeManager.Tick = 0;

		//この時点で曲が確定している
		using (var watch = new StopwatchDisposable())
		{
			_boardCreator.Setup(musicXmlText: _musicXMLData.text);
		}
			
		_timeManager.Tempo = _boardCreator.GameScore.Tempo ?? Const.DefaultTempo;

		StartParts();
		_audioSource.Play();

		Debug.Log($"NoteXAdjustment:{Const.NoteXAdjustment}, move:{Const.MovePointInTick}");
	}

	private void StartParts()
	{
		part1 = transform.Find("Part1");
		part2 = transform.Find("Part2");
		part3 = transform.Find("Part3");

		part1.GetComponent<Part1Script>().NoteQueue = _boardCreator.NoteQueues[0];
		part2.GetComponent<Part2Script>().NoteQueue = _boardCreator.NoteQueues[1];
		part3.GetComponent<Part3Script>().NoteQueue = _boardCreator.NoteQueues[2];
	}
	
	// Update is called once per frame
	void Update()
	{
		UpdateTime();

		if (BoardCreator.SetupCompleted)
			StartCoroutine(UpdateScore());

		if (!_audioSource.isPlaying)
		{
			Debug.Log("Clear");
		}
	}

	private void UpdateTime()
	{
		//時間を更新
		_timeManager.Time = _audioSource.time;

		//今までどれくらいTickが溜まったかを記録する
		//1拍を480分割したものが 1 Tick, 1/480 Tempo = 1Tick 
		//Tempo 1分間に何拍うてるか, 1分=1 Tempo, 1秒=1/60 Tempo
		//1拍 = time * 1/60 * tempo 
		//1 Tick = time * 1/60 * tempo * 480
		_timeManager.Tick = (_timeManager.Time * _timeManager.Tempo * 480f / 60f);
		//移動した距離を記録しておく
		_timeManager.Distance = _timeManager.Tick * Const.MovePointInTick;
		
		//UI-----
		//ボードは常に左に動く
		transform.localPosition = new Vector3(Const.JudgeLineX - _timeManager.Distance, 0, 0);
	}

	private IEnumerator UpdateScore()
	{
		int partNum = 0;
		foreach (var part in BoardCreator.GameScore.Parts)
		{
			partNum++;

			int noteIndex = -1;
			foreach (GameNote note in part.Notes)
			{
				noteIndex++;

				if (note.IsCreated)
				{
					continue;
				}
				if (note.Tick < (_timeManager.Tick + _prepareTicks))
				{
					Tuple<Transform, INoteQueue> partObj = GetPart(partNum);
					partObj.Item2.Enqueue(note);

					var prefab = note.IsRest ? noteRestPrefab : notePrefab;
					GameObject noteObj = Instantiate(prefab);

					//config handler
					var handler = noteObj.GetComponent<ScoreHandler>();
					handler.IsRest = note.IsRest;
					handler.GameNote = note;

					//setimage
					SetNoteImage(noteObj, note);

					//settings
					noteObj.transform.SetParent(partObj.Item1);
					noteObj.transform.localPosition = new Vector3(note.X, 0, 0);
					noteObj.transform.localScale = prefab.transform.localScale;
					noteObj.name = note.Tick.ToString();

					part.Notes[noteIndex].IsCreated = true;
					yield return null;
				}
			}
		}
	}

	private void SetNoteImage(GameObject noteObj, GameNote note)
	{
		//setimage
		var imageComponent = noteObj.GetComponent<UnityEngine.UI.Image>();

		switch (note.NoteType)
		{
			case ScoreNoteType.quarter:
				if (note.HasDot)
				{
					imageComponent.sprite = note.IsRest ? _spriteManager.Quarter_Rest_Dot : _spriteManager.Quarter_Dot;
					break;
				}
				imageComponent.sprite = note.IsRest ? _spriteManager.Quarter_Rest : _spriteManager.Quarter;
				break;
			case ScoreNoteType.eighth:
				if (note.HasDot)
				{
					imageComponent.sprite = note.IsRest ? _spriteManager.Eighth_Rest_Dot : _spriteManager.Eighth_Dot;
					break;
				}
				imageComponent.sprite = note.IsRest ? _spriteManager.Eighth_Rest : _spriteManager.Eighth;
				break;
			case ScoreNoteType.sixteenth:
				if (note.HasDot)
				{
					imageComponent.sprite = note.IsRest ? _spriteManager.Sixteen_Rest_Dot : _spriteManager.Sixteen_Dot;
					break;
				}
				imageComponent.sprite = note.IsRest ? _spriteManager.Sixteen_Rest : _spriteManager.Sixteen;
				break;
		}
	}

	private Tuple<Transform, INoteQueue> GetPart(int num)
	{
		if (num == 1)
			return Tuple.Create(part1, _boardCreator.NoteQueues[0]);
		else if (num == 2)
			return Tuple.Create(part2, _boardCreator.NoteQueues[1]);
		else
			return Tuple.Create(part3, _boardCreator.NoteQueues[2]);
	}
}

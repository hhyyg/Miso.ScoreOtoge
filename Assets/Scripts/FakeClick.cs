using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FakeClick : MonoBehaviour {

	[SerializeField]
	private GameObject _buttonPart1;
	[SerializeField]
	private GameObject _buttonPart2;
	[SerializeField]
	private GameObject _buttonPart3;

	void Update ()
	{
		if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			ExecuteEvents.Execute<IPointerClickHandler>(_buttonPart1, new PointerEventData(EventSystem.current), ExecuteEvents.pointerClickHandler);
		}

		if (Input.GetKeyDown(KeyCode.DownArrow))
		{
			ExecuteEvents.Execute<IPointerClickHandler>(_buttonPart2, new PointerEventData(EventSystem.current), ExecuteEvents.pointerClickHandler);
		}

		if (Input.GetKeyDown(KeyCode.RightArrow))
		{
			ExecuteEvents.Execute<IPointerClickHandler>(_buttonPart3, new PointerEventData(EventSystem.current), ExecuteEvents.pointerClickHandler);
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
	/// <summary>
	/// アニメーションイベントから自身を消すために呼ばれる
	/// </summary>
	public void OnAnimationStop()
	{
		Destroy(gameObject);
	}

}

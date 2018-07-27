using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ゲーム管理
/// </summary>
public class GameManager : MonoBehaviour
{
	private void Start()
	{
		UnitySystemConsoleRedirector.Redirect();
	}
}

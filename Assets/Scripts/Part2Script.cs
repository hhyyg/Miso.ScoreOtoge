﻿using Assets.Scripts;
using Game.OtoGe.Library.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Part2Script : PartBase
{
	protected override void Start()
	{
		base.Start();
		targetArea = GameObject.FindObjectOfType<Area2>();
	}
}

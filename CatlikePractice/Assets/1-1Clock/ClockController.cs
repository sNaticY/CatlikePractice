using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockController : MonoBehaviour
{
	[SerializeField] private Transform _hourArm;
	[SerializeField] private Transform _minuteArm;
	[SerializeField] private Transform _secondArm;

	// Use this for initialization
	void Start()
	{
		Debug.Log(DateTime.Now.Hour);
		Debug.Log(DateTime.Now.Minute);
		Debug.Log(DateTime.Now.Second);
		Debug.Log(DateTime.Now.Millisecond);
	}

	// Update is called once per frame
	void Update()
	{
		var hour = DateTime.Now.Hour;
		var minute = DateTime.Now.Minute;
		var second = DateTime.Now.Second;
		var milisecond = DateTime.Now.Millisecond;
		_hourArm.localEulerAngles = new Vector3(0, hour * 30 + minute  / 60f * 30f, 0);
		_minuteArm.localEulerAngles = new Vector3(0, minute * 6 + second / 60f * 6f, 0);
		_secondArm.localEulerAngles = new Vector3(0, second * 6 + milisecond / 1000f * 6f, 0);
	}
}

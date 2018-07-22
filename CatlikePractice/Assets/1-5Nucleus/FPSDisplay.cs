using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
struct FPSColor {
	public Color color;
	public int minimumFPS;
}


public class FPSDisplay : MonoBehaviour
{
	[SerializeField] private Text _lowFpsText;
	[SerializeField] private Text _averageFpsText;
	[SerializeField] private Text _highFpsText;
	[SerializeField] private int _fpsRange = 60;

	[SerializeField] private FPSColor[] _fpsColors;
	
	private int[] _fpsBuffer;
	private int _fpsBufferIndex;

	private int _lowFps;
	private int _averageFps;
	private int _highFps;

	private static readonly List<string> _fpsStrings = new List<string>
	{
		"00", "01", "02", "03", "04", "05", "06", "07", "08", "09",
		"10", "11", "12", "13", "14", "15", "16", "17", "18", "19",
		"20", "21", "22", "23", "24", "25", "26", "27", "28", "29",
		"30", "31", "32", "33", "34", "35", "36", "37", "38", "39",
		"40", "41", "42", "43", "44", "45", "46", "47", "48", "49",
		"50", "51", "52", "53", "54", "55", "56", "57", "58", "59",
		"60", "61", "62", "63", "64", "65", "66", "67", "68", "69",
		"70", "71", "72", "73", "74", "75", "76", "77", "78", "79",
		"80", "81", "82", "83", "84", "85", "86", "87", "88", "89",
		"90", "91", "92", "93", "94", "95", "96", "97", "98", "99"
	};

	private void Awake()
	{
		_fpsBuffer = new int[_fpsRange];
	}

	// Update is called once per frame
	private void Update()
	{
		UpdateBuffer();
		CalcFps();
		DisplayFps(_lowFpsText, _lowFps);
		DisplayFps(_averageFpsText, _averageFps);
		DisplayFps(_highFpsText, _highFps);
	}

	private void UpdateBuffer()
	{
		var curFps = (int) (1f / Time.unscaledDeltaTime);
		_fpsBuffer[_fpsBufferIndex] = curFps;
		_fpsBufferIndex++;
		if (_fpsBufferIndex >= _fpsRange)
		{
			_fpsBufferIndex = 0;
		}
	}


	private void CalcFps()
	{
		var lowest = int.MaxValue;
		var highest = 0;
		var sum = 0;
		foreach (var fps in _fpsBuffer)
		{
			if (fps < lowest)
			{
				lowest = fps;
			}

			if (fps > highest)
			{
				highest = fps;
			}
			sum += fps;
		}

		_lowFps = lowest;
		_averageFps = sum / _fpsRange;
		_highFps = highest;
	}

	private void DisplayFps(Text label, int fps)
	{
		label.text = _fpsStrings[Mathf.Clamp(fps, 0, 99)];
		for (var i = 0; i < _fpsColors.Length; i++) {
			if (fps < _fpsColors[i].minimumFPS) continue;
			label.color = _fpsColors[i].color;
			break;
		}
	}
	
}

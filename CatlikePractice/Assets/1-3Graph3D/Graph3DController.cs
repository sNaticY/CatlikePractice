using System;
using System.Collections;
using Boo.Lang;
using UnityEngine;

public delegate Vector3 Function(float u, float v, float t);

public enum GraphFunctionName {
	Sine,
	MultiSine,
	Sine2D,
	MultiSine2D,
	Ripple,
}

public class Graph3DController : MonoBehaviour
{
	[Range(10, 100), SerializeField] private int _resolution;
	[SerializeField] private GameObject _cube;
	[SerializeField] public GraphFunctionName _function;

	[SerializeField] private float _amplitude = 3;
	[SerializeField] private float _frequency = 4;
	[SerializeField] private float _velocity = 2;
	[SerializeField] private float _attenuation = 6;

	private List<List<Transform>> _points;
	private float _step;

	private Function[] _functions;

	// Use this for initialization
	private void Start()
	{
		_functions = new Function[] {SineFunction, MultiSineFunction, Sine2DFunction, MultiSine2DFunction, Ripple};

		_cube.SetActive(false);
		_points = new List<List<Transform>>();
		_step = 2f / _resolution;

		var scale = Vector3.one * _step;

		for (int i = 0; i < _resolution; i++)
		{
			_points.Add(new List<Transform>());
			for (int j = 0; j < _resolution; j++)
			{
				var point = Instantiate(_cube, transform);
				_points[i].Add(point.transform);
				point.transform.localScale = scale;
				point.SetActive(true);
			}
		}

	}

	private void Update()
	{
		for (int i = 0; i < _points.Count; i++)
		{
			for (int j = 0; j < _points[i].Count; j++)
			{
				var u = i * _step - 1;
				var v = j * _step - 1;
				var point = _points[i][j];
				point.localPosition = _functions[(int) _function](u, v, Time.time);
			}
		}
	}

	private Vector3 SineFunction(float u, float v, float t)
	{
		var x = u;
		var y = Mathf.Sin(Mathf.PI * (u + t));
		var z = v;
		return new Vector3(x, y, z);
	}

	private Vector3 MultiSineFunction(float u, float v, float t)
	{
		var x = u;
		float y = Mathf.Sin(Mathf.PI * (u + t));
		y += Mathf.Sin(2f * Mathf.PI * (u + 2f * t)) / 2f;
		y *= 2f / 3f;
		var z = v;
		return new Vector3(x, y, z);
	}

	private Vector3 Sine2DFunction(float u, float v, float t)
	{
		var x = u;
		float y = Mathf.Sin(Mathf.PI * (u + t));
		y += Mathf.Sin(Mathf.PI * (v + t));
		y *= 0.5f;
		var z = v;
		return new Vector3(x, y, z);
	}

	private Vector3 MultiSine2DFunction(float u, float v, float t)
	{
		var x = u;
		float y = 4f * Mathf.Sin(Mathf.PI * (u + v + t * 0.5f));
		y += Mathf.Sin(Mathf.PI * (u + t));
		y += Mathf.Sin(2f * Mathf.PI * (v + 2f * t)) * 0.5f;
		y *= 1f / 5.5f;
		var z = v;
		return new Vector3(x, y, z);
	}

	private Vector3 Ripple(float u, float v, float t)
	{
		var x = u;
		float d = Mathf.Sqrt(u * u + v * v);
		float y = Mathf.Sin(_frequency * Mathf.PI * (d - t / _velocity));
		y *= 1 / (_amplitude + _attenuation * 2 * Mathf.PI * d);
		var z = v;
		return new Vector3(x, y, z);
	}
}

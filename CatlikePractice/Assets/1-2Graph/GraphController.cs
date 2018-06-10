using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphController : MonoBehaviour
{
	[Range(10, 100), SerializeField] private int _resolution;
	[SerializeField] private GameObject _cube;
	
	private List<Transform> _points;
	private float _step;
	private float _startX;
	
	// Use this for initialization
	private void Start ()
	{
		_cube.SetActive(false);
		_points = new List<Transform>();
		_step = 2f / _resolution;
		
		var scale = Vector3.one * _step;
		
		for (int i = 0; i < _resolution; i++)
		{
			var point = Instantiate(_cube, transform);
			_points.Add(point.transform);
			point.transform.localScale = scale;
			point.SetActive(true);
		}
		
	}

	private void Update()
	{
		_startX = -1f;
		for (int i = 0; i < _resolution; i++)
		{
			var x = _startX + i * _step;
			var pos = new Vector3(x, Calc(x), 0);
			var point = _points[i];
			point.transform.localPosition = pos;
		}
	}

	private float Calc(float x)
	{
		return Mathf.Sin(Mathf.PI * (x + Time.time));
	}
}

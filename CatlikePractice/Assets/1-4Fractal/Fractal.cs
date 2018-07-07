using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class Fractal : MonoBehaviour
{
	public Mesh[] Mesh;
	public Material Material;

	public int Depth;
	public float Size;

	public float Probability;

	public float MaxRotateSpeed;

	public float RotateSpeedChangeRate;

	private float RotateSpeed;

	private readonly Vector3[] _positions = {Vector3.left, Vector3.right, Vector3.up, Vector3.down, Vector3.forward};
	private readonly Vector3[] _rotations = {Vector3.down, Vector3.up, Vector3.left, Vector3.right, Vector3.zero};
	
	// Use this for initialization
	private void Start ()
	{
		gameObject.AddComponent<MeshFilter>().mesh = Mesh[Random.Range(0, Mesh.Length)];
		gameObject.AddComponent<MeshRenderer>().material = Material;
		if (Depth <= 0) return;
		var posOffset = Size + Size / 2f;
		for (int i = 0; i < 5; i++)
		{
			if (Random.Range(0, 1f) <= Probability)
			{
				new GameObject("Fractal Child").AddComponent<Fractal>().Initialize(this, Size, _positions[i] * posOffset, _rotations[i] * 90);
			}
		}
	}

	private void Update()
	{
		Random.InitState(Depth * (int)Mathf.Ceil(Time.time * 100));
		if (Random.Range(0, 1f) <= RotateSpeedChangeRate)
		{
			RotateSpeed = Random.Range(-MaxRotateSpeed, MaxRotateSpeed);
		}
		transform.Rotate(0f, 0f,  RotateSpeed * Time.deltaTime);
	}

	public void Initialize(Fractal parent, float size, Vector3 pos, Vector3 rot)
	{
		Mesh = parent.Mesh;
		Material = parent.Material;
		Depth = parent.Depth - 1;
		Size = size;
		Probability = parent.Probability;
		MaxRotateSpeed = parent.MaxRotateSpeed;
		RotateSpeedChangeRate = parent.RotateSpeedChangeRate;
		transform.SetParent(parent.transform);
		transform.localPosition = pos;
		transform.localEulerAngles = rot;
		transform.localScale = Vector3.one * size;
	}
}

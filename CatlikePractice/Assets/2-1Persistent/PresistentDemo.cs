using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PresistentDemo : PersistableObject
{
	public PersistableObject Prefab;

	public KeyCode CreateKey = KeyCode.C;
	public KeyCode NewGameKey = KeyCode.N;
	public KeyCode SaveKey = KeyCode.S;
	public KeyCode LoadKey = KeyCode.L;

	private List<PersistableObject> _objectList;

	public PersistentStorage Storage;

	private void Awake()
	{
		_objectList = new List<PersistableObject>();
	}

	private void Update()
	{
		if (Input.GetKeyDown(CreateKey))
		{
			CreateObject();
		}
		else if (Input.GetKey(NewGameKey))
		{
			BeginNewGame();
		}
		else if (Input.GetKeyDown(SaveKey))
		{
			Storage.Save(this);
		}
		else if (Input.GetKeyDown(LoadKey))
		{
			BeginNewGame();
			Storage.Load(this);
		}
	}

	private void CreateObject()
	{
		PersistableObject o = Instantiate(Prefab);
		var t = o.transform;
		t.localPosition = Random.insideUnitSphere * 5f;
		t.localRotation = Random.rotation;
		t.localScale = Vector3.one * Random.Range(0.1f, 1f);
		_objectList.Add(o);
	}

	private void BeginNewGame()
	{
		for (int i = 0; i < _objectList.Count; i++)
		{
			Destroy(_objectList[i].gameObject);
		}

		_objectList.Clear();
	}

	public override void Save(GameDataWriter writer)
	{
		writer.Write(_objectList.Count);
		for (int i = 0; i < _objectList.Count; i++)
		{
			_objectList[i].Save(writer);
		}
	}

	public override void Load(GameDataReader reader)
	{
		int count = reader.ReadInt();
		for (int i = 0; i < count; i++)
		{
			PersistableObject o = Instantiate(Prefab);
			o.Load(reader);
			_objectList.Add(o);
		}
	}
}
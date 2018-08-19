using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Reusing
{
	public class ReusingDemo : PersistableObject
	{
		public ShapeFactory shapeFactory;

		public KeyCode CreateKey = KeyCode.C;
		public KeyCode destroyKey = KeyCode.X;
		public KeyCode NewGameKey = KeyCode.N;
		public KeyCode SaveKey = KeyCode.S;
		public KeyCode LoadKey = KeyCode.L;

		private List<Shape> _objectList;

		private int _saveVersion = 1;

		public PersistentStorage Storage;

		private void Awake()
		{
			_objectList = new List<Shape>();
		}

		private void Update()
		{
			if (Input.GetKeyDown(CreateKey))
			{
				CreateObject();
			}
			else if (Input.GetKeyDown(destroyKey)) {
				DestroyObject();
			}
			else if (Input.GetKey(NewGameKey))
			{
				BeginNewGame();
			}
			else if (Input.GetKeyDown(SaveKey))
			{
				Storage.Save(this, _saveVersion);
			}
			else if (Input.GetKeyDown(LoadKey))
			{
				BeginNewGame();
				Storage.Load(this);
			}
		}

		private void CreateObject()
		{
			Shape instance = shapeFactory.GetRandom();
			var t = instance.transform;
			t.localPosition = Random.insideUnitSphere * 5f;
			t.localRotation = Random.rotation;
			t.localScale = Vector3.one * Random.Range(0.1f, 1f);
			_objectList.Add(instance);
		}
		
		private void DestroyObject () {
			if (_objectList.Count > 0)
			{
				int index = Random.Range(0, _objectList.Count);
				Destroy(_objectList[index].gameObject);
				_objectList.RemoveAt(index);
			}
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
				writer.Write(_objectList[i].ShapeId);
				writer.Write(_objectList[i].MaterialId);
				_objectList[i].Save(writer);
			}
		}

		public override void Load(GameDataReader reader)
		{
			var version = reader.Version;
			if (version > _saveVersion)
			{
				Debug.LogError("Unsupported future save version " + version);
				return;
			}

			int count = version <= 0 ? -version : reader.ReadInt();
			for (int i = 0; i < count; i++)
			{
				var shapeId = version <= 0 ? 0 : reader.ReadInt();
				var materialId = version <= 0 ? 0 : reader.ReadInt();
				Shape instance = shapeFactory.Get(shapeId, materialId);
				instance.Load(reader);
				_objectList.Add(instance);
			}
		}
	}
}
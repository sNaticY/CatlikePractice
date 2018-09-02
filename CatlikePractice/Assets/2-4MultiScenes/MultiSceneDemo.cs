using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MultiScenes
{
	public class MultiSceneDemo : PersistableObject
	{
		public ShapeFactory shapeFactory;

		public KeyCode CreateKey = KeyCode.C;
		public KeyCode destroyKey = KeyCode.X;
		public KeyCode NewGameKey = KeyCode.N;
		public KeyCode SaveKey = KeyCode.S;
		public KeyCode LoadKey = KeyCode.L;

		private List<Shape> _objectList;

		private int _saveVersion = 2;

		public PersistentStorage Storage;

		public float CreationSpeed { get; set; }
		public float DestructionSpeed { get; set; }

		public int LevelCount;


		private float _creationProgress;
		private float _destructionProgress;

		private int _loadedLevelBuildIndex;

		private void Start()
		{
			_objectList = new List<Shape>();

			if (Application.isEditor)
			{
				for (int i = 0; i < SceneManager.sceneCount; i++)
				{
					Scene loadedScene = SceneManager.GetSceneAt(i);
					if (loadedScene.name.Contains("Level "))
					{
						SceneManager.SetActiveScene(loadedScene);
						_loadedLevelBuildIndex = loadedScene.buildIndex;
						return;
					}
				}
			}

			StartCoroutine(LoadLevel(1));
		}


		private void Update()
		{
			if (Input.GetKeyDown(CreateKey))
			{
				CreateObject();
			}
			else if (Input.GetKeyDown(destroyKey))
			{
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
			else
			{
				for (int i = 1; i <= LevelCount; i++)
				{
					if (Input.GetKeyDown(KeyCode.Alpha0 + i))
					{
						BeginNewGame();
						StartCoroutine(LoadLevel(i));
						return;
					}
				}
			}

			_creationProgress += Time.deltaTime * CreationSpeed;
			while (_creationProgress >= 1f)
			{
				_creationProgress -= 1f;
				CreateObject();
			}

			_destructionProgress += Time.deltaTime * DestructionSpeed;
			while (_destructionProgress >= 1f)
			{
				_destructionProgress -= 1f;
				DestroyObject();
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

		private void DestroyObject()
		{
			if (_objectList.Count > 0)
			{
				int index = Random.Range(0, _objectList.Count);
				shapeFactory.Reclaim(_objectList[index]);
				int lastIndex = _objectList.Count - 1;
				_objectList[index] = _objectList[lastIndex];
				_objectList.RemoveAt(lastIndex);
			}
		}

		private void BeginNewGame()
		{
			for (int i = 0; i < _objectList.Count; i++)
			{
				shapeFactory.Reclaim(_objectList[i]);
			}

			_objectList.Clear();
		}

		public override void Save(GameDataWriter writer)
		{
			writer.Write(_objectList.Count);
			writer.Write(_loadedLevelBuildIndex);
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
			StartCoroutine(LoadLevel(version < 2 ? 1 : reader.ReadInt()));
			for (int i = 0; i < count; i++)
			{
				var shapeId = version <= 0 ? 0 : reader.ReadInt();
				var materialId = version <= 0 ? 0 : reader.ReadInt();
				Shape instance = shapeFactory.Get(shapeId, materialId);
				instance.Load(reader);
				_objectList.Add(instance);
			}
		}

		private IEnumerator LoadLevel(int levelBuildIndex)
		{
			enabled = false;
			if (_loadedLevelBuildIndex > 0) {
				yield return SceneManager.UnloadSceneAsync(_loadedLevelBuildIndex);
			}
			yield return SceneManager.LoadSceneAsync(levelBuildIndex, LoadSceneMode.Additive);
			SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(levelBuildIndex));
			_loadedLevelBuildIndex = levelBuildIndex;
			enabled = true;
		}
	}
}
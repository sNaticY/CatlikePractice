using UnityEngine;

public class NucleonSpawner : MonoBehaviour
{
	public float TimeBetweenSpawns;
	public float SpawnDistance;
	public Nucleon[] NucleonPrefabs;

	private float _timeSinceLastSpawn;

	private void FixedUpdate()
	{
		_timeSinceLastSpawn += Time.deltaTime;
		if (_timeSinceLastSpawn >= TimeBetweenSpawns)
		{
			_timeSinceLastSpawn -= TimeBetweenSpawns;
			SpawnNucleon();
		}
	}

	private void SpawnNucleon()
	{
		var prefab = NucleonPrefabs[Random.Range(0, NucleonPrefabs.Length)];
		var spawn = Instantiate<Nucleon>(prefab);
		spawn.transform.localPosition = Random.onUnitSphere * SpawnDistance;
	}
}
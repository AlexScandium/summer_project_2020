
using Com.WWZR.WorldWarZRoyal.Bots;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotManager : MonoBehaviour
{
	[SerializeField] private GameObject zombiesPrefab = null;
	[SerializeField] private GameObject playersPrefab = null;

	[Space]
	[Header("Spawn details")]
	[SerializeField] private uint numberOfPlayers = 5;
	[SerializeField] private float timeBetweenZombiesSpawn = 1;

	[Space]
	[Header("Area details")]
	[SerializeField] private float areaRadius = 50f;

	private float elapsedTimeBetweenZombiesSpawn = 0f;


	// Start is called before the first frame update
	void Start()
	{
		ZombieBot.limitRadius = areaRadius;
		SpawnPlayers();
	}

	// Update is called once per frame
	void Update()
    {
		SpawnZombies();
    }


	private void SpawnPlayers()
	{

		for (int i = 0; i < numberOfPlayers; i++)
		{
			Spawn(playersPrefab);
		}
	}

	private void SpawnZombies()
	{
		if (elapsedTimeBetweenZombiesSpawn > timeBetweenZombiesSpawn)
		{
			Spawn(zombiesPrefab);
			elapsedTimeBetweenZombiesSpawn = 0;
		}

		elapsedTimeBetweenZombiesSpawn += Time.deltaTime;
	}

	/// <summary>
	/// This method make spawn an object based on the prefab given, in a position determined by the areaRadius property.
	/// </summary>
	/// <param name="prefab"></param>
	private void Spawn(GameObject prefab)
	{
		Transform bot;
		Vector3 position = Vector3.zero;

		bot = Instantiate(prefab).transform;

		position = UnityEngine.Random.insideUnitSphere * areaRadius;
		position.y = 0;

		bot.position = position;
	}
}

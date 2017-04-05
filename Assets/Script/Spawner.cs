using System.Collections;
using System.Collections.Generic;
using UnityEngine;

	public class Spawner : MonoBehaviour {
	public Enemy enemy;
	public Wave[] waves;

	Wave currentWave;
	int currentWaveNumber;

	int enemiesRemainingToSpawn;
	int enemiesRemainingAlive;
	float nextSpawnTime;

	[System.SerializableAttribute]
	public class Wave {
		public int enemyCount;
		public float timeBetweenSpawns;
	}

	void Start () {
		NextWave ();
	}

	void Update () {
		if(enemiesRemainingToSpawn > 0 && Time.time > nextSpawnTime) {
			enemiesRemainingToSpawn--;
			nextSpawnTime = Time.time + currentWave.timeBetweenSpawns;

			Enemy spawnedEnemy = Instantiate(enemy, Vector3.zero, Quaternion.identity);
			spawnedEnemy.OnDeath += OnEnemyDeath;
		}
	}

	void OnEnemyDeath () {
		print ("Enemy died");
		enemiesRemainingAlive --;

		if (enemiesRemainingAlive == 0) {
			NextWave();
		}
	}
	void NextWave () {
		currentWaveNumber ++;
		print ("Wave : " + currentWaveNumber);
		if (currentWaveNumber - 1 < waves.Length) {
			currentWave = waves [currentWaveNumber - 1];

			enemiesRemainingToSpawn = currentWave.enemyCount;
			enemiesRemainingAlive = enemiesRemainingToSpawn;
		}
	}

}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // Config params
    [SerializeField] List<WaveConfig> waveConfigs;
    [SerializeField] int startingWave = 0;
    [SerializeField] bool loopWaves = false;

	// Use this for initialization
	IEnumerator Start ()
    {
        do
        {
            yield return StartCoroutine(SpawnAllWaves());
        }
        while (loopWaves);
	}

    private IEnumerator SpawnAllWaves()
    {
        for (int waveIndex = startingWave; waveIndex < waveConfigs.Count; waveIndex++)
        {
            var currentWave = waveConfigs[waveIndex];

            yield return StartCoroutine(SpawnAllEnemiesInWave(currentWave));
        }
    }

    private IEnumerator SpawnAllEnemiesInWave(WaveConfig currentWave)
    {
        int numberOfEnemies = currentWave.GetNumberOfEnemies();

        for (int enemyCount = 0; enemyCount < numberOfEnemies; enemyCount++)
        {
            Vector3 startPosition = currentWave.GetWaypoints()[0].transform.position;
            GameObject enemyPrefab = currentWave.GetEnemyPrefab();
            Quaternion StartRotation = Quaternion.identity;
            WaitForSeconds spawnDelayTime = new WaitForSeconds(currentWave.GetTimeBetweenSpawns());

            var newEnemy = Instantiate(enemyPrefab, startPosition, StartRotation);

            newEnemy.GetComponent<EnemyPathing>().SetWaveConfig(currentWave);

            yield return spawnDelayTime;
        }

    }

    // Update is called once per frame
    void Update ()
    {
		
	}
}

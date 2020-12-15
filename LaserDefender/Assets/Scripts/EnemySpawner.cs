using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // Config params
    [SerializeField] List<WaveConfig> waveConfigs;
    [SerializeField] int startingWave = 0;
    [SerializeField] bool loopWaves = false;

    // StateVariables
    bool bossDead = false;

	// Use this for initialization
	IEnumerator Start ()
    {
        do
        {
            yield return StartCoroutine(SpawnAllWaves());

            SetBossAlive();
        }
        while (loopWaves);
	}

    private IEnumerator SpawnAllWaves()
    {
        int numberOfWaves = waveConfigs.Count;

        for (int waveIndex = startingWave; waveIndex < numberOfWaves; waveIndex++)
        {
            var currentWave = waveConfigs[waveIndex];

            if (waveIndex + 1 < numberOfWaves)
            {
                yield return StartCoroutine(SpawnAllEnemiesInWave(currentWave));
            }
            else if (waveIndex + 1 == numberOfWaves)
            {
                StartCoroutine(SpawnAllEnemiesInWave(currentWave));

                while (!bossDead)
                {
                    yield return null;
                }
            }
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

    public void SetBossDead()
    {
        bossDead = true;
    }

    private void SetBossAlive()
    {
        bossDead = false;
    }
}

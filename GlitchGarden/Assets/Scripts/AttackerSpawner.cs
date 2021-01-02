using System.Collections;
using UnityEngine;

public class AttackerSpawner : MonoBehaviour
{
    // Config params
    [SerializeField] Attacker[] attackerPrefabArray = null;
    [SerializeField] float initialMinSpawnDelay = 2f;
    [SerializeField] float initialMaxSpawnDelay = 6f;
    [SerializeField] float minSpawnDelay = 1f;
    [SerializeField] float maxSpawnDelay = 5f;
    float initialLevelDelay = 4f;

    // State params
    bool spawn = true;

	// Use this for initialization
	IEnumerator Start ()
    {
        float spawnTime = initialLevelDelay + Random.Range(initialMinSpawnDelay, initialMaxSpawnDelay);
        yield return new WaitForSeconds(spawnTime);

        SpawnAttacker();

        spawnTime = Random.Range(minSpawnDelay, maxSpawnDelay);
        yield return new WaitForSeconds(spawnTime);

        while (spawn)
        {
            SpawnAttacker();

            spawnTime = Random.Range(minSpawnDelay, maxSpawnDelay);
            yield return new WaitForSeconds(spawnTime);
        }
    }

    public void StopSpawning()
    {
        spawn = false;
    }

    private void SpawnAttacker()
    {
        int randomAttackerIndex = Random.Range(0, attackerPrefabArray.Length);

        Attacker attackerPrefab = attackerPrefabArray[randomAttackerIndex];

        Spawn(attackerPrefab);
    }

    private void Spawn(Attacker attackerPrefab)
    {
        Attacker newAttacker = Instantiate(attackerPrefab, gameObject.transform.position, Quaternion.identity) as Attacker;

        newAttacker.transform.parent = transform;
    }

    // Update is called once per frame
    void Update ()
    {
		
	}
}

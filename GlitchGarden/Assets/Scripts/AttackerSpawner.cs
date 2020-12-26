using System.Collections;
using UnityEngine;

public class AttackerSpawner : MonoBehaviour
{
    // Config params
    [SerializeField] Attacker[] attackerPrefabArray = null;
    [SerializeField] float minSpawnDelay = 1f;
    [SerializeField] float maxSpawnDelay = 5f;

    // State params
    bool spawn = true;

	// Use this for initialization
	IEnumerator Start ()
    {
		if (spawn)
        {
            float spawnTime = Random.Range(minSpawnDelay, maxSpawnDelay);

            yield return new WaitForSeconds(spawnTime);

            SpawnAttacker();
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

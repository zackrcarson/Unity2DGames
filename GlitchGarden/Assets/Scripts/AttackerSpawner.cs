using System.Collections;
using UnityEngine;

public class AttackerSpawner : MonoBehaviour
{
    // Config params
    [SerializeField] Attacker attackerPrefab = null;
    [SerializeField] float minSpawnDelay = 1f;
    [SerializeField] float maxSpawnDelay = 5f;

    // State params
    bool spawn = true;

	// Use this for initialization
	IEnumerator Start ()
    {
		while (spawn)
        {
            float spawnTime = Random.Range(minSpawnDelay, maxSpawnDelay);

            yield return new WaitForSeconds(spawnTime);

            SpawnAttacker();
        }
    }

    private void SpawnAttacker()
    {
        Attacker newAttacker = Instantiate(attackerPrefab, gameObject.transform.position, Quaternion.identity) as Attacker;

        newAttacker.transform.parent = transform;
    }

    // Update is called once per frame
    void Update ()
    {
		
	}
}

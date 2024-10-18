using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    public GameObject enemyPrefab;
    //public GameObject stronghold;

    public float spawnInterval = 10f; // Time in seconds between spawns


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnEnemy());  // Start the spawning coroutine
    }


    private IEnumerator SpawnEnemy()
    {
        while (true)
        {
            Instantiate(enemyPrefab, transform.position, Quaternion.identity); // Spawn the enemy
            yield return new WaitForSeconds(spawnInterval);  // Wait for the specified interval
        }
    }

}

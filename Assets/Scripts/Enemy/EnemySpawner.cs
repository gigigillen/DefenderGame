using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    public GameObject enemyPrefab;
    //public GameObject stronghold;

    public float spawnInterval = 10f; // Time in seconds between spawns

    [SerializeField] private Transform enemies;
    private int enemyCount;


    // Start is called before the first frame update
    void Start()
    {
        enemyCount = 0;
        StartCoroutine(SpawnEnemy());  // Start the spawning coroutine
    }


    private IEnumerator SpawnEnemy()
    {
        while (true)
        {
            GameObject newEnemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity); // Spawn the enemy
            newEnemy.transform.SetParent(enemies);

            newEnemy.name = "Enemy " + (enemyCount + 1);
            enemyCount++;
            yield return new WaitForSeconds(spawnInterval);  // Wait for the specified interval
        }
    }

}

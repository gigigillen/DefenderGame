using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    public GameObject enemyPrefab;
    //public GameObject stronghold;

    public float spawnInterval = 10f; 

    [SerializeField] private Transform enemies;
    private int enemyCount;


    void Start()
    {
        enemyCount = 0;

        //spawns enemies
        StartCoroutine(SpawnEnemy());
    }

    //routinely spawns enemies
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

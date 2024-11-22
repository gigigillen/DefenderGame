using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesSpawner : MonoBehaviour
{
    public GameObject waterEnemyPrefab;  // Prefab for water enemy
    public GameObject earthEnemyPrefab;  // Prefab for earth enemy

    public float spawnInterval = 5f;  // Default spawn interval

    [SerializeField] private Transform enemies;
    private int enemyCount;

    private bool spawnWaterEnemy = true;  // Flag to alternate between enemy types

    void Start()
    {
        enemyCount = 0;

        // Start the enemy spawning coroutine
        StartCoroutine(SpawnEnemies());
    }

    // This coroutine spawns enemies, alternating between water and earth enemies
    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
            // Alternate between spawning water and earth enemies
            if (spawnWaterEnemy)
            {
                // Spawn the water enemy
                GameObject waterEnemy = Instantiate(waterEnemyPrefab, transform.position, Quaternion.identity);
                waterEnemy.transform.SetParent(enemies);
                waterEnemy.name = "Water Enemy " + (enemyCount + 1);
            }
            else
            {
                // Spawn the earth enemy
                GameObject earthEnemy = Instantiate(earthEnemyPrefab, transform.position, Quaternion.identity);
                earthEnemy.transform.SetParent(enemies);
                earthEnemy.name = "Earth Enemy " + (enemyCount + 1);
            }

            // Increment the enemy count regardless of the type of enemy
            enemyCount++;

            // Toggle between water and earth enemy for the next spawn
            spawnWaterEnemy = !spawnWaterEnemy;

            // Wait for the specified interval before spawning the next enemy
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizedEnemies : MonoBehaviour
{
    public GameObject[] enemyPrefabs;  // Array to hold the different enemy prefabs

    public float spawnInterval = 7f;  // Default spawn interval

    [SerializeField] private Transform enemies;  // Parent object for spawned enemies
    private int enemyCount;  // Track the number of enemies spawned

    private Coroutine spawnCoroutine;

    public float SpawnInterval
    {
        get { return spawnInterval; }
        set
        {
            spawnInterval = value;
            // Restart the coroutine with new interval
            if (spawnCoroutine != null)
            {
                StopCoroutine(spawnCoroutine);
            }
            spawnCoroutine = StartCoroutine(SpawnEnemies(spawnInterval));
        }
    }

    void Start()
    {
        enemyCount = 0;

        // Start the enemy spawning coroutine
        spawnCoroutine = StartCoroutine(SpawnEnemies(spawnInterval));
    }

    // This coroutine spawns enemies, randomly selecting from the available enemy prefabs
    private IEnumerator SpawnEnemies(float spawnInt)
    {
        while (true)
        {
            // Randomly select an enemy prefab from the enemyPrefabs array
            int randomIndex = Random.Range(0, 3);  // Random index between 0 and enemyPrefabs.Length - 1
            GameObject selectedEnemy = enemyPrefabs[randomIndex];  // Get the enemy prefab at the random index

            // Instantiate the selected enemy at the spawner's position with no rotation
            GameObject enemy = Instantiate(selectedEnemy, transform.position, Quaternion.identity);
            enemy.transform.SetParent(enemies);  // Set the parent of the spawned enemy
            enemy.name = selectedEnemy.name + " " + (enemyCount + 1);  // Name the enemy (e.g., "Water Enemy 1")

            enemyCount++;  // Increment the enemy count

            // Wait for the specified interval before spawning the next enemy
            yield return new WaitForSeconds(spawnInt);
        }
    }
}

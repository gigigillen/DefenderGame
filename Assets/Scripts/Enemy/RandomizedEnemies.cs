using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizedEnemies : MonoBehaviour {
    public GameObject[] enemyPrefabs;  

    public float spawnInterval = 7f;  
    
    [SerializeField] private Transform enemies;  
    
    private int enemyCount;  
    
    private Coroutine spawnCoroutine;

    // Get the spawn interval routine (can change based on SpawnController and waves)
    public float SpawnInterval {
        get { return spawnInterval; }
        set {
            spawnInterval = value;
            // Restart the coroutine with new interval if the component is enabled
            if (isActiveAndEnabled) {
                RestartSpawnCoroutine();
            }
        }
    }

    void Start() {
        enemyCount = 0;
        StartSpawning();
    }

    void OnEnable() {
        StartSpawning();
    }

    void OnDisable() {
        StopSpawning();
    }

    // starts the spawning routine intervals
    private void StartSpawning() {
        if (spawnCoroutine == null) {
            spawnCoroutine = StartCoroutine(SpawnEnemies(spawnInterval));
        }
    }

    // stops the spawning routine interval completely
    private void StopSpawning() {
        if (spawnCoroutine != null) {
            StopCoroutine(spawnCoroutine);
            spawnCoroutine = null;
        }
    }

    // changes spawn rate by stopping it and restarting spawn interval
    private void RestartSpawnCoroutine() {
        StopSpawning();
        StartSpawning();
    }

    // this coroutine spawns enemies, randomly selecting from the available enemy prefabs
    private IEnumerator SpawnEnemies(float spawnInt) {
        while (true) {
            // randomly select an enemy prefab from the enemyPrefabs array
            int randomIndex = Random.Range(0, 3);  
            GameObject selectedEnemy = enemyPrefabs[randomIndex];  

            // instantiate the selected enemy at the spawner's position with no rotation
            GameObject enemy = Instantiate(selectedEnemy, transform.position, Quaternion.identity);
            // set the parent of the spawned enemy
            enemy.transform.SetParent(enemies);  
            enemy.name = selectedEnemy.name + " " + (enemyCount + 1); 
            enemyCount++;  

            // wait for the specified interval before spawning the next enemy
            yield return new WaitForSeconds(spawnInt);
        }
    }
}
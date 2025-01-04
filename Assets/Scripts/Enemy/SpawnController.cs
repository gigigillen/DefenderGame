using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour {

    public RandomizedEnemies eastSpawner;
    public RandomizedEnemies northSpawner;
    public RandomizedEnemies southSpawner;
    public RandomizedEnemies westSpawner;
    
    public Wizard wizardSpawner;
    
    private int numberOfWaves;
    private int currentWave = 1;
    
    [Header("Wave Settings")]
    public float timeBetweenWaves = 45f;
    public float baseSpawnInterval = 30f;
    public float minSpawnInterval = 5f;
    
    [Header("Wave Duration")]
    public float waveDuration = 30f;
    private bool isWaveActive = false;
    
    void Start() {
        numberOfWaves = MainMenu.NumberOfWaves;
        StartCoroutine(WaveController());
    }

    IEnumerator WaveController() {
        while (currentWave <= numberOfWaves) {
            Debug.Log($"Starting Wave {currentWave}");

            float spawnInterval = CalculateSpawnInterval(currentWave);

            SetSpawnRates(spawnInterval);

            isWaveActive = true;

            yield return new WaitForSeconds(waveDuration);

            isWaveActive = false;

            if (currentWave < numberOfWaves) {
                Debug.Log($"Wave {currentWave} complete. Preparing next wave...");
                yield return new WaitForSeconds(timeBetweenWaves);
            }
            
            currentWave++;
        }

        Debug.Log("Spawning wizard after final wave...");
        StopAllSpawners();
        yield return new WaitForSeconds(timeBetweenWaves);
        SpawnWizard();  
        Debug.Log("All waves completed!");
    }

    float CalculateSpawnInterval(int wave) {
        // Early waves (1-5)
        if (wave <= 5) {
            return Mathf.Lerp(45f, 30f, (wave - 1) / 4f);
        }

        // Mid waves (6-15)
        else if (wave <= 15) {
            return Mathf.Lerp(30f, 20f, (wave - 6) / 9f);
        }

        // Late waves (16-30)
        else {
            float interval = Mathf.Lerp(20f, 10f, (wave - 16) / 14f);
            return Mathf.Max(interval, 2f);
        }
    }

    void StopAllSpawners() {
        eastSpawner.enabled = false;
        northSpawner.enabled = false;
        southSpawner.enabled = false;
        westSpawner.enabled = false;
    }


    void SetSpawnRates(float baseInterval) {
        // Slightly varied intervals for each spawner to prevent synchronized spawns
        SetEastSpawnRate(baseInterval * Random.Range(0.9f, 1.1f));
        SetNorthSpawnRate(baseInterval * Random.Range(0.9f, 1.1f));
        SetSouthSpawnRate(baseInterval * Random.Range(0.9f, 1.1f));
        SetWestSpawnRate(baseInterval * Random.Range(0.9f, 1.1f));
    }

    void SetEastSpawnRate(float newInterval) {
        eastSpawner.SpawnInterval = newInterval;
        Debug.Log($"East Spawner interval set to {newInterval:F2} seconds.");
    }

    void SetNorthSpawnRate(float newInterval) {
        northSpawner.SpawnInterval = newInterval;
        Debug.Log($"North Spawner interval set to {newInterval:F2} seconds.");
    }

    void SetSouthSpawnRate(float newInterval) {
        southSpawner.SpawnInterval = newInterval;
        Debug.Log($"South Spawner interval set to {newInterval:F2} seconds.");
    }

    void SetWestSpawnRate(float newInterval) {
        westSpawner.SpawnInterval = newInterval;
        Debug.Log($"West Spawner interval set to {newInterval:F2} seconds.");
    }
    
    void SpawnWizard() {
        // Call the SpawnWizard function from the Wizard script
        if (wizardSpawner != null)
        {
            wizardSpawner.SpawnWizard();
        }
    }
    
    public int GetCurrentWave() {
        return currentWave;
    }
    
    public bool IsWaveActive() {
        return isWaveActive;
    }
}
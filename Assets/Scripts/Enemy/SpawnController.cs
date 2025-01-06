using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpawnController : MonoBehaviour {
    // spawn paths
    public RandomizedEnemies eastSpawner;
    public RandomizedEnemies northSpawner;
    public RandomizedEnemies southSpawner;
    public RandomizedEnemies westSpawner;
    public Wizard wizardSpawner;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI waveMessageText;
    private float messageDisplayDuration = 5f;
    
    [Header("Wave Settings")]
    public float timeBetweenWaves = 45f;
    public float baseSpawnInterval = 30f;
    public float minSpawnInterval = 5f;
    
    [Header("Wave Duration")]
    private int numberOfWaves;
    private int currentWave = 1;
    public float waveDuration = 45f;
    private bool isWaveActive = false;
    private Coroutine messageCoroutine;

    void Start() {
        numberOfWaves = MainMenu.NumberOfWaves;
        StartCoroutine(WaveController());
    }

    // sets the spawn rates and controls the wave to go as far as the selected wave in the main menu ui
    IEnumerator WaveController() {
        while (currentWave <= numberOfWaves) {
            ShowMessage($"Starting Wave {currentWave}!");

            float spawnInterval = CalculateSpawnInterval(currentWave);

            SetSpawnRates(spawnInterval);

            isWaveActive = true;

            yield return new WaitForSeconds(waveDuration);

            isWaveActive = false;

            if (currentWave < numberOfWaves) {
                ShowMessage($"Wave {currentWave} Complete! Next wave in {timeBetweenWaves} seconds...");
                yield return new WaitForSeconds(timeBetweenWaves);
            }
            
            currentWave++;
        }

        ShowMessage("Final Wave Complete! Preparing for Boss...");
        StopAllSpawners();
        yield return new WaitForSeconds(timeBetweenWaves);
        SpawnWizard();
        ShowMessage("The Wizard has appeared!");
    }

    // adds a message to the userinterface to announce relevant wave messages
    void ShowMessage(string message)
    {
        if (waveMessageText != null)
        {
            if (messageCoroutine != null)
            {
                StopCoroutine(messageCoroutine);
            }
            messageCoroutine = StartCoroutine(DisplayMessageCoroutine(message));
        }
        else
        {
            Debug.LogWarning("Wave Message Text component not assigned!");
        }
    }

    IEnumerator DisplayMessageCoroutine(string message)
    {
        waveMessageText.text = message;
        yield return new WaitForSeconds(messageDisplayDuration);
        waveMessageText.text = "";
    }

    float CalculateSpawnInterval(int wave) {
        // early waves (1-5)
        if (wave <= 5) {
            return Mathf.Lerp(45f, 30f, (wave - 1) / 4f);
        }

        // mid waves (6-15)
        else if (wave <= 15) {
            return Mathf.Lerp(30f, 20f, (wave - 6) / 9f);
        }

        // late waves (16-30)
        else {
            float interval = Mathf.Lerp(20f, 10f, (wave - 16) / 14f);
            return Mathf.Max(interval, 2f);
        }
    }

    // stops all the paths from spawning enemies
    void StopAllSpawners() {
        eastSpawner.enabled = false;
        northSpawner.enabled = false;
        southSpawner.enabled = false;
        westSpawner.enabled = false;
    }

    // slightly varied intervals for each spawner to prevent synchronized spawns
    void SetSpawnRates(float baseInterval) {
        SetEastSpawnRate(baseInterval * Random.Range(0.9f, 1.1f));
        SetNorthSpawnRate(baseInterval * Random.Range(0.9f, 1.1f));
        SetSouthSpawnRate(baseInterval * Random.Range(0.9f, 1.1f));
        SetWestSpawnRate(baseInterval * Random.Range(0.9f, 1.1f));
    }

    // sets the wave spawn interval rate
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


    // call the SpawnWizard function from the Wizard script
    void SpawnWizard() {
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
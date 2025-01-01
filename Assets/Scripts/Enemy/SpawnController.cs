using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    public RandomizedEnemies eastSpawner;
    public RandomizedEnemies northSpawner;
    public RandomizedEnemies southSpawner;
    public RandomizedEnemies westSpawner;

    void Start()
    {
        // Set initial spawn intervals for each spawner
        SetEastSpawnRate(5f);  // East spawner interval
        SetNorthSpawnRate(14f); // North spawner interval
        SetSouthSpawnRate(10f); // South spawner interval
        SetWestSpawnRate(7f); // West spawner interval
    }

    void SetEastSpawnRate(float newInterval)
    {
        eastSpawner.SpawnInterval = newInterval;
        Debug.Log($"East Spawner interval set to {newInterval} seconds.");
    }

    void SetNorthSpawnRate(float newInterval)
    {
        northSpawner.SpawnInterval = newInterval;
        Debug.Log($"North Spawner interval set to {newInterval} seconds.");
    }

    void SetSouthSpawnRate(float newInterval)
    {
        southSpawner.SpawnInterval = newInterval;
        Debug.Log($"South Spawner interval set to {newInterval} seconds.");
    }

    void SetWestSpawnRate(float newInterval)
    {
        westSpawner.SpawnInterval = newInterval;
        Debug.Log($"West Spawner interval set to {newInterval} seconds.");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool : MonoBehaviour {
    public GameObject projectilePrefab;
    private Queue<GameObject> pool = new Queue<GameObject>();
    private int initialSize = 20;

    void Start() {
        for (int i = 0; i < initialSize; i++) {
            CreateNewProjectile();
        }
    }

    private void CreateNewProjectile() {
        GameObject projectile = Instantiate(projectilePrefab);
        projectile.SetActive(false);
        pool.Enqueue(projectile);
    }

    public GameObject GetProjectile() {
        if (pool.Count == 0) CreateNewProjectile();
        GameObject projectile = pool.Dequeue();
        projectile.SetActive(true);
        return projectile;
    }

    public void ReturnProjectile(GameObject projectile) {
        projectile.SetActive(false);
        pool.Enqueue(projectile);
    }
}

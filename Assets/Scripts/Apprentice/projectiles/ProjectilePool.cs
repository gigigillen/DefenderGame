using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool : MonoBehaviour {
    [System.Serializable]
    public class ProjectileTypeInfo {
        public ApprenticeType type;
        public GameObject prefab;
        public int initialPoolSize = 20;
    }

    [SerializeField] private ProjectileTypeInfo[] projectileTypes;
    private Dictionary<ApprenticeType, Queue<GameObject>> pools = new Dictionary<ApprenticeType, Queue<GameObject>>();

    void Start() {
        foreach (var typeInfo in projectileTypes) {
            Queue<GameObject> typePool = new Queue<GameObject>();
            pools[typeInfo.type] = typePool;

            for (int i = 0; i < typeInfo.initialPoolSize; i++) {
                CreateNewProjectile(typeInfo.type);
            }
        }
    }

    private void CreateNewProjectile(ApprenticeType type) {
        var typeInfo = System.Array.Find(projectileTypes, x => x.type == type);
        GameObject projectile = Instantiate(typeInfo.prefab);
        projectile.SetActive(false);
        pools[type].Enqueue(projectile);
    }

    public GameObject GetProjectile(ApprenticeType type) {
        if (!pools.ContainsKey(type) || pools[type].Count == 0) {
            CreateNewProjectile(type);
        }
        GameObject projectile = pools[type].Dequeue();
        projectile.SetActive(true);
        return projectile;
    }

    public void ReturnProjectile(GameObject projectile) {
        projectile.SetActive(false);
        ApprenticeType type = projectile.GetComponent<ProjectileController>().projectileType;
        pools[type].Enqueue(projectile);
    }
}

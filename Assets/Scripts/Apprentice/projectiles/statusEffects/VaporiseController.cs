using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VaporiseController : MonoBehaviour {

    private static VaporiseController instance;
    private Dictionary<int, float> lastVaporiseTimes = new Dictionary<int, float>();
    private float vaporiseCooldown = 8f;

    private void Awake() {

        if (instance == null) {
            instance = this;
        }
    }

    public static bool CanVaporise(GameObject enemy) {

        int enemyID = enemy.GetInstanceID();
        if (!instance.lastVaporiseTimes.ContainsKey(enemyID)) {
            Debug.Log("first vaporise - can vaporise");
            return true;
        }

        if (Time.time >= instance.lastVaporiseTimes[enemyID] + instance.vaporiseCooldown) {
            Debug.Log("vaporise off cooldown - can vaporise");
            return true;
        }
        Debug.Log("vaporise in cooldown still - cannot vaporise");
        return false;

        //return Time.time >= instance.lastVaporiseTimes[enemyID] + instance.vaporiseCooldown;
    }

    public static void RecordVaporise(GameObject enemy) {

        instance.lastVaporiseTimes[enemy.GetInstanceID()] = Time.time;
    }
}

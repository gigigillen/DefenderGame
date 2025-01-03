using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VortexController : MonoBehaviour {

    private static VortexController instance;

    private Dictionary<int, float> lastVortexTimes = new Dictionary<int, float>();

    private float vortexCooldown = 15f;

    private void Awake() {
        if (instance == null) {
            instance = this;
        }
    }

    public static bool CanCreateVortex(ApprenticeController apprentice) {

        int towerID = apprentice.gameObject.GetInstanceID();

        if (!instance.lastVortexTimes.ContainsKey(towerID)) {
            return true;
        }

        if (Time.time >= instance.lastVortexTimes[towerID] + instance.vortexCooldown) {
            return true;
        }

        return false;
    }

    public static void RecordVortex(ApprenticeController apprentice) {
        instance.lastVortexTimes[apprentice.gameObject.GetInstanceID()] = Time.time;
    }

}

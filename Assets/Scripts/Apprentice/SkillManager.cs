using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour {

    private static SkillManager instance;

    [SerializeField] private ApprenticeTypeData[] apprenticeTypes;

    private Dictionary<ApprenticeType, bool> unlockedTypes = new Dictionary<ApprenticeType, bool>();
    private Dictionary<ApprenticeType, bool> unlockedVortex = new Dictionary<ApprenticeType, bool>();

    private void Awake() {

        if (instance == null) {
            instance = this;

            InitialiseUnlockStates();
        }
        else {
            Destroy(gameObject);
        }
    }


    private void InitialiseUnlockStates() {
        unlockedTypes.Clear();
        unlockedVortex.Clear();

        // setup initial states for each apprentice type
        foreach (var data in apprenticeTypes) {
            unlockedTypes[data.type] = (data.type == ApprenticeType.Basic);
            unlockedVortex[data.type] = false;
        }
    }


    public void UnlockAbility(ApprenticeType type, string abilityName) {

        switch (abilityName) {
            case "Vortex":
                if (type == ApprenticeType.Wind) {
                    unlockedVortex[type] = true;
                    Debug.Log("Vortex ability unlocked for wind apprentices!");
                }
                break;

            // cases for other abilities
        }
    }

    public static bool IsAbilityUnlocked(ApprenticeType type, string abilityName) {

        switch (abilityName) {
            case "Vortex":
                return type == ApprenticeType.Wind &&
                    instance.unlockedVortex.TryGetValue(type, out bool unlocked) &&
                    unlocked;
            default:
                return false;
        }
    }


    private ApprenticeTypeData GetTypeData(ApprenticeType type) {

        foreach (var data in apprenticeTypes) {
            if (data.type == type) return data;
        }
        return null;
    }

}

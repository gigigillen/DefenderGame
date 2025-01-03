using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour {

    private static SkillManager instance;

    [SerializeField] private ApprenticeTypeData[] apprenticeTypes;

    private Dictionary<ApprenticeType, bool> unlockedTypes = new Dictionary<ApprenticeType, bool>();

    private bool unlockedVortex = false;
    private bool unlockedAoePulse = false;
    private bool unlockedBurning = false;
    private bool unlockedWetness = false;

    private void Awake() {

        if (instance == null) {
            instance = this;
        }
        else {
            Destroy(gameObject);
        }
    }


    public void UnlockAbility(ApprenticeType type, string abilityName) {

        if (type == ApprenticeType.Wind && abilityName == "Vortex") {
            unlockedVortex = true;
            Debug.Log("Vortex ability unlocked for Wind apprentices!");
        }
        else if (type == ApprenticeType.Earth && abilityName == "aoePulse") {
            unlockedAoePulse = true;
            Debug.Log("Aoe pulse ability unlocked for Earth apprentices!");
        }
        else if (type == ApprenticeType.Fire && abilityName == "Burning") {
            unlockedBurning = true;
            Debug.Log("Burning ability unlocked for Fire apprentices!");
        }
        else if (type == ApprenticeType.Water && abilityName == "Wetness") {
            unlockedWetness = true;
            Debug.Log("Wetness ability unlocked for Water apprentices!");
        }

        
    }

    public static bool IsAbilityUnlocked(ApprenticeType type, string abilityName) {

        if (type == ApprenticeType.Wind && abilityName == "Vortex") {
            return instance.unlockedVortex;
        }
        else if (type == ApprenticeType.Earth && abilityName == "aoePulse") {
            return instance.unlockedAoePulse;
        }
        else if (type == ApprenticeType.Fire && abilityName == "Burning") {
            return instance.unlockedBurning;
        }
        else if (type == ApprenticeType.Water && abilityName == "Wetness") {
            return instance.unlockedWetness;
        }
        return false;
    }


    private ApprenticeTypeData GetTypeData(ApprenticeType type) {

        foreach (var data in apprenticeTypes) {
            if (data.type == type) return data;
        }
        return null;
    }

}

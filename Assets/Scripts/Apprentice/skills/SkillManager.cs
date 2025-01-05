using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SkillManager : MonoBehaviour {

    private static SkillManager instance;

    private static readonly Dictionary<ApprenticeType, List<string>> typeAbilities = new Dictionary<ApprenticeType, List<string>> {
        { ApprenticeType.Water, new List<string> { "Wetness" } },
        { ApprenticeType.Earth, new List<string> { "Rocky Pulse" } },
        { ApprenticeType.Wind, new List<string> { "Vortex" } },
        { ApprenticeType.Fire, new List<string> { "Burning" } }
    };

    private Dictionary<ApprenticeType, HashSet<string>> unlockedAbilities = new Dictionary<ApprenticeType, HashSet<string>>();

    private void Awake() {

        if (instance == null) {
            instance = this;
            InitialiseAbilities();
        }
        else {
            Destroy(gameObject);
        }
    }

    private void InitialiseAbilities() {
        foreach (ApprenticeType type in System.Enum.GetValues(typeof(ApprenticeType))) {
            unlockedAbilities[type] = new HashSet<string>();
        }
    }

    public void UnlockAbility(ApprenticeType type, string abilityName) {

        if (typeAbilities.TryGetValue(type, out List<string> abilities) &&
           abilities.Contains(abilityName)) {

            unlockedAbilities[type].Add(abilityName);
            Debug.Log($"{abilityName} ability unlocked for {type} apprentices!");
        }
        else {
            Debug.LogWarning($"Attempted to unlock invalid ability {abilityName} for {type}");
        }
    }

    public static bool IsAbilityUnlocked(ApprenticeType type, string abilityName) {

        return instance.unlockedAbilities[type].Contains(abilityName);
    }

    public static List<string> GetAllAbilities(ApprenticeType type) {
        return typeAbilities.TryGetValue(type, out List<string> abilities)
            ? new List<string>(abilities)
            : new List<string>();
    }

    public static List<string> GetUnlockedAbilities(ApprenticeType type) {
        return new List<string>(instance.unlockedAbilities[type]);
    }
}

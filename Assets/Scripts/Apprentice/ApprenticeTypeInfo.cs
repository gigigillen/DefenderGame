using UnityEngine;
using TMPro;

[CreateAssetMenu(fileName = "NewApprenticeTypeData", menuName = "Game/ApprenticeTypeData")]
public class ApprenticeTypeData : ScriptableObject {
    public ApprenticeType type;
    public GameObject apprenticePrefab;
    public float cooldown = 2f;
    public float attackRange = 10f;
    public float speed = 5f;
    public bool isStatic = true;
}

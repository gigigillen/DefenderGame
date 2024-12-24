using UnityEngine;

[CreateAssetMenu(fileName = "NewApprenticeTypeData", menuName = "Game/ApprenticeTypeData")]
public class ApprenticeTypeData : ScriptableObject {
    public ApprenticeType type;
    public float cooldown = 2f;
    public float attackRange = 5f;
    public float speed = 5f;
    public bool isStatic = true;
}

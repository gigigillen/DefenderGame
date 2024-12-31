using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_HealthBar : MonoBehaviour
{

    [SerializeField] private Image healthBarFill;

    private Health health;
    private int maxHealth;

    private void Awake() {

        health = GetComponentInParent<Health>();
        maxHealth = health.CompareTag("Enemy") ? 10 : 20;
    }

    private void Update() {

        float healthPercentage = (float)health.getHealth() / maxHealth;
        healthBarFill.fillAmount = healthPercentage;
        healthBarFill.color = GetHealthBarColour(healthPercentage);
    }

    private Color GetHealthBarColour(float percentage) {

        if (percentage > 0.6f)
            return Color.green;
        else if (percentage > 0.3f)
            return Color.yellow;
        else
            return Color.red;
    }
}

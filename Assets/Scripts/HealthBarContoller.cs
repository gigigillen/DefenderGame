using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBarContoller : MonoBehaviour
{
    public Image healthBarFill;
    public TMP_Text healthPercentageText;
    // max health can be adapted to be what we feel is best
    public float maxHealth = 100f;
    private float currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth * 0.65f; // Set to 65% of max health
        UpdateHealthBar();
    }

    // lowers the text and image of health bar
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthBar();
    }

    // raises the text and image of health bar
    public void GainHealth(float amount)
    {
        currentHealth += amount; // Same assumption as above
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthBar();
    }

    // updates everytime the health is changed
    private void UpdateHealthBar()
    {
        float fillAmount = currentHealth / maxHealth; // Calculate the fill amount as a fraction
        healthBarFill.fillAmount = fillAmount; // Set fill amount for the Image component
        healthPercentageText.text = $"HP: {currentHealth / maxHealth * 100:0}%"; // Update displayed percentage
    }
}

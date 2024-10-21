using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBarController : MonoBehaviour
{
    public static HealthBarController instance; // Singleton instance

    public Image healthBarFill;
    public TMP_Text healthPercentageText;
    public float maxHealth = 100f;
    private float currentHealth;

    private GameController gameController;


    void Awake()
    {
        // Ensure that there's only one instance of HealthBarController
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject); // Prevent multiple instances
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        gameController = FindObjectOfType<GameController>();
        currentHealth = maxHealth * 0.65f; // Set to 65% of max health
        UpdateHealthBar();

    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            gameController.GameOver();
        }
    }


    public void GainHealth(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        float fillAmount = currentHealth / maxHealth;
        healthBarFill.fillAmount = fillAmount;
        healthPercentageText.text = $"HP: {currentHealth / maxHealth * 100:0}%";
    }
}

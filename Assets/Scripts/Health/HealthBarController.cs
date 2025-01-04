using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBarController : MonoBehaviour
{
    // creates a singleton (calls itself in itself)
    public static HealthBarController instance;

    // gets the UI components from the interface
    public Image healthBarFill;
    public TMP_Text healthPercentageText;

    public float maxHealth = 100f;
    private float currentHealth;

    // fetches the game controller to interact with other elements
    private GameController gameController;

    // makes sure there is only one health bar, and if there is more than one, destroys the extra copies
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // called before first frame update to set initial healthbar
    void Start()
    {
        gameController = FindAnyObjectByType<GameController>();
        currentHealth = maxHealth;
        UpdateHealthBar();

    }

    // minuses the percentage of damage taken to the tower
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        //ensure it stays between 0 and 100 health
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthBar();

        //if tower runs out of health does the gameover code
        if (currentHealth <= 0)
        {
            gameController.GameOver();
        }
    }

    // adds the pecentage of health to the tower
    public void GainHealth(float amount)
    {
        currentHealth += amount;
        // ensurew it stays between 0 and 100 health
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthBar();
    }

    //changes the health bar on the screen to represent changes in health
    private void UpdateHealthBar()
    {
        float fillAmount = currentHealth / maxHealth;
        healthBarFill.fillAmount = fillAmount;
        healthPercentageText.text = $"HP: {currentHealth / maxHealth * 100:0}%";
    }
}

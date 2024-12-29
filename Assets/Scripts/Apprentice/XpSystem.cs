using UnityEngine;
using UnityEngine.UI;

public class XPSystem : MonoBehaviour
{
    public Image xpBarImage; // Reference to the XP bar's Image
    public Text skillPointsText;
    public Text xpText;     

    private int currentXP = 0;
    private int maxXP = 100;
    private int skillPoints = 0;

    public void AddXP(int xp)
    {
        currentXP += xp;
        if (currentXP >= maxXP)
        {
            currentXP -= maxXP;
            skillPoints++;
            UpdateSkillPointsUI();
        }
        UpdateXPBar();
    }

    private void UpdateXPBar()
    {
        float fillAmount = (float)currentXP / maxXP; // Calculate fill percentage
        xpBarImage.fillAmount = fillAmount;  // Update the Image fill
        UpdateXPText();              
    }

    private void UpdateSkillPointsUI()
    {
        skillPointsText.text = $"{skillPoints}";
    }

    private void UpdateXPText()
    {
        xpText.text = $"{currentXP}/{maxXP} XP"; // Format the text as "currentXP/maxXP XP"
    }

    public bool SpendSkillPoint()
    {
        if (skillPoints > 0)
        {
            skillPoints--;
            UpdateSkillPointsUI();
            return true;
        }
        return false;
    }
    private void Update()
{
    // Test adding XP with the "X" key
    if (Input.GetKeyDown(KeyCode.X)) 
    {
        Debug.Log("X key pressed. Adding 25 XP.");
        AddXP(25); // Add 25 XP
    }

    // Test spending skill points with the "S" key
    if (Input.GetKeyDown(KeyCode.S)) 
    {
        Debug.Log("S key pressed. Attempting to spend a skill point.");
        if (SpendSkillPoint())
        {
            Debug.Log("Skill point spent successfully!");
        }
        else
        {
            Debug.Log("Not enough skill points to spend.");
        }
    }
}
}

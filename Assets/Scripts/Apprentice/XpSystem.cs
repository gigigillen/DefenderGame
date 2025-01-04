using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class XPSystem : MonoBehaviour
{
    public Image xpBarImage; // Reference to the XP bar's Image
    public TextMeshProUGUI skillPointsText;
    public TextMeshProUGUI xpText;     

    private int currentXP = 0;
    private int maxXP = 100;
    [SerializeField] private int skillPoints = 0;


    public int GetSkillPoints()
    {
    return skillPoints;
    }
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
            UpdateSkillPointsUI(); // Ensure the UI updates
            Debug.Log($"Skill point spent. Remaining skill points: {skillPoints}");
            return true;
        }

        Debug.Log("Not enough skill points to spend.");
        return false;
    }

    private void Update() { 
        UpdateSkillPointsUI();
    }
}

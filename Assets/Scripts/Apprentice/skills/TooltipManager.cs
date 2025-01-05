using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class TooltipManager : MonoBehaviour { 

    public static TooltipManager instance;

    [SerializeField] private GameObject tooltipPanel;
    [SerializeField] private TextMeshProUGUI textComponent;

   
    private void Awake() {

        if (instance == null) {
            instance = this;
        }
        else {
            Destroy(gameObject);
        }

        tooltipPanel.SetActive(false);
    }


    private void Start() {

        Cursor.visible = true;
    }


    private void Update() {

        Vector2 mousePos = Mouse.current.position.ReadValue();
        float offset = 130f;
        transform.position = mousePos + new Vector2(-offset, -offset/2);
    }


    public static string GetTypeColor(ApprenticeType type) {
        switch (type) {
            case ApprenticeType.Wind:
                return "#2B7B12";
            case ApprenticeType.Earth:
                return "#BCB804";
            case ApprenticeType.Water:
                return "#1026B0";
            case ApprenticeType.Fire:
                return "#A83C32";
            default:
                return "white";
        }
    }


    public void SetAndShowTooltip(string title, ApprenticeType type, string description, int cost, bool isPurchased) {

        tooltipPanel.SetActive(true);

        string formattedText = $"<size=120%><color={GetTypeColor(type)}>{title}</color></size>\n\n" +
                         $"{description}\n\n";

        // Create a bottom line with spaces for visual separation
        if (isPurchased) {
            formattedText += $"<color=red>Purchased</color>        <color=#FFD700>Cost: {cost} SP</color>";
        }
        else {
            formattedText += $"<color=#FFD700>Cost: {cost} SP</color>";
        }

        textComponent.text = formattedText;
    }


    public void HideToolTip() {

        Debug.Log("hiding tooltip");
        tooltipPanel.SetActive(false);
        textComponent.text = string.Empty;
    }


}

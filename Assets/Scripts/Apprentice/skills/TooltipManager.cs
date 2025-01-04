using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class TooltipManager : MonoBehaviour { 

    public static TooltipManager instance;

    [SerializeField] private GameObject tooltipPanel;
    [SerializeField] private TextMeshProUGUI textComponent;

    private RectTransform tooltipRect;

   
    private void Awake() {

        if (instance == null) {
            instance = this;
        }
        else {
            Destroy(gameObject);
        }

        tooltipRect = tooltipPanel.GetComponent<RectTransform>();
        tooltipPanel.SetActive(false);
    }


    private void Start() {

        Cursor.visible = true;
    }


    private void Update() {

        Vector2 mousePos = Mouse.current.position.ReadValue();
        float offset = 100f;
        transform.position = mousePos + new Vector2(-offset, -offset/2);
    }


    private string GetTypeColor(ApprenticeType type) {
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


    public void SetAndShowTooltip(string title, ApprenticeType type, string description, int cost) {

        tooltipPanel.SetActive(true);
        string formattedText = $"<size=120%><color={GetTypeColor(type)}>{title}</color></size>\n" +
                             $"{description}\n" +
                             $"<align=right><color=#FFD700>Cost: {cost} SP</color></align>";
        textComponent.text = formattedText;
    }


    public void HideToolTip() {

        tooltipPanel.SetActive(false);
        textComponent.text = string.Empty;
    }


}

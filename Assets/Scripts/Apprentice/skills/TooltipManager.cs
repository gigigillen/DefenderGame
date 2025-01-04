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
    private float offset = 70f;

   
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
        float spaceOnRight = Screen.width - mousePos.x;
        bool shouldShowOnRight = spaceOnRight >= (tooltipRect.rect.width + offset);

        Vector2 tooltipPosition;
        if (shouldShowOnRight) {
            tooltipPosition = mousePos + new Vector2(offset, -offset / 2);
        }
        else {
            tooltipPosition = mousePos + new Vector2(offset - tooltipRect.rect.width - (offset * 2), -offset / 2);
        }

        transform.position = tooltipPosition;
        transform.position = mousePos + new Vector2(offset, -offset/2);
    }


    public void SetAndShowTooltip(string message) {

        tooltipPanel.SetActive(true);
        textComponent.text = message;
    }


    public void HideToolTip() {

        tooltipPanel.SetActive(false);
        textComponent.text = string.Empty;
    }


}

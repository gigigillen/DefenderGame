using UnityEngine;

public class UISelectCanvas : MonoBehaviour {

    private Camera mainCamera;
    [SerializeField] private Vector2 screenOffset = new Vector2(100,0);
    [SerializeField] private RectTransform canvasRectTransform;
    private ApprenticeController currentApprentice;


    private void Awake() {

        mainCamera = Camera.main;
        gameObject.SetActive(false);
    }


    public void SetTargetApprentice(ApprenticeController apprentice) {

        currentApprentice = apprentice;

        // update canvas contents
    }


    private void LateUpdate() {

        if (currentApprentice != null) {

            Vector2 screenPoint = mainCamera.WorldToScreenPoint(currentApprentice.transform.position);
            screenPoint += screenOffset;
            canvasRectTransform.position = screenPoint;
        }
    }
}

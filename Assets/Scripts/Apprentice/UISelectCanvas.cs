using UnityEngine;

public class UISelectCanvas : MonoBehaviour {

    private Camera mainCamera;
    [SerializeField] private Vector3 offset = new Vector3(2, 0, 0);
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

            transform.position = currentApprentice.transform.position + offset;
            transform.LookAt(transform.position + mainCamera.transform.forward);
        }
    }
}

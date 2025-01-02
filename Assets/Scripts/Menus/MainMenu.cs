using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    private void Awake() {

        Time.timeScale = 1f;
    }

    // method called when play button is clicked
    public void OnPlay() {

        Canvas.ForceUpdateCanvases();
        SceneManager.LoadScene(1);
    }

    // method called when quit button is clicked
    public void OnQuit() {
        Application.Quit();
    }
}

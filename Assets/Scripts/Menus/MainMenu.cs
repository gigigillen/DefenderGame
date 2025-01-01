using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour { 

    // method called when play button is clicked
    public void OnPlay() {
        SceneManager.LoadScene(1);
    }

    // method called when quit button is clicked
    public void OnQuit() {
        Application.Quit();
    }
}

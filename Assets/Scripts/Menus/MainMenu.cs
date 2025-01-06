using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    [SerializeField] private Slider waveSlider;
    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private GameObject contents;
    [SerializeField] private GameObject rules;

    public static int NumberOfWaves;

    private void Start()
    {
        waveSlider.interactable = true;

        UpdateWaveText();
        waveSlider.onValueChanged.AddListener(delegate { UpdateWaveText(); });
    }

    //* handles the number of waves in game round
    private void UpdateWaveText()
    {
        waveText.text = $"Waves: {waveSlider.value}";
    }

    public int GetCurrentWave()
    {
        return (int)waveSlider.value;
    }

    public void SetWave(float value)
    {
        waveSlider.value = value;
        UpdateWaveText();
    }
    //*

    public void ReloadScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }

    private void Awake() {

        Time.timeScale = 1f;
    }

    // method called when play button is clicked
    public void OnPlay() {
        NumberOfWaves = (int)waveSlider.value; // Explicitly cast float to int
        Debug.Log($"Selected Wave: {NumberOfWaves}");

        Canvas.ForceUpdateCanvases();
        SceneManager.LoadScene(1);
    }

    public void OnRules() {

        contents.SetActive(false);
        rules.SetActive(true);
    }

    public void OnRulesBack() {

        contents.SetActive(true);
        rules.SetActive(false);
    }

    // method called when quit button is clicked
    public void OnQuit() {
        Application.Quit();
    }
}

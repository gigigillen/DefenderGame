using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour
{

    public static ScoreManager instance;

    public TMP_Text scoreText;

    int score = 10;

    private void Awake()
    {
        instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        scoreText.text = score.ToString() + " points";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddPoint()
    {
        score += 1;
        scoreText.text = score.ToString() + " points";
        
    }
}

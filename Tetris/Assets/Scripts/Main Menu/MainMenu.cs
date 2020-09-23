using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject panel_play;

    [SerializeField]
    private GameObject panel_high_scores;

    private void Awake()
    {
        gameObject.SetActive(true);
    }

    // Go to Play Menu
    public void PlayButon()
    {
        gameObject.SetActive(false);
        panel_play.SetActive(true);
    }
    
    // Go to High Scores menu
    public void HighScoresButton()
    {
        gameObject.SetActive(false);
        panel_high_scores.SetActive(true);
    }


    // Quiting game
    public void ExitButton()
    {
        Application.Quit();
    }
}

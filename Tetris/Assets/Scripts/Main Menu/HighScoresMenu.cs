using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScoresMenu : MonoBehaviour
{
    [SerializeField]
    private Text easyScore;

    [SerializeField]
    private Text mediumScore;

    [SerializeField]
    private Text hardScore;

    [SerializeField]
    private GameObject panel_main_menu;

    
    private void Awake()
    {
        easyScore.text = "Easy: " + PlayerPrefs.GetInt(ConstVar.high_score_easy, 0).ToString();
        mediumScore.text = "Medium: " + PlayerPrefs.GetInt(ConstVar.high_score_medium, 0).ToString();
        hardScore.text = "Hard: " + PlayerPrefs.GetInt(ConstVar.high_score_hard, 0).ToString();
    }

    public void BackButton()
    {
        gameObject.SetActive(false);
        panel_main_menu.SetActive(true);
    }
}

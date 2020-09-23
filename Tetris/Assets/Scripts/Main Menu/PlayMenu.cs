using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject panel_main_menu;


    // In these three voids { EasyButton, MediumButton, HardButon } we change Time.timeScale just to be sure game is running properly if player wants to 
    // play again after losing one game
    public void EasyButton()
    {
        ActiveGame.gameLevel = ActiveGame.difficulty.easy;
        Time.timeScale = 1f;
        SceneManager.LoadScene(ConstVar.sceneGame);
    }

    public void MediumButton()
    {
        ActiveGame.gameLevel = ActiveGame.difficulty.medium;
        Time.timeScale = 1f;
        SceneManager.LoadScene(ConstVar.sceneGame);
    }

    public void HardButton()
    {
        ActiveGame.gameLevel = ActiveGame.difficulty.hard;
        Time.timeScale = 1f;
        SceneManager.LoadScene(ConstVar.sceneGame);
    }

    public void BackButton()
    {
        gameObject.SetActive(false);
        panel_main_menu.SetActive(true);
    }
}

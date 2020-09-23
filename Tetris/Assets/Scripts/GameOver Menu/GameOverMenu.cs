using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    public void ButtonRestart()
    {
        ActiveGame.points = 0;
        Time.timeScale = 1f;
        gameObject.SetActive(false);
        SceneManager.LoadScene(ConstVar.sceneGame);
    }

    public void ButtonMainMenu()
    {
        ActiveGame.points = 0;
        Time.timeScale = 1f;
        SceneManager.LoadScene(ConstVar.sceneMainMenu);
    }
}

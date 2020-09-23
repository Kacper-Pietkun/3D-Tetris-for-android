using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseActivate : MonoBehaviour
{
    private GameObject pauseMenu;

    private void Awake()
    {
        if(transform.childCount > 0)
            pauseMenu = transform.GetChild(0).gameObject;
    }

    public void enablePauseMenu()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ActiveGame
{
    public static int points { get; set; }
    public enum difficulty { easy, medium, hard};
    public static difficulty gameLevel;

    private static float oldTime = 0; // This is time when the last row was destroyed if newTime - oldTIme then we know that these rows where destroyed 
                                      // in the same move
    private static int strike = 1; // if in the same move player destroy 3 rows then stike is equal 3 (Information for us how many blocks where destroyed
                                   // in one move)

    // This function is triggered from SpawnBlocks script
    public static void GameOver()
    {
        // Pausing game after defeat
        Time.timeScale = 0f;

        string save_record = "";

        // Setting new record
        switch (gameLevel)
        {
            case difficulty.easy:
                save_record = ConstVar.high_score_easy;
                break;
            case difficulty.medium:
                save_record = ConstVar.high_score_medium;
                break;
            case difficulty.hard:
                save_record = ConstVar.high_score_hard;
                break;
        }
        if (PlayerPrefs.GetInt(save_record, 0) < points)
            PlayerPrefs.SetInt(save_record, points);
    }

    public static void UpdateTime(float time)
    {
        oldTime = time;
    }

    // This function is trigered from RowManager script
    public static void AddPoints(float newTime)
    {
        int smallPoint = 0;

        switch (gameLevel)
        {
            case difficulty.easy:
                smallPoint = 40;
                break;
            case difficulty.medium:
                smallPoint = 100;
                break;
            case difficulty.hard:
                smallPoint = 200;
                break;
        }

        if (newTime - oldTime < 0.3f)
        {
            strike++;
            smallPoint *= strike;
        }
        else
        {
            strike = 1;
        }
        oldTime = newTime;

        points += smallPoint;
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Narrate;
using UnityEngine.UI;


public class Game_Manager : MonoBehaviour
{
    public static float timeSinceGameStarted =0;
    public static float timeElapsed = 0;
    public static int deathCount = 0;
    public bool showCursor = false;


    //public float highScore;
    //PlayerData Loaded_Data = new PlayerData();

    public void setTimeSinceGameStarted(float time)
    {
        timeSinceGameStarted = time;
    }

    public float getTimeElapsed()
    {
        return timeElapsed;
    }

    public void setDeathCount(int count)
    {
        deathCount = count;
    }
    public int getDeathCount()
    {
        return deathCount;
    }
    void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public void RestartLevel()
    {
        deathCount++;
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Player_Collision.dialogueChecker = false;
        Time.timeScale = 0.0f;
        Initiate.Fade(SceneManager.GetActiveScene().buildIndex, Color.white, 1.0f);

    }

    public void AdvanceLevel()
    {
        Initiate.Fade(SceneManager.GetActiveScene().buildIndex + 1, Color.black, 5.0f);
    }
    private void Start()
    {
        Cursor.visible = showCursor;
    }


    private void Update()
    {
        int currentBuildIndex = SceneManager.GetActiveScene().buildIndex;
        if(currentBuildIndex == 0)
        {
            timeElapsed = 0;
        }else if (currentBuildIndex < 4)
        {
            timeElapsed = Time.time - timeSinceGameStarted;
        }

        DisplayTimerText();


    }

    void DisplayTimerText()
    {
        float high_score = (float)Math.Round(timeElapsed, 2);
        int high_score_seconds = (int)high_score;
        int high_score_minutes = high_score_seconds / 60;
        high_score_seconds = high_score_seconds - high_score_minutes * 60;
        int high_score_milliseconds = (int)((high_score - (int)high_score) * 100);
        if (GameObject.Find("Timer_Text"))
        {
            Text Timer_Text = GameObject.Find("Timer_Text").GetComponent<Text>();
            Timer_Text.text = "Time Taken:   " + high_score_minutes.ToString("00") + ":" + high_score_seconds.ToString("00") + ":" + high_score_milliseconds.ToString("00");
        }
    }

}

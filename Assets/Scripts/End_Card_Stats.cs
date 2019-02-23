using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class End_Card_Stats : MonoBehaviour
{
    private float high_score;
    private int high_score_minutes;
    private int high_score_seconds;
    private int high_score_milliseconds;
    public Text High_Score_Text;
    public Text Death_Counter_Text;
    public Game_Manager gm;
    string playerName;

    string[] NameList = new string[11];
    float[] HighscoreList = new float[11];
    int[] DeathcountList = new int[11];

    // Start is called before the first frame update
    void Start()
    {
        playerName = PlayerPrefs.GetString("Current_Player_Name", "");
        high_score = (float)Math.Round(gm.getTimeElapsed(), 2);
        high_score_seconds = (int)high_score;
        high_score_minutes = high_score_seconds/60;
        high_score_seconds = high_score_seconds - high_score_minutes * 60;
        high_score_milliseconds = (int)((high_score - (int)high_score) *100);

        High_Score_Text.text = "Time   Taken: " + high_score_minutes.ToString("00") + ":" + high_score_seconds.ToString("00") + ":" + high_score_milliseconds.ToString("00");
        Death_Counter_Text.text = "Death   counter:  " + gm.getDeathCount().ToString();

        //  Saving details of current gameplay into last element of array
        NameList = updateNameList();
        HighscoreList = updateHighscoreList();
        DeathcountList = updateDeathcountList();

        //Debug.Log("Moving all null values to the back of the array:");
        MoveNullValuesToTheBack(ref NameList, ref HighscoreList, ref DeathcountList);
        // sort the values in increasing order
        SortList(ref NameList, ref HighscoreList, ref DeathcountList);
        //save the values
        SaveHighScore(NameList, HighscoreList, DeathcountList);
        //printArray();
    }

    string[] updateNameList()
    {
        string[] NameList = new string[11];
        for(int i=0; i<10; i++)
        {
            NameList[i] = PlayerPrefs.GetString("Name" + i, "");
        }
        NameList[10] = playerName;
        return NameList;
    }
    float[] updateHighscoreList()
    {
        float[] HighscoreList = new float[11];
        for (int i = 0; i < 10; i++)
        {
            HighscoreList[i] = PlayerPrefs.GetFloat("HighScore" + i, -1f);
        }
        HighscoreList[10] = high_score;
        return HighscoreList;
    }
    int[] updateDeathcountList()
    {
        int[] DeathcountList = new int[11];
        for(int i=0; i<10; i++)
        {
            DeathcountList[i] = PlayerPrefs.GetInt("DeathCounter" + i, -1);
        }
        DeathcountList[10] = gm.getDeathCount();
        return DeathcountList;
    }

    void MoveNullValuesToTheBack(ref string[] NameList, ref float[] HighscoreList, ref int[] DeathcountList)
    {
        string[] tempNameList = new string[11];
        float[] tempHighscoreList = new float[11];
        int[] tempDeathcountList = new int[11];
        int counter = 0;
        for(int i=0; i<NameList.Length; i++)
        {
            if(NameList[i] != "")
            {
                tempNameList[counter] = NameList[i];
                tempHighscoreList[counter] = HighscoreList[i];
                tempDeathcountList[counter] = DeathcountList[i];
                counter++;
            }
        }
        //counter will be the first null index in the array
        for (int i = counter; i < NameList.Length; i++) {
            tempNameList[i] = "";
            tempHighscoreList[i] = -1f;
            tempDeathcountList[i] = -1;
        }

        NameList = tempNameList;
        HighscoreList = tempHighscoreList;
        DeathcountList = tempDeathcountList;

    }

    void SortList(ref string[] NameList, ref float[] HighscoreList, ref int[] DeathcountList)
    {
        int nullcounter = 0;
        for(int i=0; i < NameList.Length; i++) {
            if(DeathcountList[i] == -1)
            {
                nullcounter = i;
                break;
            }
        }

        // bubble sort
        for (int i = 0; i < nullcounter - 1; i++)
        {
            for (int j = 0; j < nullcounter - i - 1; j++)
            {
                if (HighscoreList[j] > HighscoreList[j + 1])
                {
                    // swap temp and arr[i] 
                    float tempscore = HighscoreList[j];
                    string tempname = NameList[j];
                    int tempdeathcount = DeathcountList[j];

                    NameList[j] = NameList[j + 1];
                    HighscoreList[j] = HighscoreList[j + 1];
                    DeathcountList[j] = DeathcountList[j + 1];

                    NameList[j + 1] = tempname;
                    HighscoreList[j + 1] = tempscore;
                    DeathcountList[j + 1] = tempdeathcount;
                }
            }
        }
    }

    void SaveHighScore(string[] NameList, float[] HighscoreList, int[] DeathcountList)
    {
        for (int i = 0; i < NameList.Length; i++)
        {
            PlayerPrefs.SetString("Name" + i, NameList[i]);
            PlayerPrefs.SetFloat("HighScore" + i, HighscoreList[i]);
            PlayerPrefs.SetInt("DeathCounter" + i, DeathcountList[i]);
        }
    }
    void printArray()
    {
        for (int i = 0; i < NameList.Length; i++)
        {
            Debug.Log(NameList[i] + ", highscore: " + HighscoreList[i].ToString() + ", deathcount: " + DeathcountList[i].ToString());
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Load_HighScores : MonoBehaviour
{
    Game_Manager gm;
    public GameObject ScorePrefab;
    string[] HighScore_Names;
    float[] HighScores;
    int[] HighScore_DeathCounter;
    // Start is called before the first frame update
    void Start()
    {

        for (int i=0; i<10; i++)
        {
            CreateHighScores(i+1, PlayerPrefs.GetString("Name" + i, ""), PlayerPrefs.GetFloat("HighScore" + i, -1f), PlayerPrefs.GetInt("DeathCounter" + i, -1));
        }

    }

    public void displayScores()
    {
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        for (int i = 0; i < 10; i++)
        {
            CreateHighScores(i+1, PlayerPrefs.GetString("Name" + i, ""), PlayerPrefs.GetFloat("HighScore" + i, -1f), PlayerPrefs.GetInt("DeathCounter" + i, -1));
        }
    }

    void CreateHighScores(int rank, string name, float highscore, int deathCounter)
    {
        gm = GameObject.Find("GameManager").GetComponent<Game_Manager>();


        GameObject go = Instantiate(ScorePrefab);
        go.transform.SetParent(this.transform);
        go.transform.localScale = new Vector3(1, 1, 1);
        //rank
        go.transform.GetChild(0).GetComponent<Text>().text = "#" + rank.ToString();
        //name
        go.transform.GetChild(1).GetComponent<Text>().text = name;
        //time completed
        if (highscore.Equals(-1f)) { go.transform.GetChild(2).GetComponent<Text>().text = ""; }
        else
        {
            int high_score_seconds = (int)highscore;
            int high_score_minutes = high_score_seconds / 60;
            high_score_seconds = high_score_seconds - high_score_minutes * 60;
            int high_score_milliseconds = (int)((highscore - (int)highscore) * 100);
            go.transform.GetChild(2).GetComponent<Text>().text = high_score_minutes.ToString("00") + ":" + high_score_seconds.ToString("00") + ":" + high_score_milliseconds.ToString("00");
        }
        //death counter
        if (deathCounter == -1) { go.transform.GetChild(3).GetComponent<Text>().text = ""; }
        else
        {
            go.transform.GetChild(3).GetComponent<Text>().text = deathCounter.ToString();
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Death_Counter : MonoBehaviour
{
    Game_Manager gm;

    private void Start()
    {
        if (GameObject.Find("GameManager")) { gm = GameObject.Find("GameManager").GetComponent<Game_Manager>(); }
        else { Debug.LogError("Scene needs a game manager"); }
    }
    // Update is called once per frame
    void Update()
    {
        GetComponent<Text>().text = "Death Counter: " + gm.getDeathCount().ToString();
    }
}

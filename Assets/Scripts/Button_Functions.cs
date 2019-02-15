using Hellmade.Sound;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Button_Functions : MonoBehaviour
{
    Game_Manager gm;
    Audio_Manager am;
    bool showPopup = false;
    public int okWidth, okXPos, okYPos;
    public int okHeight;
    string playerName;
    bool showVolume = false;
    private void Start()
    {
        if (GameObject.Find("AudioManager")) { am = GameObject.Find("AudioManager").GetComponent<Audio_Manager>(); }
        //else { Debug.LogError("Scene needs an audio manager"); }
        if (GameObject.Find("globalVol")) { GameObject.Find("globalVol").GetComponent<Slider>().value = am.globalVolume; }
        if (GameObject.Find("globalMusicVol")) { GameObject.Find("globalMusicVol").GetComponent<Slider>().value = am.globalMusicVolume; }
        if (GameObject.Find("globalSoundVol")) { GameObject.Find("globalSoundVol").GetComponent<Slider>().value = am.globalSoundFXVolume; }

        if (this.gameObject.transform.Find("ControlsPanel"))
        {
            this.gameObject.transform.Find("ControlsPanel").gameObject.SetActive(showVolume);
        }
    }
    public void StartGame()
    {
        if (GameObject.Find("GameManager")) { gm = GameObject.Find("GameManager").GetComponent<Game_Manager>(); }
        else { Debug.LogError("Scene needs a game manager"); }
        gm.setDeathCount(0);
        gm.setTimeSinceGameStarted(Time.time);
        playerName = GameObject.Find("Name_Text").GetComponent<Text>().text;
        PlayerPrefs.SetString("Current_Player_Name", playerName);
        if(playerName.Length > 10 || playerName.Length == 0) {
            showPopup = true;
            return;
        }
        gm.AdvanceLevel();
    }

    public void VolumeSettings()
    {
        showVolume = !showVolume;
        this.gameObject.transform.Find("ControlsPanel").gameObject.SetActive(showVolume);
    }
    public void GlobalVolumeChanged()
    {
        EazySoundManager.GlobalVolume = GameObject.Find("globalVol").GetComponent<Slider>().value;
    }

    public void GlobalMusicVolumeChanged()
    {
        EazySoundManager.GlobalMusicVolume = GameObject.Find("globalMusicVol").GetComponent<Slider>().value; 
        
    }

    public void GlobalSoundVolumeChanged()
    {
        EazySoundManager.GlobalSoundsVolume = GameObject.Find("globalSoundVol").GetComponent<Slider>().value;
    }

    private void OnGUI()
    {
        if (showPopup)
        {
            GUI.Window(0, new Rect((Screen.width / 2) - 150, (Screen.height / 2) - 75
                   , 300, 250), ShowGUI, "Invalid Name");

        }
    }
    void ShowGUI(int windowID)
    {
        // You may put a label to show a message to the player

        GUI.Label(new Rect(65, 40, 200, 60), playerName.Length > 10 ? "Name must be less than 10 characters long" : playerName.Length == 0 ? "Please enter a name" : "");

        // You may put a button to close the pop up too

        if (GUI.Button(new Rect(90, 180, 100, 30), "OK"))
        {
            showPopup = false;
            // you may put other code to run according to your game too
        }

    }
    public void BackToMain()
    {
        if (GameObject.Find("LobbyManager"))
        {
            Destroy(GameObject.Find("LobbyManager"));
        }
        if (GameObject.Find("GameManager"))
        {
            Destroy(GameObject.Find("GameManager"));
        }
        Debug.Log("Going back to main");
        SceneManager.LoadScene("starting");
    }

    public void ResetHighscore()
    {
        PlayerPrefs.DeleteAll();
        GameObject.Find("Scores").GetComponent<Load_HighScores>().displayScores();
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMultiplayer()
    {
        SceneManager.LoadScene("Lobby");
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game");
        Application.Quit();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Setup_Local_Player : NetworkBehaviour
{
    [SyncVar]   //sync var get pushed out from server to clients whenever its changed.
    public string Player_Name = "player";
    [SyncVar]
    public bool isPaused = false;

    private void OnGUI()
    {
        if (isLocalPlayer)
        {
            Player_Name = GUI.TextField(new Rect(25, Screen.height - 40, 100, 30), Player_Name);
            if (GUI.Button(new Rect(130, Screen.height - 40, 80, 30), "Change"))
            {
                CmdChangeName(Player_Name);
            }
            this.transform.GetChild(3).gameObject.SetActive(true);
        }
    }

    public void EndGame()
    {
        if (isLocalPlayer)
        {
            CmdEndGame();
        }
    }

    [Command]   // command from client to server
    public void CmdChangeName(string NewName)
    {
        Player_Name = NewName;
    }

    [Command]
    public void CmdEndGame()
    {
        isPaused = true;
    }
    // Start is called before the first frame update
    void Start()
    {
        if (isLocalPlayer)
        {
            Time.timeScale = 1f;
            GetComponent<Player_Controller>().enabled = true;
        }
    }

    private void Update()
    {
        GetComponentInChildren<TextMesh>().text = Player_Name;
        if (isPaused)
        {
            Time.timeScale = 0f;
            transform.Find("Canvas").transform.Find("Instruction_Text").GetComponent<Text>().text = Player_Name + " has won! ";
        }
    }


}

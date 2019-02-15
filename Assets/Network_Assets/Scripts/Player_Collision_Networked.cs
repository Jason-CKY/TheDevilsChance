using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
public class Player_Collision_Networked : NetworkBehaviour
{
    //public GameObject[] diamond_Prefabs;
    //GameObject[] diamonds;
    public Text score_Text;
    public float Restart_YPos = -1f;
    public float Advance_YPos = -5f;
    public int numOfCollectibles = 3;
    Animator Door_Animator;
    Audio_Manager am;
    [HideInInspector]
    public int score = 0;
    Vector3 startingPos;

    [SyncVar]
    public bool openDoor = false;
    private void Start()
    {
        if (isLocalPlayer)
        {
            startingPos = transform.position;
            //if (GameObject.Find("Count_Text")) { score_Text = GameObject.Find("Count_Text").GetComponent<Text>(); }
            //else { Debug.LogError("Scene needs score text"); }
            score_Text = transform.Find("Canvas").transform.Find("Count_Text").GetComponent<Text>();
            if (GameObject.Find("AudioManager")) { am = GameObject.Find("AudioManager").GetComponent<Audio_Manager>(); }
            if (GameObject.FindGameObjectWithTag("Door"))
            {
                Door_Animator = GameObject.FindGameObjectWithTag("Door").GetComponent<Animator>();
            }
        }
    }
    private void Update()
    {
        if (score >= numOfCollectibles && transform.position.y < Advance_YPos)
        {
            GetComponent<Setup_Local_Player>().EndGame();
        }else if(score < numOfCollectibles && transform.position.y < Restart_YPos)
        {
            transform.position = startingPos;
        }
        if (isLocalPlayer)
        {
            score_Text.text = "Diamonds Collected: " + score.ToString() + "/" + numOfCollectibles.ToString();
        }

        if (openDoor)
        {
            Door_Animator = GameObject.FindGameObjectWithTag("Door").GetComponent<Animator>();
            Door_Animator.SetBool("isOpen", true);
        }
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (isLocalPlayer)
        {
            if (collision.collider.tag.Equals("Collectible"))
            {
                am.PlaySoundFx("Collect_Sound");
                CmdDestroyDiamonds(collision.collider.gameObject);
                score++;
                if (score == numOfCollectibles)
                {
                    CmdOpenDoor();
                    am.PlaySoundFx("Door_Open");
                }
            }
            else if (collision.collider.tag.Equals("Enemy"))
            {
                transform.position = startingPos;
                am.PlaySoundFx("Dying_Sound");
            }
        }
    }
    [Command]
    void CmdOpenDoor() {
        openDoor = true;
    }
    [Command]
    void CmdDestroyDiamonds(GameObject diamond)
    {
        NetworkServer.Destroy(diamond);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    Player_Collision pc;
    Animator anim;
    Audio_Manager am;
    bool playSound = false;
    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.FindGameObjectWithTag("Player")) { pc = GameObject.FindGameObjectWithTag("Player").GetComponent<Player_Collision>(); }
        else { Debug.LogError("Scene needs Player"); }
        if (GameObject.Find("AudioManager")) { am = GameObject.Find("AudioManager").GetComponent<Audio_Manager>(); }
        else { Debug.LogError("Scene needs an audio manager"); }
        if (GetComponent<Animator>()){
            anim = GetComponent<Animator>();
        }
        else
        {
            Debug.LogError("Door script needs animator component");
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if(pc.score >= pc.numOfCollectibles)
        {
            playSound = true;
            anim.SetBool("isOpen", true);
            GetComponent<BoxCollider>().enabled = false;
        }
        else
        {
            anim.SetBool("isOpen", false);
        }

        if (playSound)
        {
            am.PlaySoundFx("Door_Open");
            playSound = false;
        }
    }
}

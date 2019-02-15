 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Hellmade.Sound;

public class Player_Collision : MonoBehaviour
{
    Text score_Text;
    public float Restart_YPos = -1f;
    public float Advance_YPos = -5f;
    public int numOfCollectibles = 3;
    public static bool dialogueChecker = false;
    Animator Door_Animator;
    Audio_Manager am;
    Game_Manager gm;
    Audio Dialogue_Audio;
    [HideInInspector]
    public int score = 0;

    private void Start()
    {
        if (GameObject.Find("Count_Text")) { score_Text = GameObject.Find("Count_Text").GetComponent<Text>(); }
        else { Debug.LogError("Scene needs score text"); }
        if (GameObject.Find("GameManager")) { gm = GameObject.Find("GameManager").GetComponent<Game_Manager>(); }
        else { Debug.LogError("Scene needs a game manager"); }
        if (GameObject.Find("AudioManager")) { am = GameObject.Find("AudioManager").GetComponent<Audio_Manager>(); }
        else { Debug.LogError("Scene needs an audio manager"); }
        if (GameObject.FindGameObjectWithTag("Door").GetComponent<Animator>())
        {
            Door_Animator = GameObject.FindGameObjectWithTag("Door").GetComponent<Animator>();
        }
        else
        {
            Debug.LogError("Door needs animator component");
        }
    }
    private void Update()
    {
        if(score < numOfCollectibles && transform.position.y < Restart_YPos)
        {
            gm.RestartLevel();
        }else if(score >= numOfCollectibles && transform.position.y < Advance_YPos)
        {
            gm.AdvanceLevel();
        }
        score_Text.text = "Diamonds Collected: " + score.ToString() + "/" + numOfCollectibles.ToString();

        if (Dialogue_Audio != null && dialogueChecker == true)
        {
            //Player_Controller.canControl = false;
            EazySoundManager.GlobalMusicVolume = 0.0f;
            if (Dialogue_Audio.IsPlaying != true)
            {
                EazySoundManager.GlobalMusicVolume = 0.5f;
            }
        }
        else
        {
            //Player_Controller.canControl = true;
            EazySoundManager.GlobalMusicVolume = 0.5f;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag.Equals("Collectible"))
        {
            //collision.collider.enabled = false;
            am.PlaySoundFx("Collect_Sound");
            Destroy(collision.gameObject);
            score++;
            if(score == numOfCollectibles)
            {
                Door_Animator.SetBool("isOpen", true);
                am.PlaySoundFx("Door_Open");
            }
        }
        else if (collision.collider.tag.Equals("Enemy"))
        {
            am.PlaySoundFx("Dying_Sound");
            gm.RestartLevel();
        }

        if (collision.collider.tag.Equals("Starter_Platform") && dialogueChecker == false)
        {
            Dialogue_Audio = EazySoundManager.GetAudio(am.PlaySoundFx("Starter_Voice"));
            dialogueChecker = true;

        }


    }
}

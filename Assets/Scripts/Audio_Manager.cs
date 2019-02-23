using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hellmade.Sound;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Audio_Manager : MonoBehaviour
{
    public AudioClip[] BG_List;
    public AudioClip Collect_Sound, Door_Open, Dying_Sound, Starting_BGM, Ending_BGM, Starter_Voice;

    public Audio[] BG_List_Audio;
    public Audio Collect_Sound_Audio, Door_Open_Audio, Dying_Sound_Audio, Starting_BGM_Audio, Ending_BGM_Audio, Starter_Voice_Audio;


    public float globalVolume = 1f, globalMusicVolume = 0.5f, globalSoundFXVolume = 1f;

     // Start is called before the first frame update
    void Start()
    {
        BG_List_Audio = new Audio[BG_List.Length];
        for (int i = 0; i < BG_List.Length; i++)
        {
            BG_List_Audio[i] = EazySoundManager.GetAudio(EazySoundManager.PrepareMusic(BG_List[i], globalMusicVolume, true, false, 0.5f, 1));
        }
        Starting_BGM_Audio = EazySoundManager.GetAudio(EazySoundManager.PrepareMusic(Starting_BGM, globalMusicVolume, true, false, 0.5f, 1));
        Ending_BGM_Audio = EazySoundManager.GetAudio(EazySoundManager.PrepareMusic(Ending_BGM, globalMusicVolume, true, false, 0.5f, 1));

        // playing the appropriate bgm
        int currentBuildIndex = SceneManager.GetActiveScene().buildIndex;
        if (SceneManager.GetActiveScene().name.Equals("Multiplayer_Scene")){
            BG_List_Audio[0].Play();
        }
        else{
            switch (currentBuildIndex)
            {
                case 0:
                    Starting_BGM_Audio.Play();
                    break;
                case 1:
                    BG_List_Audio[0].Play();
                    break;
                case 2:
                    BG_List_Audio[1].Play();
                    break;
                case 3:
                    BG_List_Audio[2].Play();
                    break;
                case 4:
                    Ending_BGM_Audio.Play();
                    break;
            }
        }
    }

    public int PlaySoundFx(string fxname)
    {
        if (fxname.Equals("Collect_Sound"))
        {
            return EazySoundManager.PlaySound(Collect_Sound, globalSoundFXVolume);
        }else if (fxname.Equals("Door_Open"))
        {
            return EazySoundManager.PlaySound(Door_Open, globalSoundFXVolume);
        }
        else if (fxname.Equals("Dying_Sound"))
        {
            return EazySoundManager.PlaySound(Dying_Sound, globalSoundFXVolume);
        }else if (fxname.Equals("Starter_Voice"))
        {
            return EazySoundManager.PlaySound(Starter_Voice, globalSoundFXVolume);
        }
        else
        {
            return -1;
        }
    }
}

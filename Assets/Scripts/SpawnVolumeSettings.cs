using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnVolumeSettings : MonoBehaviour
{
    bool showVolume = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            showVolume = !showVolume;
        }

        this.gameObject.transform.Find("ControlsPanel").gameObject.SetActive(showVolume);
        Cursor.visible = showVolume;

    }
}

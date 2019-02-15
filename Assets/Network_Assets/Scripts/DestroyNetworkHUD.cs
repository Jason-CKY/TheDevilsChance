using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyNetworkHUD : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.Find("Network_Manager"))
        {
            Destroy(GameObject.Find("Network_Manager"));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

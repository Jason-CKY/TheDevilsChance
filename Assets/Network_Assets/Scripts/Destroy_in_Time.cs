using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Destroy_in_Time : MonoBehaviour
{
    public float Time_To_Destroy = 2f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!GetComponent<Text>().text.Equals(""))
        {
            Invoke("destroyThis", Time_To_Destroy);
        }
    }

    void destroyThis()
    {
        GetComponent<Text>().text = "";
    }
}

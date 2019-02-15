using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FixPosition : MonoBehaviour
{
    public float yPosFixed_Level1 = 2f;
    public float yPosFixed_Level2 = 2f;
    public float yPosFixed_Level3 = 2f;

    // Start is called before the first frame update
    void Start()
    {

        //transform.position = new Vector3(1, 0, 1);
    }

    // Update is called once per frame
    void Update()
    {
        float yPos = 0;
        int currentBuildIndex = SceneManager.GetActiveScene().buildIndex;
        switch (currentBuildIndex)
        {
            case 1:
                yPos = yPosFixed_Level1;
                break;
            case 2:
                yPos = yPosFixed_Level2;
                break;
            case 3:
                yPos = yPosFixed_Level3;
                break;
        }
        transform.position = new Vector3(transform.position.x, yPos, transform.position.z);
    }
}

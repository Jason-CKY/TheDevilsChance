using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vanish_On_Touch : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag.Equals("Player"))
        {
            Debug.Log("Collided!");
            gameObject.SetActive(false);
            Invoke("appearLater", 1f);
        }
    }

    void appearLater()
    {
        gameObject.SetActive(true);
    }
}
